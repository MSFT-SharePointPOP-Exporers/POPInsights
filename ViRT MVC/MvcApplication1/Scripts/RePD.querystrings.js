var date = new Date();
var upper = 99.9057654764;
var lower = 91.054;
var reliability_val;
var performance_val;
var QoS_val = 99.186453654363;
var latency;


function setDefaults() {
    sessionStorage["team"] = "Authentication";
    sessionStorage["month"] = new Date(date.getFullYear(),date.getMonth()-1, 1).toString("yyyy-MM-dd");

}

function updateQueryString() {
    if (sessionStorage["team"] == undefined) {
        setDefaults();
    }
    sessionStorage["query"] = "?team=" + sessionStorage["team"] + "&month=" + sessionStorage["month"];
    window.location.search = sessionStorage["query"];
}

function setSessionStorage() {
    sessionStorage["team"] = $.QueryString("team");
    sessionStorage["month"] = $.QueryString("month");
}

function setFields() {
    $(".month").val($.QueryString("month"));
}

function setTeam(id) {
    sessionStorage["team"] = id;
    updateQueryString();
}


/*
  Changes the background color and the text color of the team selected. This is determined by the query string.
*/
function setSelectedTeam() {
    $("#" + sessionStorage["team"]).css({
        "background-color": "#008cba",
    });

    $("#" + sessionStorage["team"] + " a:not(.button)").css({
        "color": "black"
    });
}

$(document).ready(function () {
    $(document).foundation();
    if (window.location.search == "") {
        updateQueryString();
    } else {
        setSessionStorage();
        setSelectedTeam();
        setFields();
    }
    if (sessionStorage["team"] != "Authentication") {
        $("#overallstats").hide();
        $("#chartdiv").hide();
        $("#PerformanceDiv").hide();
        $("#rendering").append("<h2>Data Unavailable<br/>:(</h2>");
    }
});

//Move Elsewhere!
$(document).ready(function () {
    $('.month').datepicker({
        defaultDate: sessionStorage["month"],
        changeMonth: true,
        changeYear: true,
        minDate: "-4M -0Y",
        maxDate: "-1M -0Y",
        showButtonPanel: true,
        dateFormat: 'yy-mm-dd',
        onClose: function (dateText, inst) {
            var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(".month").val(new Date(year, month, 1).toString("yyyy-MM-dd"));
            sessionStorage["month"] = $(".month").val();
            updateQueryString();
        },
        beforeShow: function (input, inst) {
            var datestr;
            if ((datestr = $(this).val()).length > 0) {
                year = datestr.substring(datestr.length - 4, datestr.length);
                month = jQuery.inArray(datestr.substring(0, datestr.length - 5), $(this).datepicker('option', 'monthNamesShort'));
//                $(this).datepicker('option', 'defaultDate', new Date(year, month, 1));
//                $(this).datepicker('setDate', new Date(year, month, 1));
            }
        }
    });
    //$("#loading").fadeOut("slow");
});