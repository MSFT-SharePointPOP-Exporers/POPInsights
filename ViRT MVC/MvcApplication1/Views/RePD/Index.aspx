<%@ Page Language="C#" MasterPageFile="~/Views/Shared/RePD.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    MSR Report
</asp:Content>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <style>
    #chartdiv {
            width: 100%;
            height: 700px;
            display:none;
            text-align:center;
            margin-bottom:15%;
        }
        #PerformanceDiv {
            width: 100%;
            height: 700px;
            display:none;
            text-align:center;
            margin-bottom:15%;
        }
        #PercentileDiv {
            width: 100%;
            display:none;
            text-align:center;
            height: 700px;
            margin-bottom:15%;
        }
        </style>
    <script>
        $(document).ready(function () {
            $("#rendering h1").append($.QueryString("team"));

                $.ajax({
                    data: "month=" + sessionStorage["month"],
                    url: '<%= Url.Action("getReliability", "RePD_Query") %>',
                    success: function (data) {
                        reliability_val = parseFloat(data);
                        if (reliability_val != 0) {
                            $(".reliability").append(reliability_val.toFixed(2) + "%");
                            if (reliability_val > upper) {
                                $(".reliability").addClass("green");
                            } else if (reliability_val > lower && reliability_val < upper) {
                                $(".reliability").addClass("yellow");
                            } else if (reliability_val < lower) {
                                $(".reliability").addClass("red");
                            }
                        } else {
                            $(".reliability").append("N/A");
                        }
                    }
                });

            $.ajax({
                data: "month=" + sessionStorage["month"],
                    url: '<%= Url.Action("getPerformance", "RePD_Query") %>',
                    success: function (data) {
                        performance_val = parseFloat(data);
                        if (performance_val != 0) {
                            $(".performance").append(performance_val.toFixed(2) + "%");
                            if (performance_val > upper) {
                                $(".performance").addClass("green");
                            } else if (performance_val > lower && performance_val < upper) {
                                $(".performance").addClass("yellow");
                            } else if (performance_val < lower) {
                                $(".performance").addClass("red");
                            }
                        } else {
                            $(".performance").append("N/A");
                        }
                    }
                });

            //Still needs to be done!
                $.ajax({
                    data: "month=" + sessionStorage["month"],
                    url: '<%= Url.Action("getQoS", "RePD_Query") %>',
                    success: function (data) {
                        $(".qos").append(QoS_val.toFixed(2) + "%");
                        if (QoS_val > upper) {
                            $(".qos").addClass("green");
                        } else if (QoS_val > lower && QoS_val < upper) {
                            $(".qos").addClass("yellow");
                        } else if (QoS_val < lower) {
                            $(".qos").addClass("red");
                        }
                    }
            });

            //It just so happens that it works. But fix it.
            $.ajax({
              data: "month=" + sessionStorage["month"],
              url: '<%= Url.Action("getLatency", "RePD_Query") %>',
              success: function (data) {
                  latency = parseInt(data);
                  if (latency != 0) {
                      $(".latency").append(latency + " ms");
                      if (latency > upper) {
                          $(".latency").addClass("green");
                      } else if (latency > lower && latency < upper) {
                          $(".latency").addClass("yellow");
                      } else if (latency < lower) {
                          $(".latency").addClass("red");
                      }
                  } else {
                      $(".latency").append("N/A");
                  }
               }
            });
        });
    </script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/amcharts.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/serial.js"></script>
    <script type="text/javascript" src="http://www.amcharts.com/lib/3/themes/dark.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id = "Overall">
    <div id="overallstats">
        <h2>Overall Statistics</h2>
    <table>
  <tr>
    <td rowspan="2">QoS</td>
    <td>Reliability</td>
    <td class="text reliability"></td><!--Reliability-->
    <td class="text qos" rowspan="2"></td><!--QoS-->
  </tr>
  <tr>
    <td class="secondary">Performance</td>
    <td class="text performance"></td><!--Performance-->
  </tr>
  <tr>
    <td class="text" colspan="3">
     95th percentile server latency*<br />
     *Goal is < 200 ms
    </td>
    <td class="text latency"></td>
  </tr>
</table>
        </div>

        <div id="legend" class="dates large-2 medium-2 columns show-for-medium-up">
        <table>
         <tr>
           <th>Legend</th>
         </tr>
         <tr>
           <td class="green"></td>
         </tr>
          <tr>
           <td class="yellow"></td>
         </tr>
          <tr>
           <td class="red"></td>
         </tr>
        </table>
      </div>

    <div id="chartdiv"></div>
    <div id="PerformanceDiv"></div>
    <div id="PercentileDiv"></div>

        </div>
    <script>
        var bullets = ["round", "square", "triangleUp", "triangleDown", "triangleLeft", "triangleRight", "diamond", "xError", "yError"];
        var reliability;
        var performance;                                                                                              
        var percentile;

        reliability = <%= Html.Raw(ViewBag.MSRreliabilityChart)%>;//generateChartData();
        performance = <%= Html.Raw(ViewBag.MSRPerfChart)%>;//generateChartData();
        percentile = <%= Html.Raw(ViewBag.MSRPerfPercentileChart)%>;

        if (reliability.length > 0){$("#chartdiv").show();createReliabilityChart(reliability);}
        if (performance.length > 0){$("#PerformanceDiv").show();createPerformanceChart(performance);}
        if (percentile.length > 0){$("#PercentileDiv").show();createPercentileChart(percentile);} 

        function createReliabilityChart(chartData) {
            var chart = AmCharts.makeChart("chartdiv", {
                "titles": [{
                    "text": "Overall Reliability",
                    "size": 30,
                    "bold": false
                }],
                "type": "serial",
                "theme": "dark",
                "pathToImages": "http://www.amcharts.com/lib/3/images/",
                "dataProvider": chartData,
                "graphLineColor": "#008cba",
                "valueAxes": [{
                    "dashLength": 1,
                    "position": "left"
                }],
                "chartScrollbar": {
                    "autoGridCount": true,
                    "scrollbarHeight": 40
                },
                "chartCursor": {
                    "cursorPosition": "mouse",
                    "categoryBalloonDateFormat": "MMM DD, YYYY"
                },
                "dataDateFormat": "YYYY-MM-DD",
                "categoryField": "Date",
                "categoryAxis": {
                    "parseDates": true
                }
            });

            var i = 0;
            for (var propertyName in chartData[0]) {
                if (propertyName != 'Date') {
                    if (i == 9)
                        i = 0;
                    var graph1 = new AmCharts.AmGraph();
                    graph1.type = "line";
                    graph1.valueField = propertyName;
                    graph1.lineColor = "#008cba";
                    graph1.balloonText = "<b><span style='font-size:14px;'>[[title]]</span></b><br />[[category]]<br /><span style='font-size:14px;'>Milliseconds: [[value]]</span>";
                    graph1.title = "Reliability";
                    graph1.bullet = bullets[i];
                    graph1.bulletSize = 10;
                    graph1.hideBulletsCount = 30;
                    graph1.connect = false;
                    chart.addGraph(graph1);
                    i++;
                }
            }


            return chart;
        }

        function createPerformanceChart(chartData) {
            var chart = AmCharts.makeChart("PerformanceDiv", {
                "titles": [{
                    "text": "Overall Performance",
                    "size": 30,
                    "bold": false
                }],
                "type": "serial",
                "theme": "dark",
                "pathToImages": "http://www.amcharts.com/lib/3/images/",
                "dataProvider": chartData,
                "graphLineColor": "#008cba",
                "valueAxes": [{
                    "dashLength": 1,
                    "position": "left"
                }],
                "chartScrollbar": {
                    "autoGridCount": true,
                    "scrollbarHeight": 40
                },
                "chartCursor": {
                    "cursorPosition": "mouse",
                    "categoryBalloonDateFormat": "MMM DD, YYYY"
                },
                "dataDateFormat": "YYYY-MM-DD",
                "categoryField": "Date",
                "categoryAxis": {
                    "parseDates": true
                }
            });

            var i = 0;
            for (var propertyName in chartData[0]) {
                if (propertyName != 'Date') {
                    if (i == 9)
                        i = 0;
                    var graph1 = new AmCharts.AmGraph();
                    graph1.type = "line";
                    graph1.valueField = propertyName;
                    graph1.lineColor = "#008cba";
                    graph1.balloonText = "<b><span style='font-size:14px;'>[[title]]</span></b><br />[[category]]<br /><span style='font-size:14px;'>Reliability: [[value]]</span>";
                    graph1.title = "Performance";
                    graph1.bullet = bullets[i];
                    graph1.bulletSize = 10;
                    graph1.hideBulletsCount = 30;
                    graph1.connect = false;
                    chart.addGraph(graph1);
                    i++;
                }
            }


            return chart;
        }

        function createPercentileChart(chartData) {
            var chart = AmCharts.makeChart("PercentileDiv", {
                "titles": [{
                    "text": "Overall 95th Percentile",
                    "size": 30,
                    "bold": false
                }],
                "type": "serial",
                "theme": "dark",
                "pathToImages": "http://www.amcharts.com/lib/3/images/",
                "dataProvider": chartData,
                "valueAxes": [{
                    "dashLength": 1,
                    "position": "left"
                }],
                "chartScrollbar": {
                    "autoGridCount": true,
                    "scrollbarHeight": 40
                },
                "chartCursor": {
                    "cursorPosition": "mouse",
                    "categoryBalloonDateFormat": "MMM DD, YYYY"
                },
                "dataDateFormat": "YYYY-MM-DD",
                "categoryField": "Date",
                "categoryAxis": {
                    "parseDates": true
                }
            });

            var i = 0;
            for (var propertyName in chartData[0]) {
                if (propertyName != 'Date') {
                    if (i == 9)
                        i = 0;
                    var graph1 = new AmCharts.AmGraph();
                    graph1.type = "line";
                    graph1.lineColor = "#008cba";
                    graph1.valueField = propertyName;
                    graph1.balloonText = "<b><span style='font-size:14px;'>[[title]]</span></b><br />[[category]]<br /><span style='font-size:14px;'>Reliability: [[value]]</span>";
                    graph1.title = "95th Percentile";
                    graph1.bullet = bullets[i];
                    graph1.bulletSize = 10;
                    graph1.hideBulletsCount = 30;
                    graph1.connect = false;
                    chart.addGraph(graph1);
                    i++;
                }
            }


            return chart;
        }
    </script>
</asp:Content>
