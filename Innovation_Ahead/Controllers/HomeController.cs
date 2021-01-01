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
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
                    UserRegister postmodel = new UserRegister
                    {
                        Username = Usermodel.Username,
                        Password = Encryption(Usermodel.Password.Trim()),
                        mobileNo = Usermodel.mobileNo
                    };

                    context.UserRegisters.Add(postmodel);

                    if (postmodel.Username != null && postmodel.Password != null)
                    {
                        Session["usern@me"] = postmodel.Username;
                        Session["passw0rd"] = postmodel.Password;
                        context.SaveChanges();
                        return RedirectToAction("Login");
                    }
                    return RedirectToAction("Error");
            //    }

            //    else
            //    {
            //        return RedirectToAction("Error");
            //    }
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Error");
            //}
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

        //private string Decryption(string crypto)
        //{
        //    string key = "MAKV2SPBNI99212";
        //    byte[] converted_crypto = Convert.FromBase64String(crypto);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes decoded = new Rfc2898DeriveBytes()(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = decoded.GetBytes(32);
        //        encryptor.IV = decoded.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(converted_crypto, 0, converted_crypto.Length);
        //                cs.Close();
        //            }
        //            crypto = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return crypto;
        //}

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
            Usermodel.Password = Encryption(Usermodel.Password.Trim());
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

            public ActionResult Index(string selection = "Customer Login")
        {
            selection = "test";
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
                        join p in context.CarParts on c.Username equals p.link1
                        where c.mobileNo == p.link2
                        select p;
            var table = query.ToList();
            List<CarPart> filter = new List<CarPart>();
            foreach (var row in table)
            {
                if (!string.IsNullOrEmpty(fil))
                {
                    if ((row.link1 + " " + row.link2 + " " + row.carName + " " + row.makeyear + " " + row.sparePart).ToUpper().Contains(fil.ToUpper()))
                    {
                        filter.Add(new CarPart()
                        {
                            link1 = row.link1,
                            link2 = row.link2,
                            carName = row.carName,
                            makeyear = row.makeyear,
                            sparePart = row.sparePart
                        });
                    }
                }
                else
                {
                    filter.Add(new CarPart()
                    {
                        link1 = row.link1,
                        link2 = row.link2,
                        carName = row.carName,
                        makeyear = row.makeyear,
                        sparePart = row.sparePart
                    });
                }
            }
            ViewBag.filter = filter;
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
            datamodel.Username = (string)Session["usern@me"];
            datamodel.Password = (string)Session["passw0rd"];
            var data = context.UserRegisters.Where(s => s.Username.Equals(datamodel.Username) && s.Password.Equals(datamodel.Password)).ToList();

            CarPart carpartsObject = new CarPart();
            carpartsObject.carName = carmodel.carName;
            carpartsObject.makeyear = carmodel.makeyear;
            carpartsObject.sparePart = carmodel.sparePart;
            carpartsObject.link1 = (string)Session["usern@me"];
            if (data.Count == 1)
            {
                carpartsObject.link2 = data[0].mobileNo;
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
        
        public ActionResult ClientManagement(string fil = "")
        {
            UserRegister datamodel = new UserRegister();
            datamodel.Username = (string)Session["usern@me"];
            datamodel.Password = (string)Session["passw0rd"];
            var query = from c in context.UserRegisters
                        join p in context.CarParts on c.Username equals p.link1
                        where c.Username == datamodel.Username && c.Password == datamodel.Password && c.mobileNo == p.link2
                        select p;
            var table = query.ToList();
            List<CarPart> filter = new List<CarPart>();
            foreach (var row in table)
            {
                if (!string.IsNullOrEmpty(fil))
                {
                    if ((row.link1 + " " + row.link2 + " " + row.carName + " " + row.makeyear + " " + row.sparePart).ToUpper().Contains(fil.ToUpper()))
                    {
                        filter.Add(new CarPart()
                        {
                            link1 = row.link1,
                            link2 = row.link2,
                            carName = row.carName,
                            makeyear = row.makeyear,
                            sparePart = row.sparePart
                        });
                    }
                }
                else
                {
                    filter.Add(new CarPart()
                    {
                        link1 = row.link1,
                        link2 = row.link2,
                        carName = row.carName,
                        makeyear = row.makeyear,
                        sparePart = row.sparePart
                    });
                }
            }
            ViewBag.filter = filter;
            return View(table);
        }
    }
}