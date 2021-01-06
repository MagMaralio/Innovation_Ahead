using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Innovation_Ahead;

namespace Innovation_Ahead.Controllers
{
    public class CarPartsController : Controller
    {
        private VehiclesEntities db = new VehiclesEntities();

        // GET: CarParts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPart carPart = db.CarParts.Find(id);
            if (carPart == null)
            {
                return HttpNotFound();
            }
            ViewBag.link1 = new SelectList(db.UserRegisters, "email", "password", carPart.link1);
            return View(carPart);
        }

        // POST: CarParts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sno,carName,makeyear,sparePart,quantity,description,link1,link2")] CarPart carPart)
        {
            if (ModelState.IsValid)
            {
                UserRegister datamodel = new UserRegister();
                datamodel.email = (string)Session["em@il"];
                datamodel.password = (string)Session["passw0rd"];
                var data = from c in db.UserRegisters
                           where c.email == datamodel.email && c.password == datamodel.password
                           select c.mobileNo;

                carPart.link1 = datamodel.email;
                carPart.link2 = data.ToList()[0];
                db.Entry(carPart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ClientManagement", "Home");
            }
            ViewBag.link1 = new SelectList(db.UserRegisters, "email", "password", carPart.link1);
            return View(carPart);
        }

        // GET: CarParts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarPart carPart = db.CarParts.Find(id);
            if (carPart == null)
            {
                return HttpNotFound();
            }
            return View(carPart);
        }

        // POST: CarParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarPart carPart = db.CarParts.Find(id);
            db.CarParts.Remove(carPart);
            db.SaveChanges();
            return RedirectToAction("ClientManagement", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
