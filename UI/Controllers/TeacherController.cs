using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using BLL;
using System.Collections;
using System.IO;

namespace UI.Controllers
{
    public class TeacherController : Controller
    {
        private SelectClassEntities db = new SelectClassEntities();
        private BLLStudent bLLStudent = new BLLStudent();
        private BLLClass bllClass = new BLLClass();
        private BLLTeacher bllTeacher = new BLLTeacher();
        public BLLSub bllSub = new BLLSub();
        public ActionResult Index()
        {
            try
            {
                string num = Request.Cookies["teacherInfo"]["userNum"];
                if (string.IsNullOrEmpty(num))
                {
                    return Content("请登录后重试");
                }
            }
            catch {
                return Content("请登录后重试");
            }
            ViewBag.ClassForm = new SelectList(db.Class, "ClassForm", "ClassName");
            return View(bllSub.GetAll().AsEnumerable().OrderByDescending(c=>c.SubID));
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

        public ActionResult CreateSub() {
            ViewBag.TeacherID = new SelectList(bllTeacher.GetAll(), "TeacherID", "TeacherName");
            List<SelectListItem> listOrderState = new List<SelectListItem>();
            listOrderState.Add(new SelectListItem());
            listOrderState.AddRange(bllClass.GetAll().Select(t=>new SelectListItem(){
                Text = t.ClassName,
                Value=t.ClassForm.ToString()
            
            }).ToList());
            ViewBag.ClassForm = listOrderState;
            return View();
        }

        [HttpPost]
        public ActionResult CreateSub(string subName, string teacherID, string subTime, string subPoint, string subBegintime, string subEndtime, string subPeople, string optionsRadios, string ClassForm, string subDM)
        {
            Sub sub = new Sub();
            HttpPostedFileBase file = Request.Files["fileInput"];
            if (file != null)
            {
                var filename = Path.Combine(Server.MapPath("~/Pictures/Subs"), file.FileName);
                file.SaveAs(filename);
                sub.SubPictures = System.Web.HttpUtility.UrlEncode(Path.Combine("/Pictures/Subs", file.FileName), System.Text.Encoding.UTF8);
            }
            if (ModelState.IsValid)
            {
                
                sub.SubName = subName;
                sub.SubDM = subDM;
                sub.ClassAddress = int.Parse(ClassForm);
                sub.TeacherID = int.Parse(teacherID);
                sub.ClassTime = subTime;
                sub.NeedPeople = int.Parse(subPeople);
                sub.SubPoint = int.Parse(subPoint);
                sub.StartTime = subBegintime;
                sub.EndTime = subEndtime;
                if (optionsRadios == "option1")
                {
                    sub.SubJJ = "考试课程";
                }
                else
                {
                    sub.SubJJ = "考察课程";
                }
                bllSub.Add(sub);
                bllSub.SaveChange();
            }
            //跳转到学生管理页面
            return RedirectToAction("SubManage");
        }

        public ActionResult SubManage() {

            return View(bllSub.GetAll().AsEnumerable());
        }

        public ActionResult DeleteSub(string subId)
        {
            string[] subids;
            subids = subId.Split(',');
            for (int i = 0; i < subids.Length; i++)
            {
                bllSub.Delete(int.Parse(subids[i]));
            }
            bllSub.SaveChange();
            return Content("ok");
        }

        [HttpGet]
        public ActionResult EditSub(string  subId)
        {

            int subidint = int.Parse(subId);
            return View(bllSub.Details(c=>c.SubID ==subidint).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult EditSub(string subID, string subName, string teacherID, string ClassTime, string subPoint, string StartTime, string EndTime, string NeedPeople, string ClassAddress, string SubJJ, string SubDM)
        {
            Sub sub = new Sub();
            HttpPostedFileBase file = Request.Files["fileInput"];
            if (file != null)
            {
                var filename = Path.Combine(Server.MapPath("~/Pictures/Subs"), file.FileName);
                file.SaveAs(filename);
                sub.SubPictures = System.Web.HttpUtility.UrlEncode(Path.Combine("/Pictures/Subs", file.FileName), System.Text.Encoding.UTF8);
            }
            if (ModelState.IsValid)
            {
                sub.SubDM = SubDM;
                sub.SubID = int.Parse(subID);
                sub.SubName = subName;
                sub.ClassAddress = int.Parse(ClassAddress);
                sub.TeacherID = int.Parse(teacherID);
                sub.ClassTime = ClassTime;
                sub.NeedPeople = int.Parse(NeedPeople);
                sub.SubPoint = Decimal.Parse(subPoint);
                sub.StartTime = StartTime;
                sub.EndTime = EndTime;
                sub.SubJJ = SubJJ;
                bllSub.Edit(sub);
                bllSub.SaveChange();
            }
            return RedirectToAction("SubManage");
        }
        public ActionResult CreateStudent()
        {
            ViewBag.ClassForm = new SelectList(bllClass.GetAll(), "ClassForm", "ClassNum");
            return View();
        }
        [HttpPost]
        public ActionResult CreateStudent(string registerUsername, string registerUserID, string registerPassword, string ClassForm)
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
            //跳转到学生管理页面
            return RedirectToAction("StudentManage");
        }
        [HttpPost]
        public ActionResult EditInfo(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                Teacher teacher = new Teacher();
                HttpPostedFileBase file = Request.Files["fileInput"];
                if (file != null)
                {
                        var filename = Path.Combine(Server.MapPath("~/Pictures/Teachers"), file.FileName);
                        file.SaveAs(filename);
                }
                teacher.TeacherPicture = System.Web.HttpUtility.UrlEncode(Path.Combine("/Pictures/Teachers", file.FileName), System.Text.Encoding.UTF8);
                teacher.TeacherPwd = password;
                teacher.TeacherID = int.Parse(Request.Cookies["teacherInfo"]["teacherID"]);
                teacher.TeacherName = System.Web.HttpUtility.UrlDecode(Request.Cookies["teacherInfo"]["teacherName"],System.Text.Encoding.UTF8);
                teacher.TeacherNum = Request.Cookies["teacherInfo"]["userNum"] ;
                bllTeacher.Edit(teacher);
                bllTeacher.SaveChange();
                return Redirect("/Login/index");
            }
            else {
                return RedirectToAction("index");
            }
            
        }

        //[HttpPost]
        //public ActionResult UploadFile()
        //{
        //    HttpPostedFileBase file = Request.Files["fileInput"];
        //    if (file != null)
        //    {
        //        try
        //        {
        //            var filename = Path.Combine(Server.MapPath("~/Pictures/Teachers"), file.FileName);
        //            file.SaveAs(filename);
        //            return Content("上传成功");
        //        }
        //        catch (Exception ex)
        //        {
        //            return Content(string.Format("上传文件出现异常：{0}", ex.Message));
        //        }

        //    }
        //    else
        //    {
        //        return Content("没有文件需要上传！");
        //    }
        //}

        //
        // GET: /Teacher/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Teacher teacher = db.Teacher.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        public ActionResult StudentManage() {
            
            return View(bLLStudent.GetAll().AsEnumerable());
        }

        //
        // POST: /Teacher/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = db.Teacher.Find(id);
            db.Teacher.Remove(teacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteStudent(string stuId)
        {
            string[] stuids;
            stuids = stuId.Split(',');
            for (int i = 0; i < stuids.Length; i++)
            {
                bLLStudent.Delete(int.Parse(stuids[i]));
            }
            bLLStudent.SaveChange();
            return Content("ok");
        }


        public ActionResult TeacherInfo() {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}