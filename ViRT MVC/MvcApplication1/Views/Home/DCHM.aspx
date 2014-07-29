<%@ Page Language="C#" MasterPageFile="~/Views/Shared/ViRT.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link href="../../Content/DCHM.css" rel="stylesheet" />
    <script>
        $(document).ready(function () {
            $("button").click(function () {
                $("#loading").fadeIn();
            });

            if (window.location.search == "") {
                updateQueryString();
            } else  {
                $("#rendering h1").text(sessionStorage["datacen"]);
                $("#loading").fadeIn();
                $.ajax({
                    data: sessionStorage["query"],
                    url: '<%= Url.Action("getNetworkFarm", "ViRT_Query") %>',
                    dataType: "json",
                    success: function (data) {
                        console.log(data);
                       $(".dchm").append('<ul class="small-block-grid-2 medium-block-grid-3 large-block-grid-4">');
                        if (data != null) {
                            for (var x = 0; x < data.length; x++) {
                                $(".dchm ul").append("<li id ='" + x + "'>");
                                $("#" + x).append("<h2>Network " + data[x].NetworkID + "<br/><a href='#' onClick='setNetwork(" + data[x].NetworkID + ")'>" + data[x].Percentage.toFixed(2) + "%</a></h2>");
                                $("#" + x).append("<div class ='network-box' id =" + data[x].NetworkID + ">");
                                $("select").append("<option value=" + data[x].NetworkID + ">" + data[x].NetworkID + "</option>");
                                for (var y = 0; y < data[x].Farms.length; y++) {
                                    if (data[x].Farms[y].Percentage <= 100.00 && data[x].Farms[y].Percentage >= 99.9) {
                                        $("#" + data[x].NetworkID).append("<div class ='farm-box green' onClick='setFarm(this.id)' id=" + data[x].Farms[y].Farms + ">Farm " + data[x].Farms[y].Farms.toString() + "<br/>" + data[x].Farms[y].Percentage.toFixed(2) + "%</div>");
                                    } else if (data[x].Farms[y].Percentage < 99.9 & data[x].Farms[y].Percentage >= 99.0) {
                                        $("#" + data[x].NetworkID).append("<div class ='farm-box yellow' onClick='setFarm(this.id)' id=" + data[x].Farms[y].Farms + ">Farm " + data[x].Farms[y].Farms.toString() + "<br/>" + data[x].Farms[y].Percentage.toFixed(2) + "%</div>");
                                    } else if (data[x].Farms[y].Percentage < 99.0 && data[x].Farms[y].Percentage >= 95.0) {
                                        $("#" + data[x].NetworkID).append("<div class ='farm-box red1' onClick='setFarm(this.id)' id=" + data[x].Farms[y].Farms + ">Farm " + data[x].Farms[y].Farms.toString() + "<br/>" + data[x].Farms[y].Percentage.toFixed(2) + "%</div>");
                                    } else if (data[x].Farms[y].Percentage < 95.0 && data[x].Farms[y].Percentage >= 85.0) {
                                        $("#" + data[x].NetworkID).append("<div class ='farm-box red2' onClick='setFarm(this.id)' id=" + data[x].Farms[y].Farms + ">Farm " + data[x].Farms[y].Farms.toString() + "<br/>" + data[x].Farms[y].Percentage.toFixed(2) + "%</div>");
                                    } else {
                                        $("#" + data[x].NetworkID).append("<div class ='farm-box red3' onClick='setFarm(this.id)' id=" + data[x].Farms[y].Farms + ">Farm " + data[x].Farms[y].Farms.toString() + "<br/>" + data[x].Farms[y].Percentage.toFixed(2) + "%</div>");
                                    }
                                }
                            }
                        }
                    },
                    complete: function (data) {
                        $("#loading").fadeOut("slow");
                    }
                });
            }
            });
    </script>
</asp:Content>

<asp:Content ID="contactTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Data Center Heat Map
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="FeaturedContent" runat="server">
    <h1></h1> <!-- Data Center will go here -->
    <h2>Select a Network of Farm to filter reliability data.</h2>
    <div class="" id="jumptonetwork">
        <label for="networks">Jump To Network</label>
        <select id="networks" onchange="window.location.hash = this.value">
        </select>
    </div>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
   <div class = "dchm">
    </div>
        <div id ="legendBar" class="small-12 medium-12 large-12 columns">
        <ul class="small-block-grid-5 medium-block-grid-5 large-block-grid-5">
            <li><div class="green legend">100.00-99.90</div></li>
            <li><div class="yellow legend">99.90-99.00</div></li>
            <li><div class="red1 legend">99.00-95.00</div></li>
            <li><div class="red2 legend">95.00-85.00</div></li>
            <li><div class="red3 legend">85.00-0.00</div></li>
        </ul>
    </div>
</asp:Content>