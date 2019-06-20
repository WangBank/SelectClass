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
    public class SelectSubController : Controller
    {
        private SelectClassEntities db = new SelectClassEntities();

        //
        // GET: /SelectSub/

        public ActionResult Index()
        {
            var selectsub = db.SelectSub.Include(s => s.Student).Include(s => s.Sub);
            return View(selectsub.ToList());
        }

        //
        // GET: /SelectSub/Details/5

        public ActionResult Details(int id = 0)
        {
            SelectSub selectsub = db.SelectSub.Find(id);
            if (selectsub == null)
            {
                return HttpNotFound();
            }
            return View(selectsub);
        }

        //
        // GET: /SelectSub/Create

        public ActionResult Create()
        {
            ViewBag.StuID = new SelectList(db.Student, "StuID", "StuName");
            ViewBag.SubID = new SelectList(db.Sub, "SubID", "SubName");
            return View();
        }

        //
        // POST: /SelectSub/Create

        [HttpPost]
        public ActionResult Create(SelectSub selectsub)
        {
            if (ModelState.IsValid)
            {
                db.SelectSub.Add(selectsub);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StuID = new SelectList(db.Student, "StuID", "StuName", selectsub.StuID);
            ViewBag.SubID = new SelectList(db.Sub, "SubID", "SubName", selectsub.SubID);
            return View(selectsub);
        }

        //
        // GET: /SelectSub/Edit/5

        public ActionResult Edit(int id = 0)
        {
            SelectSub selectsub = db.SelectSub.Find(id);
            if (selectsub == null)
            {
                return HttpNotFound();
            }
            ViewBag.StuID = new SelectList(db.Student, "StuID", "StuName", selectsub.StuID);
            ViewBag.SubID = new SelectList(db.Sub, "SubID", "SubName", selectsub.SubID);
            return View(selectsub);
        }

        //
        // POST: /SelectSub/Edit/5

        [HttpPost]
        public ActionResult Edit(SelectSub selectsub)
        {
            if (ModelState.IsValid)
            {
                db.Entry(selectsub).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StuID = new SelectList(db.Student, "StuID", "StuName", selectsub.StuID);
            ViewBag.SubID = new SelectList(db.Sub, "SubID", "SubName", selectsub.SubID);
            return View(selectsub);
        }

        //
        // GET: /SelectSub/Delete/5

        public ActionResult Delete(int id = 0)
        {
            SelectSub selectsub = db.SelectSub.Find(id);
            if (selectsub == null)
            {
                return HttpNotFound();
            }
            return View(selectsub);
        }

        //
        // POST: /SelectSub/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SelectSub selectsub = db.SelectSub.Find(id);
            db.SelectSub.Remove(selectsub);
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