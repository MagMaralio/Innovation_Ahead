using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Innovation_Ahead.Controllers
{
    public class MailAuthenticateController : Controller
    {
        // GET: MailAuthenticate
        public ActionResult sendmail()
        {
            string HostAddress = ConfigurationManager.AppSettings["Host"].ToString();
            string FormEmailId = ConfigurationManager.AppSettings["MailFrom"].ToString();
            string Password = ConfigurationManager.AppSettings["Password"].ToString();
            string Port = ConfigurationManager.AppSettings["Port"].ToString();
            MailMessage mailmodel = new MailMessage();
            mailmodel.From = new MailAddress("aaromal303@gmail.com");
            mailmodel.Subject = "Authentication mail";
            mailmodel.Body = "Please click on the link below to authenticate";
            mailmodel.To.Add(new MailAddress((string)Session["em@il"]));

            SmtpClient smtp = new SmtpClient();
            smtp.Host = HostAddress;
            smtp.EnableSsl = true;

            NetworkCredential networkCredential = new NetworkCredential();
            networkCredential.UserName = "aaromal303@gmail.com";
            networkCredential.Password = "mzbbybdcvvntxmoy";
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = Convert.ToInt32(Port);
            smtp.Send(mailmodel);

            return RedirectToAction("Index", "Home");
        }
    }
}