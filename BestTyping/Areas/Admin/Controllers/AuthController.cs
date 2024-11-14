using BestTyping.Models;
using BestTyping.Security;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: Admin/Auth
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult Login(string username,string pass)
        {
            var checkAdmin =  db.USERs.FirstOrDefault(u => u.TaiKhoan == username);
            if (checkAdmin == null || checkAdmin.TypeAccount != 2) {
                return Json(new { code = 400, msg = "Sai mật khẩu hoặc tài khoản" });
            }
            if (!BCrypt.Net.BCrypt.Verify(pass, checkAdmin.MatKhau))
            {
                return Json(new { code = 400 , msg = "Sai mật khẩu hoặc tài khoản" });
            }
            Session["User"] = checkAdmin;
            return Json(new { code = 200, msg = "Đăng Nhập thành công" });
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index","Auth");
        }
    }
}