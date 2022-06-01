"use strict";

window.onload = function () {
    $.ajax({
        method: "GET",
        url: "/MessagesMVC/GetAllMessages",
        success: function (msg) {
            console.log("Initial data loaded successfully!");
        },
        error: function () {
            console.log("Error at loading initial data!");
        }
    }).done(function (msgs) {
        for (var i = 0; i < msgs.length; i++) {

            var li = document.createElement("p");
            document.getElementById("messagesList").appendChild(li);
            li.innerHTML = "<b>" + msgs[i].userName + "</b><br/>" + msgs[i].messageText + "<br/><span style='float: right;'>" + " " + new Date(msgs[i].createdDate).getHours() + ":" + new Date(msgs[i].createdDate).getMinutes() + " &#10003;&#10003;</span>";
            li.classList.add("signalr-chart-message-left");
        }
    });
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    if (user != "") {
        var li = document.createElement("p");
        document.getElementById("messagesList").appendChild(li);
        var current = new Date();
        li.innerHTML = "<b>" + user + "</b><br/>" + message + "<br/><span style='float: right;'>" + " " + current.getHours() + ":" + current.getMinutes() + " &#10003;&#10003;</span>";
        if (user === document.getElementById("userName").value) {
            li.classList.add("signalr-chart-message-right");
        } else {
            li.classList.add("signalr-chart-message-left");
        }
        $.ajax({
            method: "POST",
            url: "/MessagesMVC/InsertMessage",
            data: {
                messageText: message,
                userName: user
            },
            success: function (result) { }
        });
    } else {
        document.getElementById("message-error").style = "display: block;";
    }
    
});

"message-add-user"

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userName").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    document.getElementById("messageInput").value = '';
    event.preventDefault();
});

document.getElementById("enterName").addEventListener("click", function (event) {
    //document.getElementById("userName").disabled = true;
    document.getElementById("enterName").disabled = true;
    document.getElementById("enterName").value = "Enter name ✔";
    event.preventDefault();
    $.ajax({
        method : "POST",
        url: "/UsersMVC/Create",
        data: {
            userName: document.getElementById("userName").value
        },
        success: function (result) {
            document.getElementById("message-add-user").style = "display: block;";
        }
    });
});