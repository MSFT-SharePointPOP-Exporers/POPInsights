﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using System.Data;
using MvcApplication1.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;



namespace MvcApplication1.Controllers
{
    [SessionState(SessionStateBehavior.Default)]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
			Reliability world = new Reliability();

			DataTable worldLocs = world.GetDataCenterLatLong();
			if (Request.QueryString["pipeline"] != null)
			{
				world.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
				world.ChangeDataCenter(Request.QueryString["pipeline"]);
			}

			var json = JsonConvert.SerializeObject(worldLocs);

			DataTable dcPipeAverage = world.CalculateWorldMapCircle();

			var percentages = JsonConvert.SerializeObject(dcPipeAverage);

			ViewBag.AverageDCPercent = percentages;
			ViewBag.WorldMap = json;
            return View();
        }

        public ActionResult DCHM()
        {

            return View();
        }

        public ActionResult PercentData()
        {
            Reliability paramsPercent = new Reliability(Request.QueryString["datacen"], Convert.ToInt32(Request.QueryString["network"]), Convert.ToInt32(Request.QueryString["farm"]), Request.QueryString["pipeline"], Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));

            DataTable percentTable = paramsPercent.PipelineGraphTable(Request.QueryString["pipeline"]);

            var json = JsonConvert.SerializeObject(percentTable, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            ViewBag.PercentData = json;

            return View();
        }

        public ActionResult RawData()
        {
            Reliability rawData = new Reliability(Request.QueryString["datacen"], Convert.ToInt32(Request.QueryString["network"]), Convert.ToInt32(Request.QueryString["farm"]), Request.QueryString["pipeline"], Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));

            rawData.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
            String[] components = rawData.GetComponents(Request.QueryString["pipeline"]);
            List<DataTable> allComponentsRawData = new List<DataTable>();

            foreach (var compName in components)
            {
                DataTable rawDataTable = rawData.RawDataGraphTable(compName);
                allComponentsRawData.Add(rawDataTable);
            }
            var table = JsonConvert.SerializeObject(allComponentsRawData, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            //ViewData["RawData"] = data;
            ViewBag.RawData = table;
            ViewBag.RawTitles = JsonConvert.SerializeObject(components);

            return View();
        }
    }
}
