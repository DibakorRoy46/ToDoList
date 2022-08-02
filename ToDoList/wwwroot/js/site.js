

$(() => {
    LoadToDoList();
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();
    connection.start();
    connection.on("LoadToDo", function () {
        LoadToDoList();
    });

    function LoadToDoList() {
        $.ajax({
            url: '/ToDo/ToDoTable/',
            type: "GET"
        }).done(function (response) { 
            $("#table-container").html(response);
        })
    }
});