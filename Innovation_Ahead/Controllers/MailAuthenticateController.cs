using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Innovation_Ahead.Controllers
{
    public class MailAuthenticateController : Controller
    {
        private VehiclesEntities datapool = new VehiclesEntities();
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
            var enKey = Encryption((string)Session["em@il"]);
            enKey = enKey.Replace("+", "%20");
            mailmodel.Body = ("Please click on the link below to authenticate" 
                + " https://localhost:44360/MailAuthenticate/validate/"+enKey);
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
        public ActionResult validate(string id)
        {
            id = id.Replace(" ", "+");
            string email = decryption(id);
            var userdetails = datapool.UserRegisters.Where(s => s.email.Equals(email)).ToList();

            foreach (UserRegister user in userdetails)
            {
                user.authToken = 1;
            }
            datapool.SaveChanges();
            return RedirectToAction("Login", "Home");
        }

        private string Encryption(string credential)
        {
            string key = "makv2spbni30039";
            byte[] converted_secret = Encoding.Unicode.GetBytes(credential);
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
                    credential = Convert.ToBase64String(ms.ToArray());
                }
            }
            return credential;
        }
        private string decryption(string email)
        {
            string key = "makv2spbni30039";
            byte[] converted_crypto = Convert.FromBase64String(email);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes decoded = new Rfc2898DeriveBytes(key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = decoded.GetBytes(32);
                encryptor.IV = decoded.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(converted_crypto, 0, converted_crypto.Length);
                        cs.Close();
                    }
                    email = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return email;
        }
    }
}