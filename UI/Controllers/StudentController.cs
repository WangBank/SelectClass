using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using System.IO;

namespace UI.Controllers
{
    public class StudentController : Controller
    {
        private SelectClassEntities db = new SelectClassEntities();
        private BLLStudent bLLStudent = new BLLStudent();
        private BLLClass bllClass = new BLLClass();
        public BLLSub bllSub = new BLLSub();
        public BLLSelectSub bllselectsub = new BLLSelectSub();
        //
        // GET: /Student/

        public ActionResult Index()
        {
            try
            {
                string num = Request.Cookies["studentInfo"]["studentNum"];
                if (string.IsNullOrEmpty(num))
                {
                    return Content("请登录后重试");
                }
                int stuid = bLLStudent.Details(c => c.StuNum == num).FirstOrDefault().StuID;
            ViewBag.ClassForm = new SelectList(db.Class, "ClassForm", "ClassName");
            var s = bllselectsub.GetAll().Where(c => c.StuID == stuid).ToList();
                List<int> list = new List<int> ();
            foreach (var item in s)
            {
                list.Add(item.SubID);
            }
            //return View(bllSub.GetAll().Where(c=>list.Contains(c.SubID)).AsEnumerable());
            return View(bllSub.GetAll().AsEnumerable().OrderByDescending(c => c.SubID));
            }
            catch
            {
                return Content("请登录后重试");
            }
            
        }


        /// <summary>
        /// 考试课程
        /// </summary>
        /// <returns></returns>
        public ActionResult kskc() {
            ViewBag.ClassForm = new SelectList(db.Class, "ClassForm", "ClassName");
            return View(bllSub.GetAll().Where(c=>c.SubJJ=="考试课程").AsEnumerable());
        }



        /// <summary>
        /// 考察课程
        /// </summary>
        /// <returns></returns>
        public ActionResult kckc() {
            ViewBag.ClassForm = new SelectList(db.Class, "ClassForm", "ClassName");
            return View(bllSub.GetAll().Where(c => c.SubJJ == "考察课程").AsEnumerable());
        }

      [HttpGet]
        public ActionResult EditInfo() {

            return View();
        }

        [HttpPost]
      public ActionResult EditInfo(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                string num = Request.Cookies["studentInfo"]["studentNum"];
                Student student = bLLStudent.Details(c => c.StuNum == num).FirstOrDefault();
                HttpPostedFileBase file = Request.Files["fileInput"];
                if (file != null)
                {
                    var filename = Path.Combine(Server.MapPath("~/Pictures/Students"), file.FileName);
                    file.SaveAs(filename);
                }
                student.StuPicture = System.Web.HttpUtility.UrlEncode(Path.Combine("/Pictures/Students", file.FileName), System.Text.Encoding.UTF8);
                
                
                student.StuPwd = password;
                bLLStudent.Edit(student);
                bLLStudent.SaveChange();
                return Redirect("/Login/index");
            }
            else
            {
                return RedirectToAction("index");
            }
        }

        public ActionResult kskcselect(string subID)
        {
            try
            {
                string[] subIDs;
                subIDs = subID.Split(',');
                Sub subss = new Sub();
                Student stu = new Student();
                SelectSub ss = new SelectSub();
                string num = Request.Cookies["studentInfo"]["studentNum"];
                int stuid = bLLStudent.Details(c => c.StuNum == num).FirstOrDefault().StuID;
                stu = bLLStudent.Details(c => c.StuNum == num).FirstOrDefault();
                string msg = "";
                for (int i = 0; i < subIDs.Length; i++)
                {

                    stu = null;
                    int nowdi = int.Parse(subIDs[i]);
                    subss = bllSub.Details(c => c.SubID == nowdi).FirstOrDefault();

                    if (bllselectsub.Details(c => c.StuID == stuid && c.SubID == nowdi).Count() != 0)
                    {
                        msg = "您已选择这门课程，不允许重复选择！~";
                                           
                    }else
                    if (subss.ClassPeople < subss.NeedPeople)
                    {

                        int peopels = subss.ClassPeople + 1;
                        subss.ClassPeople = peopels;
                        ss.StuID = stuid;
                        ss.SubID = nowdi;
                        bllselectsub.Add(ss);
                        bllSub.Edit(subss);
                        bllselectsub.SaveChange();
                        bllSub.SaveChange();
                        msg = "ok";
                    }
                    else
                    {

                        msg = "此课程选课人数已满！，不允许选课";
                    }


                }
                return Content(msg);
            }
            catch {
                return Content("没有相应的数据，请刷新重试！");            
            }
        }


        public ActionResult sxkc()
        {
            string num = Request.Cookies["studentInfo"]["studentNum"];
            int stuid = bLLStudent.Details(c => c.StuNum == num).FirstOrDefault().StuID;
            ViewBag.ClassForm = new SelectList(db.Class, "ClassForm", "ClassName");
            var s = bllselectsub.GetAll().Where(c => c.StuID == stuid).ToList();
            List<int> list = new List<int>();
            foreach (var item in s)
            {
                list.Add(item.SubID);
            }
            return View(bllSub.GetAll().Where(c => list.Contains(c.SubID)).AsEnumerable());
        }

        public ActionResult cancelselect(string subId)
        {
            try
            {
                int subids = int.Parse(subId);
                string num = Request.Cookies["studentInfo"]["studentNum"];
                int stuid = bLLStudent.Details(c => c.StuNum == num).FirstOrDefault().StuID;
                int ssid = bllselectsub.Details(c => c.SubID == subids && c.StuID == stuid).FirstOrDefault().SelectSubID;
                bllselectsub.Delete(ssid);
                bllselectsub.SaveChange();

                Sub subss = bllSub.Details(c => c.SubID == subids).FirstOrDefault();
                int peopels = subss.ClassPeople - 1;
                subss.ClassPeople = peopels;
                bllSub.Edit(subss);
                bllSub.SaveChange();
                return Content("ok");
            }
            catch {
                return Content("请每次只选择一条数据！");
            }

            
        }

        //
        // GET: /Student/Details/5

        public ActionResult Details(int id = 0)
        {
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            ViewBag.ClassForm = new SelectList(bllClass.GetAll(), "ClassForm", "ClassName");
            return View();
        }

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(string registerUsername,string registerUserID,string registerPassword,string ClassForm)
        {
            Student student = new Student();
            int classform = bllClass.GetClassFormByName(ClassForm);
            if (ModelState.IsValid)
            {
                student.StuNum = registerUserID;
                student.StuName = registerUsername;
                student.StuPwd = registerPassword;
                student.ClassForm = classform;
                bLLStudent.Add(student);
                bLLStudent.SaveChange();
            }
            return View();
        }

        //
        // GET: /Student/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassForm = new SelectList(db.Class, "ClassForm", "ClassName", student.ClassForm);
            return RedirectToAction("Index");
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassForm = new SelectList(db.Class, "ClassForm", "ClassName", student.ClassForm);
            return RedirectToAction("Index");
        }

        //
        // GET: /Student/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Student.Find(id);
            db.Student.Remove(student);
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