<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ViRT.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="../../Content/Raw%20Data.css" rel="stylesheet" />
             <style>
        .amChartsLegend g text {
            text-decoration: underline;
        }

        #chartdiv .newchart {
            width: 100%;
            height: 500px;
        }

        rect {
            text-decoration: none;
        }

        #PercentDataLink {
            padding: 0;
        }
    </style>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/amcharts.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/serial.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/themes/dark.js"></script>
    <script>
    	$(document).ready(function() {
    		$(document).ajaxSend(function () {
    			$("#loading").fadeIn();
    		});

    		$(document).ajaxComplete(function () {
    			$("#loading").fadeOut("slow");
    		});

    		$.ajax({
    			data: sessionStorage["query"],
    			url: '<%= Url.Action("getDatacenters", "ViRT_Query") %>',
                dataType: "json",
                success: function (data) {
                	$("#FeaturedContent_Datacenter").append("<option value='All'>All</option>");
                	for (var x = 0; x < data.length; x++) {
                		$("#FeaturedContent_Datacenter").append("<option value=" + data[x].DataCenter + ">" + data[x].DataCenter + "</option>");
                	}
                	setFields();
                }
            });

        	$.ajax({
        		data: sessionStorage["query"],
        		url: '<%= Url.Action("getNetworks", "ViRT_Query") %>',
                dataType: "json",
                success: function (data) {
                	$("#FeaturedContent_Network").append("<option value='-1'>All</option>");
                	for (var x = 0; x < data.length; x++) {
                		$("#FeaturedContent_Network").append("<option value=" + data[x].NetworkID + ">" + data[x].NetworkID + "</option>");
                	}
                	setFields();
                }
            });

        	$.ajax({
        		data: sessionStorage["query"],
        		url: '<%= Url.Action("getFarms", "ViRT_Query") %>',
                dataType: "json",
                success: function (data) {
                	$("#FeaturedContent_Farm").append("<option value='-1'>All</option>");
                	for (var x = 0; x < data.length; x++) {
                		$("#FeaturedContent_Farm").append("<option value=" + data[x].FarmID + ">" + data[x].FarmID + "</option>");
                	}
                	setFields();
                }
        	});

        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="TitleContent" runat="server">
    RawData
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="FeaturedContent" runat="server">
    <div class="graph_header">
        <h1>RawData</h1>
        <a href="PercentData" id="PercentDataLink">View Component Reliability</a>
    </div>

    <div id="selectors" class="small-12 medium-12 large-12">
        <form id="form1" runat="server">
            <div id="SelectDatacenter">
                <p>Datacenter</p>

                <asp:DropDownList ID="Datacenter" runat="server">
                </asp:DropDownList>
            </div>
            <div id="SelectNetwork">
                <p>Network ID</p>

                <asp:DropDownList ID="Network" runat="server">
                </asp:DropDownList>
            </div>
            <div id="SelectFarm">
                <p>Farm ID</p>

                <asp:DropDownList ID="Farm" runat="server">
                </asp:DropDownList>
            </div>
            <div id="Entry">
            </div>
        </form>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <div id="chartdiv" style="width: 100%; height: 70%;"></div>
    <script>
    	$(document).ready(function() {
    		$(document).ajaxSend(function () {
    			$("#loading").fadeIn();
    		});

    		$(document).ajaxComplete(function () {
    			$("#loading").fadeOut("slow");
    		});
    	});
    	var bullets = ["round", "square", "triangleUp", "triangleDown", "triangleLeft", "triangleRight", "diamond", "xError", "yError"];
    	var data = <%= Html.Raw(ViewBag.RawData)%>;//generateChartData();
        var titles = <%= Html.Raw(ViewBag.RawTitles)%>;
    	createCharts(data);

    	function createCharts(datasets) {
    		for (var i = 0; i < datasets.length; i++) {
    			var div = document.createElement("div");
    			div.className = "newchart";
    			document.getElementById("chartdiv").appendChild(div);
    			var object = datasets[i];
    			var title = titles[i];
    			var chart = create(object, div, title);
    		}
    	}

    	function create(chartData, newdiv, graphTitle) {
    		var chart = AmCharts.makeChart(newdiv, {
    			"titles": [{
    				"text": graphTitle,
    				"size": 30
    			}],
    			"type": "serial",
    			"theme": "dark",
    			"pathToImages": "http://www.amcharts.com/lib/3/images/",
    			"dataProvider": chartData,
    			"valueAxes": [{
    				"id": "v1",
    				"position": "left",
    				"axisColor": "#008CBA",
    				"axisThickness": 2,
    				"gridAlpha": 0,
    				"axisAlpha": 1

    			}, {
    				"id": "v2",
    				"position": "right",
    				"axisColor": "#43AC6A",
    				"axisThickness": 2,
    				"gridAlpha": 0,
    				"axisAlpha": 1
    			}],
    			"legend": {
    				"title": "Select Component:",
    				"font": 40,
    				"labelText": "[[title]]",
    				"titlePosition": "top",
    				"useGraphSettings": true,
    				"color": "#e1e1e1",
    				"valueText": ""
    			},
    			"chartScrollbar": {
    				"autoGridCount": true,
    				"scrollbarHeight": 40
    			},
    			"chartCursor": {
    				"cursorPosition": "mouse",
    				"categoryBalloonDateFormat": "MMM DD, YYYY \nJJ:NN"
    			},
    			"dataDateFormat": "YYYY-MM-DDTJJ:NN:SS",
    			"categoryField": "Date",
    			"categoryAxis": {
    				"parseDates": true,
    				"minPeriod": "hh"
    			}
    		});

    		var i = 0;
    		for (var j = 0; j < chartData.length; j++) {

    			if (Object.keys(chartData[j]).length > 1)
    			{
    				for (var propertyName in chartData[j]) {
    					if (propertyName != 'Date') {
    						if (i == 9)
    							i = 0;
    						var graph1 = new AmCharts.AmGraph();
    						graph1.type = "line";
    						graph1.valueField = propertyName;
    						graph1.balloonText = "<b><span style='font-size:14px;'>[[title]]</span></b><br />[[category]]<br /><span style='font-size:14px;'>Count: [[value]]</span>";
    						graph1.title = propertyName;
    						graph1.bullet = bullets[i];
    						graph1.bulletSize = 10;
    						graph1.connect = false;
    						graph1.hideBulletsCount = 30;

    						if( i ==0)
    						{
    							graph1.valueAxis = "v1";
    							graph1.lineColor = "#008CBA";
    						}
                        
    						if( i ==1)
    						{
    							graph1.valueAxis = "v2";
    							graph1.lineColor = "#43AC6A";
    						}
                        

    						chart.addGraph(graph1);
    						i++;
    					}
    				}
    				break;
    			}
    		}
    		return chart;
    	}

    </script>
</asp:Content>


