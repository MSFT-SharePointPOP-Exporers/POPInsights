/*
    Sets the sessionStorage variables to their default values.
*/
function setDefaults() {
	sessionStorage["start"] = new Date.parse("t - 8 d").toString("yyyy-MM-dd");
	sessionStorage["end"] = new Date.parse("t - 1 d").toString("yyyy-MM-dd");
	sessionStorage["pipeline"] = "Overview";
	sessionStorage["datacen"] = "All";
	sessionStorage["network"] = -1;
	sessionStorage["farm"] = -1;
	sessionStorage["query"] = "start=" + sessionStorage["start"] + "&end=" + sessionStorage["end"] + "&pipeline=" + sessionStorage["pipeline"] + "&datacen=" + sessionStorage["datacen"] + "&network=" + sessionStorage["network"] + "&farm=" + sessionStorage["farm"];
}

/*
    When the user goes back to the home screen, the sessionStorage values 
    for datacenter, network, and farm are set to default values.   
*/
function setHomeDefaults() {
        sessionStorage["datacen"] = "All";
        sessionStorage["network"] = -1;
        sessionStorage["farm"] = -1;
}

/*
    If a sessionStorage variable is undefined (meaning that there are no stored values), then set the defaults.
    Then set/update the sessionStorage variable for the query string.
    Finally, set the window.location.search to the querystring.
*/
function updateQueryString() {
	sessionStorage["query"] = "start=" + sessionStorage["start"] + "&end=" + sessionStorage["end"] + "&pipeline=" + sessionStorage["pipeline"] + "&datacen=" + sessionStorage["datacen"] + "&network=" + sessionStorage["network"] + "&farm=" + sessionStorage["farm"];
	window.location.search = sessionStorage["query"];
}

/*
    sessionStorage variables rely on the querystring in the URL bar. 
    So, this makes sure that the sessionStorage variables are the same as the querystring.
*/
function setSessionStorage() {
    if (window.location.search != "") {
        sessionStorage["start"] = $.QueryString("start");
        sessionStorage["end"] = $.QueryString("end");
        sessionStorage["pipeline"] = $.QueryString("pipeline");
        sessionStorage["datacen"] = $.QueryString("datacen");
        sessionStorage["network"] = $.QueryString("network");
        sessionStorage["farm"] = $.QueryString("farm");
        sessionStorage["changed"] = true;
    }
}

/*
    This sets the different fields, which includes the drop downs on the graph pages and the date selectors.
*/
function setFields() {
	$(".from").val(sessionStorage["start"]);
	$(".to").val(sessionStorage["end"]);
	$("#FeaturedContent_Datacenter").val(sessionStorage["datacen"]);
	$("#FeaturedContent_Network").val(sessionStorage["network"]);
	$("#FeaturedContent_Farm").val(sessionStorage["farm"]);
}

/*
  Changes the background color and the text color of the pipeline selected. This is determined by the query string.
*/
function setSelectedPipeline() {
    $("#" + sessionStorage["pipeline"] + " a:not(.button)").css({
        "color": "black",
        "background-color": "#008cba"
    });
}

/*
    Takes in an parameter for the selected element's id, whose type is string.
    This function is meant for the list of pipelines on the left side of the page.
    Sets the sessionStorage variable for the pipeline to the id parameter, then updates the query string.
*/
function setPipeline(id) {
    $("#loading").fadeIn();
    sessionStorage["pipeline"] = id;
    sessionStorage["changed"] = true;
	updateQueryString();
}


/*
    Takes in an parameter for the selected element's id.
    This function is meant for the Data Center Circles on the World Heat Map.
    Sets the sessionStorage variable for the datacenter to the id parameter,
    then it navigates the user to the Data Center Heat Map.
*/
function setDatacenter(id) {
    sessionStorage["datacen"] = id;
    sessionStorage["changed"] = true;
    updateQueryString();
	window.location.href = "../Home/DCHM";
}

/*
    CHANGE DESCRIPTIONS UJEFJ@#QR@#    
*/
function setBreadcrumbs() {
    if (sessionStorage["datacen"] != "All") {
        $(".breadcrumbs").append("<li onclick='setHomeDefaults();'><a href = '../Home/'>Home</a></li>");
        $(".breadcrumbs").append("<li><a href = '../RePD/'>MSR Report</a></li>");
    } else  if (sessionStorage["datacen"] == "All") {
        $(".breadcrumbs").append("<li class='current'>Home</li>");
        $(".breadcrumbs").append("<li><a href = '../RePD/'>MSR Report</a></li>");
    }
}

/*
    For the DCHM. It sets the sessionStorage variable for the farm to the selected element's id.
    Then it grabs the id of its parent container (which is why each network's farms are within)
    and sets it to the sessionStorage variable for the network. This way the network and farm in
    the querystring is accurate. It also takes the user to the Percent Data page.
*/
function setFarm(id) {
	sessionStorage["farm"] = id;
	sessionStorage["network"] = $("#" + id).parent().attr('id');
	sessionStorage["changed"] = true;
	window.location.href = "../Home/PercentData";
}

/*
    For the DCHM. It sets the sessionStorage variable for the network to the selected element's id.
    It resets the farm, as a network was selected. It also takes the user to the Percent Data page.
*/
function setNetwork(id) {
	sessionStorage["network"] = id;
	sessionStorage["farm"] = -1;
	sessionStorage["changed"] = true;
	window.location.href = "PercentData";
}

/*
    This runs at the start of every page. First, it checks to see if there is a querystring.
    If not, then it runs updateQueryString(). If there is a querystring, then it updates the
    sessionStorage variables, the input fields, and the breadcrumbs.
*/
$(document).ready(function () {
    //$("#loading").fadeIn();
    $(document).foundation();
    if (sessionStorage["changed"] == false || sessionStorage["query"] == undefined) {
        setDefaults();
    } else if (sessionStorage["changed"] == "true" && window.location.search == "") {
        updateQueryString();
    }
        setSessionStorage();
		setFields();
		setBreadcrumbs();
	//$("#loading").fadeOut("slow");
});

//Move Elsewhere!
$(document).ready(function () {
    $("#loading").fadeIn();
	$(".from").datepicker({
		defaultDate: sessionStorage["start"],
		changeMonth: true,
		changeYear: true,
		dateFormat: 'yy-mm-dd',
		maxDate:"-1D",
		numberOfMonths: 1,
		onClose: function (selectedDate) {
			$(".to").datepicker("option", "minDate", selectedDate);
			$('.from').val(selectedDate);
		}
	});
	$(".to").datepicker({
		defaultDate: sessionStorage["end"],
		changeMonth: true,
		changeYear: true,
		dateFormat: 'yy-mm-dd',
		numberOfMonths: 1,
		onClose: function (selectedDate) {
			$(".from").datepicker("option", "maxDate", selectedDate);
			$('.to').val(selectedDate);
		}
	});

	//$("#loading").fadeOut("slow");
});