using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Innovation_Ahead.Controllers
{
    public class UserController : Controller
    {
        private VehiclesEntities database = new VehiclesEntities();
        // Details: User
        public ActionResult UserManagement()
        {
            if (Session["em@il"] == null | Session["passw0rd"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRegister accountuser = new UserRegister();
            accountuser.email = (string)Session["em@il"];
            accountuser.password = (string)Session["passw0rd"];
            var query = from c in database.UserRegisters
                        where c.email == accountuser.email && c.password == accountuser.password
                        select c;

            accountuser.mobileNo = query.First().mobileNo;
            accountuser.firm = query.First().firm;
            if (accountuser == null)
            {
                return HttpNotFound();
            }
            return View(accountuser);
        }
        // Delete: User
        public ActionResult DeleteUser(UserRegister deleteuser)
        {
            //if (deleteuser is null)
            //{
            //    throw new ArgumentNullException(nameof(email));
            //}
            //var query = from c in database.UserRegisters
            //            where c.email == deleteuser.email && c.password == deleteuser.password
            //            select c;
            //deleteuser.email = email;
            //deleteuser.password = password;
            //deleteuser.mobileNo = query.First().mobileNo;
            //deleteuser.firm = query.First().firm;

            if (deleteuser == null)
            {
                return HttpNotFound();
            }
            return View(deleteuser);
        }
        // Post delete
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(UserRegister softdelete)
        {
            softdelete.softdelete = 1;
            database.Entry(softdelete).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Logout", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                database.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}