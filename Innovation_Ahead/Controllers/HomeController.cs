using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Innovation_Ahead.Controllers
{
    public class HomeController : Controller
    {
        VehiclesEntities context = new VehiclesEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Customer()
        {
            var query = from c in context.CARS
                        select c;
            return View(query.ToList());
        }
    }
}