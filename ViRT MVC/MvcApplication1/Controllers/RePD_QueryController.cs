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
    public class RePD_QueryController : Controller
    {
        MSRreliability rel = new MSRreliability();
        MSRperformance perf = new MSRperformance();

        public decimal getRange()
        {
            return 487.1846846846M;
        }

        public string getTeams()
        {
            return "teststetsa";
        }

        public decimal getReliability()
        {
            return rel.MonthReliabilityPercent(Convert.ToDateTime(Request.QueryString["month"]));
        }

        public decimal getPerformance()
        {
            return perf.PerformancePercent(Convert.ToDateTime(Request.QueryString["month"]));
        }

        public decimal getQOS()
        {
            return 0;
        }

        public int getLatency()
        {
            return perf.Percentile95th(Convert.ToDateTime(Request.QueryString["month"]));
        }
    }
}
