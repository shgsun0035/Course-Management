var events = [];
var selectedEvent = [];
console.log("test", "1");
$(".events").each(function () {
    var eventID = $(".courseId", this).text().trim();
    var title = $(".title", this).text().trim();
    var start = $(".start", this).text().trim();
    var end = $(".end", this).text().trim();
    var event = {
        "eventID": eventID,
        "title": title,
        "start": start,
        "end": end
    };
    events.push(event);
});
$("#calendar").fullCalendar({
    contentHeight: 500,
    // calender tabs for month, week and day
    header: {
        left: 'prev, next today',
        center: 'title',
        right:'month, agendaWeek, agendaDay'
    },
    locale: 'au',
    events: events,
    // redirect to the create course page
    dayClick: function (date, allDay, jsEvent, view) {
        var d = new Date(date);
        var m = moment(d).format("YYYY/MM/DD");
        m = encodeURIComponent(m);
        var uri = "/Courses/Create?date=" + m;
        $(location).attr('href', uri);
    },
    // redirect to detail course page
    eventClick: function (calEvent, jsEvent, view) {
        selectedEvent = calEvent;
        var selectedEventId = selectedEvent.eventID;
        var uri = "/Courses/Details/" + selectedEventId;
        $(location).attr('href', uri);
    }
});

