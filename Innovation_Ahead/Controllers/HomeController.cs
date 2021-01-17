using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Innovation_Ahead.Models;
using PagedList;

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
            try
            {
                if (ModelState.IsValid)
                {
                    UserRegister postmodel = new UserRegister
                    {
                        firm = Usermodel.firm,
                        email = Usermodel.email,
                        password = Encryption(Usermodel.password.Trim()),
                        mobileNo = Usermodel.mobileNo,
                        authToken = null,
                        softdelete = null
                    };

                    context.UserRegisters.Add(postmodel);

                    if (postmodel.email != null && postmodel.password != null)
                    {
                        Session["em@il"] = postmodel.email;
                        Session["passw0rd"] = postmodel.password;
                        context.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    return RedirectToAction("Error");
                }
                return RedirectToAction("Error");
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        private string Encryption(string secret)
        {
            string key = "MAKV2SPBNI99212";
            byte[] converted_secret = Encoding.Unicode.GetBytes(secret);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes encoded = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = encoded.GetBytes(32);
                encryptor.IV = encoded.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(converted_secret, 0, converted_secret.Length);
                        cs.Close();
                    }
                    secret = Convert.ToBase64String(ms.ToArray());
                }
            }
            return secret;
        }

        public ActionResult Login()
        {
            if (Session["em@il"] != null && Session["passw0rd"] != null)
            {
                return RedirectToAction("ClientManagement");
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
            Usermodel.password = Encryption(Usermodel.password.Trim());
            var data = context.UserRegisters.Where(s => s.email.Equals(Usermodel.email) && s.password.Equals(Usermodel.password)).ToList();
            if (data.Count > 0 && data.First().softdelete == null)
            {
                Session["em@il"] = Usermodel.email;
                Session["passw0rd"] = Usermodel.password;
                return RedirectToAction("clientManagement");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Logout()
        {
            if (Session["em@il"] != null && Session["passw0rd"] != null)
            {
                Session["em@il"] = null;
                Session["passw0rd"] = null;
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Index(string selection = "Customer Login")
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
        public ActionResult Customer(string fil = "", int page = 1, int pageSize = 10)
        {
            var query = from c in context.UserRegisters
                        join p in context.CarParts on c.email equals p.link1
                        where c.mobileNo == p.link2 && p.quantity != 0 && c.softdelete == null
                        select new { c, p };
            var table = query.ToList();
            List<CarPart> filter = new List<CarPart>();
            foreach (var row in table)
            {
                //if (row.c.authToken == 1)
                //{
                //    TempData["flag"] = "Authenticated";
                //}
                //else { TempData["flag"] = null; }
                if (!string.IsNullOrEmpty(fil))
                {
                    if ((row.p.link1 + " " + row.p.link2 + " " + row.p.carName + " " + row.p.makeyear + " " + row.p.sparePart + " " + row.p.description).ToUpper().Contains(fil.ToUpper()))
                    {
                        filter.Add(new CarPart()
                        {
                            link1 = row.c.firm,
                            link2 = row.p.link2,
                            carName = row.p.carName,
                            makeyear = row.p.makeyear,
                            sparePart = row.p.sparePart,
                            quantity = row.p.quantity,
                            description = row.p.description
                        });
                    }
                }
                else
                {
                    filter.Add(new CarPart()
                    {
                        link1 = row.c.firm,
                        link2 = row.p.link2,
                        carName = row.p.carName,
                        makeyear = row.p.makeyear,
                        sparePart = row.p.sparePart,
                        quantity = row.p.quantity,
                        description = row.p.description
                    });
                }
            }
            PagedList<CarPart> model = new PagedList<CarPart>(filter, page, pageSize);
            return View(model);
        }

        public ActionResult Client()
        {
            CarPart carObject = new CarPart();

            return View(carObject);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Client(CarPart carmodel)
        {
            UserRegister datamodel = new UserRegister();
            datamodel.email = (string)Session["em@il"];
            datamodel.password = (string)Session["passw0rd"];
            var urdata = context.UserRegisters.Where(s => s.email.Equals(datamodel.email) && s.password.Equals(datamodel.password)).ToList();
            var cpdata = context.CarParts.Where(s => s.carName.Equals(carmodel.carName) &&
            s.makeyear.Equals(carmodel.makeyear) && s.sparePart.Equals(carmodel.sparePart) && s.link1.Equals(datamodel.email) && 
            s.link2.Equals(datamodel.password));

            if (cpdata.Count() == 1) 
            {
                TempData["alert"] = "You already have a same item added. Please edit the quantity";
                return View("~/Views/Home/alert.cshtml", cpdata.First());
            }

            CarPart carpartsObject = new CarPart();
            carpartsObject.carName = carmodel.carName;
            carpartsObject.sparePart = carmodel.sparePart;
            carpartsObject.makeyear = carmodel.makeyear;
            carpartsObject.quantity = carmodel.quantity;
            carpartsObject.description = carmodel.description;
            carpartsObject.link1 = (string)Session["em@il"];
            if (urdata.Count == 1)
            {
                carpartsObject.link2 = urdata[0].mobileNo;
            }
            context.CarParts.Add(carpartsObject);

            if (carpartsObject.link1 != "null" && carpartsObject.link2 != "null"
                && carpartsObject.sparePart != "null")
            {
                context.SaveChanges();
            }
            else { return RedirectToAction("Error"); }
            return RedirectToAction("ClientManagement");
        }

        public ActionResult ClientManagement(string fil = "", int page = 1, int pageSize = 10)
        {
            UserRegister datamodel = new UserRegister();
            datamodel.email = (string)Session["em@il"];
            datamodel.password = (string)Session["passw0rd"];
            var query = from c in context.UserRegisters
                        join p in context.CarParts on c.email equals p.link1
                        where c.email == datamodel.email && c.password == datamodel.password && c.mobileNo == p.link2 && c.softdelete == null
                        select new { c, p };
            var table = query.ToList();
            List<CarPart> filter = new List<CarPart>();
            if (table.Count != 0 && table[0].c.authToken == 1)
            {
                TempData["flag"] = "Authenticated";
            }
            else { TempData["flag"] = null; }
            foreach (var row in table)
            {
                if (!string.IsNullOrEmpty(fil))
                {
                    if ((row.p.link1 + " " + row.p.link2 + " " + row.p.carName + " " + row.p.makeyear + " " + row.p.sparePart + " " + row.p.description).ToUpper().Contains(fil.ToUpper()))
                    {
                        filter.Add(new CarPart()
                        {
                            sno = row.p.sno,
                            link1 = row.c.firm,
                            link2 = row.p.link2,
                            carName = row.p.carName,
                            makeyear = row.p.makeyear,
                            sparePart = row.p.sparePart,
                            quantity = row.p.quantity,
                            description = row.p.description
                        });
                    }
                }
                else
                {
                    filter.Add(new CarPart()
                    {
                        sno = row.p.sno,
                        link1 = row.c.firm,
                        link2 = row.p.link2,
                        carName = row.p.carName,
                        makeyear = row.p.makeyear,
                        sparePart = row.p.sparePart,
                        quantity = row.p.quantity,
                        description = row.p.description
                    });
                }
            }
            PagedList<CarPart> model = new PagedList<CarPart>(filter, page, pageSize);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}