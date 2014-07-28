using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MvcApplication1.Models;
using Newtonsoft.Json;

namespace MvcApplication1.Controllers
{
    public class RawDataController : Controller
    {
        //
        // GET: /RawData/

        public ActionResult RawData()
        {
            Reliability rawData = new Reliability();

            rawData.ChangeDate(Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
            String[] components = rawData.getComponents(Request.QueryString["pipeline"]);
            List<DataTable> allComponentsRawData = new List<DataTable>();

            foreach (var compName in components)
            {
                DataTable rawDataTable = rawData.RawDataTable(compName);
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
