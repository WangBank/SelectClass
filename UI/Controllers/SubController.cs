using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;

namespace UI.Controllers
{
    public class SubController : Controller
    {
        private SelectClassEntities db = new SelectClassEntities();

        //
        // GET: /Sub/

        public ActionResult Index()
        {
            return View(db.Sub.ToList());
        }

        //
        // GET: /Sub/Details/5

        public ActionResult Details(int id = 0)
        {
            Sub sub = db.Sub.Find(id);
            if (sub == null)
            {
                return HttpNotFound();
            }
            return View(sub);
        }

        //
        // GET: /Sub/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Sub/Create

        [HttpPost]
        public ActionResult Create(Sub sub)
        {
            if (ModelState.IsValid)
            {
                db.Sub.Add(sub);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sub);
        }

        //
        // GET: /Sub/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Sub sub = db.Sub.Find(id);
            if (sub == null)
            {
                return HttpNotFound();
            }
            return View(sub);
        }

        //
        // POST: /Sub/Edit/5

        [HttpPost]
        public ActionResult Edit(Sub sub)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sub).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sub);
        }

        //
        // GET: /Sub/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Sub sub = db.Sub.Find(id);
            if (sub == null)
            {
                return HttpNotFound();
            }
            return View(sub);
        }

        //
        // POST: /Sub/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Sub sub = db.Sub.Find(id);
            db.Sub.Remove(sub);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}