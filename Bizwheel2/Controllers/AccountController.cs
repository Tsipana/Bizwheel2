using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Bizwheel2.Models;

namespace Bizwheel2.Controllers
{
    public class AccountController : Controller
    {
        private PRG522SA2021Entities db = new PRG522SA2021Entities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(BizDataB bizDataB)
        {
            var Bizwheell = db.BizDataBs.Where(x => x.Email == bizDataB.Email && x.Password == bizDataB.Password).FirstOrDefault();
            if (Bizwheell != null)
            {
                FormsAuthentication.SetAuthCookie(bizDataB.Email, false);
                return RedirectToAction("Index", new RouteValueDictionary(new { Controller = "Home", Action = "Index", id = UrlParameter.Optional }));
            }
            else
            {
                ViewBag.ErrorMessage = "Wrong password";
                return View(bizDataB);
            }
        }

        // GET: Account/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(BizDataB bizDataB)
        {
            if (ModelState.IsValid)
            {
                db.BizDataBs.Add(bizDataB);
                db.SaveChanges();
                FormsAuthentication.SetAuthCookie(bizDataB.Email, false);
                return RedirectToAction("Portfolio", new RouteValueDictionary(new { Controller = "Home", Action = "Portfolio", id = UrlParameter.Optional }));

            }

            return View(bizDataB);
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BizDataB bizDataB = db.BizDataBs.Find(id);
            if (bizDataB == null)
            {
                return HttpNotFound();
            }
            return View(bizDataB);
        }

        // POST: Account/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BizwheelID,Email,Password")] BizDataB bizDataB)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bizDataB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bizDataB);
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BizDataB bizDataB = db.BizDataBs.Find(id);
            if (bizDataB == null)
            {
                return HttpNotFound();
            }
            return View(bizDataB);
        }

        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BizDataB bizDataB = db.BizDataBs.Find(id);
            db.BizDataBs.Remove(bizDataB);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult logOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", new RouteValueDictionary(new { Controller = "Account", Action = "Login", id = UrlParameter.Optional }));
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
