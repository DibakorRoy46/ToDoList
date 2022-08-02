


$(() => {
    LoadToDoList();
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();
    connection.start();
    connection.on("LoadToDo", function () {
        LoadToDoList();
    });
    LoadToDoList();

    function LoadToDoList() {
        $.ajax({
            url: '/ToDo/ToDoTable/',
            type: "GET"
        }).done(function (response) {
            $("#table-container").html(response);
        }).fail(function (xhr) {
            /*toastr.error("Something went wrong");*/
        })
    }
});