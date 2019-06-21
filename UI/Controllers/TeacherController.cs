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
using System.Web.UI;
using NPOI.HSSF.UserModel;

namespace UI.Controllers
{
    public class TeacherController : Controller
    {
        private SelectClassEntities db = new SelectClassEntities();
        private BLLStudent bLLStudent = new BLLStudent();
        private BLLClass bllClass = new BLLClass();
        private BLLTeacher bllTeacher = new BLLTeacher();
        public BLLSub bllSub = new BLLSub();
        public BLLSelectSub selectSub = new BLLSelectSub();
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
        public void CreateSub(string subName, string teacherID, string subTime, string subPoint, string subBegintime, string subEndtime, string subPeople, string optionsRadios, string ClassForm, string subDM)
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
                if (DateTime.Parse(subBegintime) > DateTime.Parse(subEndtime)) {
                    Response.Write("<script language='JavaScript'>alert('起始日期不能大于截至日期');history.go(-1);</script>");
                    return;
                }
                else if (DateTime.Parse(subBegintime) < DateTime.Now) {
                    Response.Write("<script language='JavaScript'>alert('起始日期不能小于当前日期');history.go(-1);</script>");
                    return;
                }
                else
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
                RedirectToAction("SubManage");
            }
               
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
        public void EditSub(string subID, string subName, string teacherID, string ClassTime, string subPoint, string StartTime, string EndTime, string NeedPeople, string ClassAddress, string SubJJ, string SubDM)
        {
            Sub sub = new Sub();
            HttpPostedFileBase file = Request.Files["fileInput"];
            if (file != null)
            {
                var filename = Path.Combine(Server.MapPath("~/Pictures/Subs"), file.FileName);
                file.SaveAs(filename);
                sub.SubPictures = System.Web.HttpUtility.UrlEncode(Path.Combine("/Pictures/Subs", file.FileName), System.Text.Encoding.UTF8);
            }
            try
            {
                if (ModelState.IsValid)
                {

                    if (DateTime.Parse(StartTime) > DateTime.Parse(EndTime))
                    {
                        Response.Write("<script language='JavaScript'>alert('起始日期不能大于截至日期');history.go(-1);</script>");
                        return;
                    }
                    else if (DateTime.Parse(StartTime) < DateTime.Now)
                    {
                        Response.Write("<script language='JavaScript'>alert('起始日期不能小于当前日期');history.go(-1);</script>");
                        return;
                    }
                    else
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
                }
                Response.Write("<script language='JavaScript'>alert('修改完毕，刷新后查看结果！');window.close();window.opener.reload();</script>");
            }
            catch {
                Response.Write("<script language='JavaScript'>alert('请输入正确的日期格式！');history.go(-1);</script>");
                return;
            }
        }

        public ActionResult Edit() {

            return View();
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

        public ActionResult ExportExcel(int subID) {
            List<Student> students = new List<Student>();
            var studentsid =  selectSub.GetAll().Where(c => c.SubID == subID);
            foreach (var item in studentsid)
            {
                students.Add(bLLStudent.Details(c=>c.StuID == item.StuID).FirstOrDefault());
            }
            MemoryStream stream = new MemoryStream();
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("ID");
            headerRow.CreateCell(1).SetCellValue("学生姓名");
            headerRow.CreateCell(2).SetCellValue("学生学号");
            headerRow.CreateCell(3).SetCellValue("学生班级");
            for (int i = 0; i < students.Count; i++)
            {
                foreach (var item in students)
                {
                    var newRow = sheet.CreateRow(i + 1);
                    newRow.CreateCell(0).SetCellValue(item.StuID);
                    newRow.CreateCell(1).SetCellValue(item.StuName);
                    newRow.CreateCell(2).SetCellValue(item.StuNum);
                    newRow.CreateCell(3).SetCellValue(bllClass.Details(c=>c.ClassForm == item.ClassForm).FirstOrDefault().ClassNum);
                }
            }

            workbook.Write(stream);
            stream.Flush();
            stream.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;
            return File(stream,"application/vnd.ms-excel","选择" + bllSub.Details(c=>c.SubID == subID).FirstOrDefault().SubName + "此课程的学生信息.xls");
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
            int nowid = 0;
            for (int i = 0; i < stuids.Length; i++)
            {
                nowid = int.Parse(stuids[i]);
                if (selectSub.Details(c=>c.StuID== nowid).Count()!=0){
                    return Content("该学生已经学过课程，不能删除！！");
                }
                else
                {
                    bLLStudent.Delete(nowid);
                }
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