var SaveList = function () {

    var userEmailList = [];
    var commaSeparatedUserEmails = "";

    // listener for selected users in the user checkbox
    $("#ItemList li input[type=checkbox]").each(function (index, val) {

        var selectedUserId = $(val).attr("id");

        var selectedUserEmail = $(val).attr("value");

        var IsSelected = $("#" + selectedUserId).is(":checked", true);

        if (IsSelected) {

            userEmailList.push(selectedUserEmail);

        }
    })

    if (userEmailList.length != 0) {

        // save all the selected user as a single String
        commaSeparatedUserEmails = userEmailList.toString();

        // display the selected users on the input box
        $("#ToEmail").val(commaSeparatedUserEmails);
    }
}