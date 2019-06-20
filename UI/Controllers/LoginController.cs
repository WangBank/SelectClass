using Model;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace UI.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private SelectClassEntities db = new SelectClassEntities();
        private BLLTeacher bllTeacher = new BLLTeacher();
        private BLLStudent bllStudent = new BLLStudent();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TeacherIndex()
        {
            return View();
        }

        public ActionResult StudentLogin(string userNum, string pwd)
        {
           var student = bllStudent.Details(c => c.StuNum == userNum);
           if (student.Count() == 0)
           {
               return Content("请联系管理员添加相关信息!");
           }
           if (student.FirstOrDefault().StuPwd != pwd) {
               return Content("密码错误");
           }
           if (Request.Cookies["studentInfo"] == null)
           {
               Request.Cookies.Remove("studentInfo");
               HttpCookie studentInfoCookies = new HttpCookie("studentInfo");
               studentInfoCookies["studentNum"] = userNum;
               studentInfoCookies["pwd"] = pwd;
               String studentName = System.Web.HttpUtility.UrlEncode(bllStudent.Details(c => c.StuNum == userNum).FirstOrDefault().StuName, System.Text.Encoding.UTF8);
               studentInfoCookies["studentName"] = studentName;
               studentInfoCookies["studentPicture"] = bllStudent.Details(c => c.StuNum == userNum).FirstOrDefault().StuPicture;
               Response.Cookies.Add(studentInfoCookies);
               return Content("ok");
           }
           else {
               Request.Cookies.Remove("studentInfo");
               HttpCookie studentInfoCookies = new HttpCookie("studentInfo");
               studentInfoCookies["studentNum"] = userNum;
               studentInfoCookies["pwd"] = pwd;
               String studentName = System.Web.HttpUtility.UrlEncode(bllStudent.Details(c => c.StuNum == userNum).FirstOrDefault().StuName, System.Text.Encoding.UTF8);
               studentInfoCookies["studentName"] = studentName;
               studentInfoCookies["studentPicture"] = bllStudent.Details(c => c.StuNum == userNum).FirstOrDefault().StuPicture;
               Response.Cookies.Add(studentInfoCookies);
               return Content("ok");
           }
        }

        public ActionResult TeacherLogin(string userNum, string pwd)
        {
            var student = bllTeacher.Details(c => c.TeacherNum == userNum);
            if (student.Count() == 0)
            {
                return Content("请注册后登录！");
            }
            if (student.FirstOrDefault().TeacherPwd != pwd)
            {
                return Content("密码错误");
            }
            if (Request.Cookies["teacherInfo"] == null)
            {
                Request.Cookies.Remove("teacherInfo");
                HttpCookie teacherInfoCookies = new HttpCookie("teacherInfo");
                teacherInfoCookies["teacherID"] = bllTeacher.Details(c => c.TeacherNum == userNum).FirstOrDefault().TeacherID.ToString();
                teacherInfoCookies["userNum"] = userNum;
                teacherInfoCookies["pwd"] = pwd;
                //将中文编码
                String teacherName = System.Web.HttpUtility.UrlEncode(bllTeacher.Details(c => c.TeacherNum == userNum).FirstOrDefault().TeacherName, System.Text.Encoding.UTF8);
                teacherInfoCookies["teacherName"] = teacherName;
                teacherInfoCookies["teacherPicture"] = bllTeacher.Details(c => c.TeacherNum == userNum).FirstOrDefault().TeacherPicture;
                Response.Cookies.Add(teacherInfoCookies);
                return Content("ok");
            }
            else
            {
                Request.Cookies.Remove("teacherInfo");
                HttpCookie teacherInfoCookies = new HttpCookie("teacherInfo");
                teacherInfoCookies["teacherID"] = bllTeacher.Details(c => c.TeacherNum == userNum).FirstOrDefault().TeacherID.ToString();
                teacherInfoCookies["userNum"] = userNum;
                teacherInfoCookies["pwd"] = pwd;
                //将中文编码
                String teacherName = System.Web.HttpUtility.UrlEncode(bllTeacher.Details(c => c.TeacherNum == userNum).FirstOrDefault().TeacherName, System.Text.Encoding.UTF8);
                teacherInfoCookies["teacherName"] = teacherName;
                teacherInfoCookies["teacherPicture"] = bllTeacher.Details(c => c.TeacherNum == userNum).FirstOrDefault().TeacherPicture;
                Response.Cookies.Add(teacherInfoCookies);
                return Content("ok");
            }
            
        }

        public ActionResult TeacherRegisterIndex() {
            return View();
        }

        public ActionResult TeacherRegister(string userName, string pwd, string userNum)
        {
            Teacher teacher = new Teacher ();
            teacher.TeacherName =userName;
            teacher.TeacherNum = userNum;
            teacher.TeacherPwd = pwd;
            teacher.TeacherPicture = "Test";
            try
            {
                bllTeacher.Add(teacher);
                bllTeacher.SaveChange();
                return Content("ok");              
            }
            catch {
                return Content("false"); 
            
            }
                



        }
    }
}
