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
    public class WorldMapController : Controller
    {
        //
        // GET: /WorldMap/

        public ActionResult Index()
        {
			Reliability world = new Reliability();

			DataTable worldLocs = world.GetDataCenterLatLong();

			var json = JsonConvert.SerializeObject(worldLocs);

			ViewBag.WorldMap = json;

            return View();
        }

    }
}
