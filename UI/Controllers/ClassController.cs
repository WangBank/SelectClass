using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using Newtonsoft.Json;
using System.IO;

namespace UI.Controllers
{
    public class ClassController : Controller
    {
        private BLLClass bllClass = new BLLClass();
        //
        // GET: /Class/

        public ActionResult Index()
        {
            return View(bllClass.GetAll());
        }

        public JsonResult GetAllClass() {
            List<Class> list = bllClass.GetAll();
            return  Json(list,JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Class/Details/5

        public ActionResult Details(int id)
        {
            IQueryable<Class> modelclass = bllClass.Details(c=>c.ClassForm == id);
            if (modelclass == null)
            {
                return HttpNotFound();
            }
            return View(modelclass);
        }

        //
        // GET: /Class/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Class/Create

        [HttpPost]
        public ActionResult Create(Class modelclass)
        {
            
            if (ModelState.IsValid)
            {
                bllClass.Add(modelclass);
                return RedirectToAction("Index");
            }

            return View(modelclass);
        }

        //
        // GET: /Class/Edit/5

        public ActionResult Edit(int id)
        {
            IQueryable<Class> modelclass = bllClass.Details(c => c.ClassForm == id);
            if (modelclass == null)
            {
                return HttpNotFound();
            }
            return View(modelclass);
        }

        //
        // POST: /Class/Edit/5

        [HttpPost]
        public ActionResult Edit(Class modelclass)
        {
            if (ModelState.IsValid)
            {
                if(bllClass.Edit(modelclass))
                return RedirectToAction("Index");
            }
            return View(modelclass);
        }

        //
        // GET: /Class/Delete/5

        public ActionResult Delete(int id = 0)
        {
            IQueryable<Class> modelclass = bllClass.Details(c => c.ClassForm == id);
            if (modelclass == null)
            {
                return HttpNotFound();
            }
            return View(modelclass);
        }

        //
        // POST: /Class/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!bllClass.Delete(id))
            {
                return Content("<script>alert('该班级已经与相应的学生绑定，不能删除！');window.location.href='/Class/index';</script>");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}   