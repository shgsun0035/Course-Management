var hub = $.connection.chatHub;
var currentUserList = [];

// view the online user in the chatroom
$("#onlineUserList").click(function () {

    $("#user").empty();
    hub.server.getAllActiveConnections().done(function (connections) {
        currentUserList = removeDuplicates(connections);
        $.map(currentUserList, function (user) {
            $("#user").append("<li>Online User: " + user + "</li>");
        });
    });
});

// display the message in the chatroom
hub.client.message = function (message) {
    console.log("message", message);
    $("#message").append("<li>" + message + "</li>")
}

// listener for sending message
$.connection.hub.start(function () {
    $("#send").click(function () {
        hub.server.send($("#txt").val());
        $("#txt").val("");
    })
})

// filter the duplicated online user
function removeDuplicates(data) {
    var uniqueUserList = [];
    data.forEach(function (item) {
        if (!uniqueUserList.includes(item)) {
            uniqueUserList.push(item);
        }
    });
    return uniqueUserList;
}
