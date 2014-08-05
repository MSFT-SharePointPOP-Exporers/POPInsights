using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;
using MvcApplication1.Models;


namespace MvcApplication1.Controllers
{
    public class ViRT_QueryController : Controller
    {

        //initialize a new Reliability model
        Reliability rel = new Reliability();
 
        /// <summary>
        /// Runs the the GetAllPipelines function to grab the list of pipelines, then stores in it a data table.
        /// Finally serializes it into a JSON object.
        /// </summary>
        /// <returns>JSON Object with all pipeline names</returns>
        public string getPipelines()
        {
            return JsonConvert.SerializeObject(rel.GetAllPipelines());
        }

        /// <summary>
        /// This function is for the percentage at the top of the Overview bar. First, it updates the date range, datacenter, network,
        /// and farm of the Reliability model object then calculates the Overview Bar percentage for all pipelines. 
        /// </summary>
        /// <returns>JSON Object with all pipeline names and their percentage reliability</returns>
        public string getOverviewHeader()
        {
            rel.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
            rel.ChangeDataCenter(Request.QueryString["datacen"]);
            rel.ChangeNetworkID(Convert.ToInt32(Request.QueryString["network"]));
            rel.ChangeFarmID(Convert.ToInt32(Request.QueryString["farm"]));
            return JsonConvert.SerializeObject(rel.CalculateOverviewBar("Overview"));
        }

        /// <summary>
        /// This function is for the percentages for the components for the specific pipeline. First, updates the date range, 
        /// datacenter, network, and farm of the Reliability model object then calculates the Overview Bar percentage for all 
        /// pipelines.
        /// </summary>
        /// <returns>JSON Object with the components of the pipeline and their percentages</returns>
        public string getOverview()
        {               
            rel.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
            rel.ChangeDataCenter(Request.QueryString["datacen"]);
            rel.ChangeNetworkID(Convert.ToInt32(Request.QueryString["network"]));
            rel.ChangeFarmID(Convert.ToInt32(Request.QueryString["farm"]));
            return JsonConvert.SerializeObject(rel.CalculateOverviewBar(Request.QueryString["pipeline"]));
        }

        /// <summary>
        /// Gets list of datacenters for the dropdown for datacenters on the Component Reliability and Raw Data Graph pages
        /// </summary>
        /// <returns>JSON Object with all available datacenters</returns>
        public string getDatacenters()
        {
            return JsonConvert.SerializeObject(rel.GetDataCenterLatLong());
        }

        /// <summary>
        /// Gets list of networks for the dropdown for networks on the Component Reliability and Raw Data Graph pages
        /// </summary>
        /// <returns>JSON Object with all available networks</returns>
        public string getNetworks()
        {
            return JsonConvert.SerializeObject(rel.GetAllNetworks());
        }

        /// <summary>
        /// Gets list of farms for the dropdown for farms on the Component Reliability and Raw Data Graph pages
        /// </summary>
        /// <returns>JSON Object with all available farms</returns>
        public string getFarms()
        {
            return JsonConvert.SerializeObject(rel.GetAllFarms());
        }

        /// <summary>
        /// Updates the date range, datacenter, and pipeline from the querystring, then serializes the information for the
        /// DataCenter Heat Map.
        /// </summary>
        /// <returns>JSON Object that stores the each network, their percentages, their farms, and the farm percentages</returns>
        public string getNetworkFarm()
        {                 
            rel.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));

			rel.ChangeDataCenter(Request.QueryString["datacen"]);

            rel.ChangePipeline(Request.QueryString["pipeline"]);

            return JsonConvert.SerializeObject(rel.CalculateDataCenterHeatMap(), Formatting.Indented);
        }
    }
}
