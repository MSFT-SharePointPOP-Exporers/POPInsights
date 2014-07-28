using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using Newtonsoft.Json;
using System.Data;

namespace MvcApplication1.Controllers
{
    public class RePDController : Controller
    {

        public ActionResult Index()
        {
            MSRreliability reliability = new MSRreliability();
            DateTime startdate = Convert.ToDateTime(Request.QueryString["month"]);
            DateTime newdate = new DateTime(0001, 1, 1);
            if (startdate != newdate)
            {
                DataTable MSRreliabilityChart = reliability.ReliaiblityDailyTable(Convert.ToDateTime(Request.QueryString["month"]));//Convert.ToDateTime(Request.QueryString["start"]));
                var json = JsonConvert.SerializeObject(MSRreliabilityChart, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                ViewBag.MSRreliabilityChart = json;

                MSRperformance performance = new MSRperformance();
                DataTable MSRPerfChart = performance.PerformancePercentDaily(Convert.ToDateTime(Request.QueryString["month"]));
                var performanceJson = JsonConvert.SerializeObject(MSRPerfChart, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                ViewBag.MSRPerfChart = performanceJson;

                DataTable MSRPerfPercentileChart = performance.Percentile95Table(Convert.ToDateTime(Request.QueryString["month"]));
                var perfJson = JsonConvert.SerializeObject(MSRPerfPercentileChart, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                ViewBag.MSRPerfPercentileChart = perfJson;
            }

            return View();
        }

    }
}
