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
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegister Usermodel)
        {
            UserRegister postmodel = new UserRegister();
            postmodel.Username = Usermodel.Username;
            postmodel.Password = Usermodel.Password;
            context.UserRegisters.Add(postmodel);
            if (postmodel.Username != null && postmodel.Password != null)
            {
                Session["usern@me"] = postmodel.Username;
                Session["passw0rd"] = postmodel.Password;
                context.SaveChanges();
            }
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            if(Session["usern@me"] != null && Session["passw0rd"] != null)
            {
                return RedirectToAction("Client");
            }
            else
            {
                return View();
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserRegister Usermodel)
        {
            var data = context.UserRegisters.Where(s => s.Username.Equals(Usermodel.Username) && s.Password.Equals(Usermodel.Password)).ToList();
            if (data.Count > 0)
            {
                Session["usern@me"] = Usermodel.Username;
                Session["passw0rd"] = Usermodel.Password;
                return RedirectToAction("Client");
            }
            else
            {
                return RedirectToAction("Register");
            }
        }

            public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
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
        public ActionResult Customer(string fil = "a")
        {
            var query = from c in context.CARS
                        select c;
            var table = query.ToList();
            List<SelectListItem> filter = new List<SelectListItem>();
            foreach (var row in table)
            {
                if ((row.carName + " " + row.makeyear + " " + row.sparePart).ToUpper().Contains(fil.ToUpper()))
                {
                    filter.Add(new SelectListItem()
                    {
                        Text = (row.carName + " "
                    + row.makeyear + " " + row.sparePart)
                    });
                }
            }
            ViewBag.filter = filter;
            return View(table);
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
            carObject.clientName = (string)Session["usern@me"];
            carObject.mobileNo = carmodel.mobileNo;
            carObject.carName = carmodel.carName;
            carObject.makeyear = carmodel.makeyear;
            carObject.sparePart = carmodel.sparePart;
            context.CARS.Add(carObject);
            if (carObject.clientName != "null" && carObject.mobileNo != "null"
                && carObject.sparePart != "null")
            {
                context.SaveChanges();
            }
            else { return RedirectToAction("Error"); }
            return RedirectToAction("Customer");
        }
        
    }
}