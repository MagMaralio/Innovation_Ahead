using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Innovation_Ahead.Models;


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

        public ActionResult Client()
        {
            CAR carObject = new CAR();
            return View(carObject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Client(car carmodel)
        {
            CAR carObject = new CAR();
            carObject.clientName = carmodel.clientName;
            carObject.mobileNo = carmodel.mobileNo;
            carObject.carName = carmodel.carName;
            carObject.makeyear = carmodel.makeyear;
            carObject.sparePart = carmodel.sparePart;
            context.CARS.Add(carObject);
            context.SaveChanges();
            return RedirectToAction("Customer");
        }
    }
}