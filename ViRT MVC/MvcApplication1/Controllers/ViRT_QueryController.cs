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
        Reliability test = new Reliability(); 
        public string getPipelines()
        {
            return JsonConvert.SerializeObject(test.GetAllPipelines());
        }

        public string getOverview()
        {               
            test.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
            return JsonConvert.SerializeObject(test.CalculateOverviewBar(Request.QueryString["pipeline"]));
        }

        public string getDatacenters()
        {
            return JsonConvert.SerializeObject(test.GetDataCenterLatLong());
        }

        public string getNetworks()
        {
            return JsonConvert.SerializeObject(test.GetAllNetworks());
        }

        public string getFarms()
        {
            return JsonConvert.SerializeObject(test.GetAllFarms());
        }

        public string getNetworkFarm()
        {
            Reliability test = new Reliability();
            
            test.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));

			test.ChangeDataCenter(Request.QueryString["datacen"]);

			DataTable table = test.CalculateDataCenterHeatMap();

            return JsonConvert.SerializeObject(table, Formatting.Indented);
        }

        public string getTeams()
        {
            return "";
        }

        public string getReliability()
        {
            return "";
        }

        public string getPerformance()
        {
            return "";
        }
    }
}
