using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using ToDoList.DataAccess.IRepository;
using ToDoList.Models.Models;
using ToDoList.Models.ViewModels;
using ToDoList.Utility;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly IUnitOfWork _unitOfWrok;
        private readonly IHubContext<SignalRServer> _signalHub;
        public ToDoController(IUnitOfWork unitOfWork, IHubContext<SignalRServer> signalHub)
        {
            _unitOfWrok = unitOfWork;
            _signalHub = signalHub;
        }
        #region Index
        [Route("")]
        [Route("ToDo")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> ToDoTable(string searchValue, int pageNo, int pageSize)
        {
            pageNo = pageNo != 0 ? pageNo : 1;
            pageSize = 10;
            var numberOfToDo = await _unitOfWrok.ToDo.CountAsync(searchValue);
            ToDoVM toDoVM = new ToDoVM()
            {
                ToDoList = await _unitOfWrok.ToDo.SearchAsync(searchValue, pageNo, pageSize),
                Search = searchValue,
                Pager = new Pager(numberOfToDo, pageNo, pageSize)
            };
            return PartialView("_ToDoTable", toDoVM);
        }
        #endregion
        #region Upsert
        [Authorize(Roles ="Admin")]
        [Route("ToDo/Upsert")]
        public async Task<IActionResult> Upsert(Guid id)
        {
            try
            {
                ToDo toDo = new ToDo();
                if (id.Equals(Guid.Empty))
                {
                    return View(toDo);
                }
                else
                {
                    toDo = await _unitOfWrok.ToDo.FirstOrDefaultAsync(x => x.Id.Equals(id));
                    if (toDo == null)
                    {
                        return NotFound();
                    }
                    return View(toDo);
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [HttpPost]
        [Route("ToDo/Upsert")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(ToDo toDo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (toDo.Id.Equals(Guid.Empty))
                    {
                        await _unitOfWrok.ToDo.AddAsync(toDo);
                        TempData["message"] = "Successfully Created";
                    }
                    else
                    {
                        await _unitOfWrok.ToDo.UpdateAsync(toDo);
                        TempData["message"] = "Successfully Updated";
                    }
                    await _unitOfWrok.SaveAsync();
                    await _signalHub.Clients.All.SendAsync("LoadToDo");
                    return RedirectToAction(nameof(Index));
                }
                return View(toDo);

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        #endregion
        #region Delete
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (!id.Equals(Guid.Empty))
                {
                    var toDoObj = await _unitOfWrok.ToDo.FirstOrDefaultAsync(x => x.Id.Equals(id));
                    if (toDoObj == null)
                    {
                        return NotFound();
                    }
                    await _unitOfWrok.ToDo.RemoveAsync(toDoObj);
                    await _unitOfWrok.SaveAsync();
                    await _signalHub.Clients.All.SendAsync("LoadToDo");
                    int pageNo = 1;
                    int pageSize = 10;
                    var numberOfToDo = await _unitOfWrok.ToDo.CountAsync(null);
                    ToDoVM toDoVM = new ToDoVM()
                    {
                        ToDoList = await _unitOfWrok.ToDo.SearchAsync(null, pageNo, pageSize),
                        Search = null,
                        Pager = new Pager(numberOfToDo, pageNo, pageSize)
                    };
                    return PartialView("_ToDoTable", toDoVM);

                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        #endregion

        #region ToDoDone

        public async Task<IActionResult> ToDoDone(Guid id, int pageNo, string searchValue)
        {
            try
            {
                if (!id.Equals(Guid.Empty))
                {
                    var toDo = await _unitOfWrok.ToDo.FirstOrDefaultAsync(x => x.Id == id);
                    if (toDo == null)
                    {
                        return NotFound();
                    }
                    if (toDo.Status==false)
                    {
                        toDo.Status = true;
                        await _unitOfWrok.SaveAsync();
                        await _signalHub.Clients.All.SendAsync("LoadToDo");
                        return Json(new {success=true, message = "Successfully Done" });
                    }
                    else {
                        toDo.Status = false;
                        await _unitOfWrok.SaveAsync();
                        await _signalHub.Clients.All.SendAsync("LoadToDo");
                        return Json(new { success = false, message = "Undo" });
                    }
                    

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion
    }
}