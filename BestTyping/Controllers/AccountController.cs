using System;
using System.Collections.Generic;
using BCrypt.Net;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BestTyping.Models;
using hbehr.recaptcha;

namespace BestTyping.Controllers
{
    public class AccountController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: Account
        [HttpPost]
        public JsonResult CheckCaptcha(string captcharesponse)
        {
            string userResponse = captcharesponse;
            bool validCapcha = ReCaptcha.ValidateCaptcha(userResponse);
            if (validCapcha)
            {
                return Json(new { result = "PASS" });
            }
            else
            {
                return Json(new { result = "FAIL" });
            }
        }

        [HttpGet]
        public ActionResult VerifyEmail(string token, string email)
        {
            var user = db.USERs.FirstOrDefault(u => u.Email.Equals(email) && u.LinkVerification.Equals(token));
            if (user != null)
            {
                user.IsEnable = true;
                db.SubmitChanges();
                ViewBag.IsCheck = true;
                ViewBag.Notification = "Xác thực Email thành công!";
                ViewBag.Content = "Email của bạn đã được xác minh thành công. Bây giờ bạn có thể đăng nhập vào tài khoản của mình.";
                return View();
            }
            else
            {
                ViewBag.IsCheck = false;
                ViewBag.Notification = "Xác thực Email không thành công!";
                ViewBag.Content = "Đã xảy ra lỗi khi xác minh email của bạn. Hãy đảm bảo rằng bạn đã nhấp vào đúng liên kết xác minh hoặc liên hệ với bộ phận hỗ trợ.";
                return View();
            }
        }
        [HttpPost]
        public JsonResult RegisterAccount(string fullname,string username,string password,string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(fullname) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(email))
                {
                    var checkUser = db.USERs.Where(n => n.Email == email || n.TaiKhoan == username).SingleOrDefault();
                    if (checkUser == null)
                    {
                        var verifyToken = Guid.NewGuid().ToString();
                        var confirmationLink = Url.Action("VerifyEmail", "Account", new { token = verifyToken, email = email }, Request.Url.Scheme);
                        SendEmail(email, "Xác thực tài khoản BestTyping", confirmationLink, 0);

                        // Đây là ví dụ sử dụng milliseconds từ epoch
                        DateTime now = DateTime.UtcNow;
                        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        long milliseconds = (long)(now - epoch).TotalMilliseconds;

                        USER user = new USER();
                        user.HoTen = fullname;
                        user.TaiKhoan = username;
                        user.Email = email;
                        user.MatKhau = BCrypt.Net.BCrypt.HashPassword(password);
                        user.IsEnable = false;
                        user.Avatar = "https://firebasestorage.googleapis.com/v0/b/login-besttyping.appspot.com/o/Avatars%2Fuser-default.jpg?alt=media&token=a336a970-1f58-46fe-91c3-362edad77581";
                        user.CreateDate = milliseconds;
                        user.TypeAccount = 1;
                        user.LinkVerification = verifyToken;
                        db.USERs.InsertOnSubmit(user);
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Vui lòng kiểm tra Email để xác minh tài khoản" });
                    }
                    else
                    {
                        return Json(new { code = 400, msg = "Tài khoản hoặc Email đã tồn tại!" });
                    }
                    
                }
                else
                {
                    return Json(new { code = 500, msg = "Có lỗi xảy ra" });
                }
            }
            catch(Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }           
        }
        [HttpPost]
        public JsonResult LoginWithSocial(string fullname,  string email,string avatar)
        {
            try
            {
                if (!string.IsNullOrEmpty(fullname) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(avatar))
                {
                    var checkUser = db.USERs.Where(n => n.Email == email).SingleOrDefault();
                    if (checkUser == null)
                    {
                        USER user = new USER();
                        user.HoTen = fullname;
                        user.Email = email;
                        user.Avatar = avatar;
                        user.IsEnable = true;
                        user.TypeAccount = 1;
                        db.USERs.InsertOnSubmit(user);
                        db.SubmitChanges();
                        Session["User"] = user;
                        return Json(new { code = 200, msg = "Đâng nhập thành công" });
                    }
                    else
                    {
                        Session["User"] = checkUser;
                        return Json(new { code = 400, msg = "Đâng nhập thành công" });
                    }

                }
                else
                {
                    return Json(new { code = 500, msg = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CheckMail(string recipientEmail,string username)
        {
            try
            {
                var checkUser = db.USERs.FirstOrDefault(u => u.Email == recipientEmail && u.TaiKhoan == username);
                if (checkUser == null)
                {
                    return Json(new { code = 500, msg = "Tài khoản hoặc Email không tồn tại" });
                }
                else
                {
                    return Json(new { code = 200, msg = "Vui lòng kiểm tra Email của bạn" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ResetPassWord(string username, string password, string email,string sendcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(sendcode))
                {
                    var checkUser = db.USERs.Where(n => n.Email == email && n.TaiKhoan == username).SingleOrDefault();
                    if (checkUser != null)
                    {
                        if(checkUser.XacMinh == sendcode)
                        {
                            checkUser.MatKhau = BCrypt.Net.BCrypt.HashPassword(password);
                            db.SubmitChanges();
                            return Json(new { code = 200, msg = "Đổi mật khẩu thành công" });
                        }
                        else
                        {
                            return Json(new { code = 500, msg = "Mã xác minh không hợp lệ!" });
                        }
                       
                    }
                    else
                    {
                        return Json(new { code = 400, msg = "Tài khoản không tồn tại" });
                    }
                }
                else
                {
                    return Json(new { code = 500, msg = "Có lỗi xảy ra" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult CheckLogin(string usernamesignin,string passsignin)
        {
            try
            {
                var checkUser = db.USERs.FirstOrDefault(u => u.TaiKhoan == usernamesignin);
                if (checkUser != null)
                {
                    // Sử dụng BCrypt để kiểm tra mật khẩu
                    if (BCrypt.Net.BCrypt.Verify(passsignin, checkUser.MatKhau))
                    {
                        // Kiểm tra xem tài khoản đã được kích hoạt chưa
                        if (checkUser.IsEnable == false)
                        {
                            return Json(new { code = 400, msg = "Vui lòng kiểm tra Email để kích hoạt tài khoản" });
                        }
                        else
                        {
                            Session["User"] = checkUser;
                            return Json(new { code = 200, msg = "Đăng Nhập thành công" });
                        }
                    }
                    else
                    {
                        return Json(new { code = 500, msg = "Tài khoản hoặc mật khẩu không hợp lệ!" });
                    }                   
                }
                else
                {
                    return Json(new { code = 500, msg = "Tài khoản hoặc mật khẩu không hợp lệ!" });
                }              
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SendVerificationCode(string recipientEmail)
        {
            try
            {
                var checkUser = db.USERs.FirstOrDefault(u => u.Email == recipientEmail);
                if(checkUser!= null)
                {
                    // Tạo mã xác minh ngẫu nhiên gồm 6 ký tự
                    string verificationCode = GenerateVerificationCode();
                    checkUser.XacMinh = verificationCode;
                    db.SubmitChanges();
                    // Gửi mã xác minh qua email
                    SendEmail(recipientEmail, "Mã xác minh đổi mật khẩu", verificationCode, 1);

                    return Json(new { code = 200, msg = "Mã xác nhận đã gửi" });
                }else
                {
                    return Json(new { code = 500, msg = "Email không tồn tại!" });
                }    
                
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        private string GenerateVerificationCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }

            return sb.ToString();
        }
        private void SendEmail(string recipientEmail, string subject, string message, int select)
        {
            string template;
            if (select == 0)
            {
                template = $@"
            <table  
                border=""0"" align=""center"" cellspacing=""0"" cellpadding=""0"" bgcolor=""white"" width=""650"" style=""background: #f5d9c0;padding: 0 20px 0 65px;border-radius: 10px;justify-content: center;display: flex;"">
                <tr>
                    <td>
                        <!--Child table-->
                        <table border=""0"" cellspacing=""0"" cellpadding=""0"" style=""color:#0f3462; font-family: sans-serif;"">
                            <tr>
                                <td>
                                    <h1 style=""text-align:center; margin-top: 25px;font-size:50px;"">
                                        <i>Best</i><span style=""color:lightcoral"">Typing</span>
                                    </h1>
                                </td>
                            </tr>
                            <tr>
                                <td style=""text-align: center;"">
                                                    <p style=""font-size:30px;margin: 0 0 10px 0;color: #36b445;text-align:center;text-align: center;"">Click vào nút bên dưới để xác thực Email</p>
                                                    <a href=""{message}"" style=""padding:8px 10px;background:green;font-size:20px;color:white;margin: 0;font-weight: 600;text-decoration: none;border-radius: 10px;"">Xác thực Email</p>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p style=""font-size:30px;color: #0f3462;font-weight: 600;"">Vui lòng kiểm tra thông tin, Cảm ơn!</p>                                   
                                </td>
                            </tr>
                        </table>
                        <!--/Child table-->
                    </td>
                </tr>
            </table>";
            }
            else
            {
                template = $@"
            <table  
                border=""0"" align=""center"" cellspacing=""0"" cellpadding=""0"" bgcolor=""white"" width=""650"" style=""background: #f5d9c0;padding: 0 20px 0 65px;border-radius: 10px;justify-content: center;display: flex;"">
                <tr>
                    <td>
                        <!--Child table-->
                        <table border=""0"" cellspacing=""0"" cellpadding=""0"" style=""color:#0f3462; font-family: sans-serif;"">
                            <tr>
                                <td>
                                    <h1 style=""text-align:center; margin-top: 25px;font-size:50px;"">
                                        <i>Best</i><span style=""color:lightcoral"">Typing</span>
                                    </h1>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p style=""font-size:30px;color: #36b445;margin: 0;text-align:center;text-align: center;"">Mã xác minh của bạn là:</p>
                                    <p style=""font-size:60px;color:lightcoral;margin: 0;text-align: center;font-weight: 600;"">{message}</p>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p style=""font-size:30px;color: #0f3462;font-weight: 600;"">Vui lòng kiểm tra thông tin, Cảm ơn!</p>                                   
                                </td>
                            </tr>
                        </table>
                        <!--/Child table-->
                    </td>
                </tr>
            </table>";
            }                      
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("pingvocuc555@gmail.com","BestTyping");
                mail.To.Add(recipientEmail);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = template; // Set your HTML template as the email body

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("pingvocuc555@gmail.com", "bkxflmlmnyxxrzrz");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
        public ActionResult LogoutEdu()
        {
                FormsAuthentication.SignOut();
                Session["User"] = null;
                return RedirectToAction("CheckTyping", "Home");
        }

        [HttpPost]
        public JsonResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session["User"] = null;
                return Json(new { code = 200, msg = "Đăng Xuất thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult ChangeInfo(string name,string introduce)
        {
            try
            {
                USER us = (USER)Session["User"];
                if(us!= null)
                {
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(introduce))
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var user = db.USERs.FirstOrDefault(u => u.Id == us.Id);
                        user.HoTen = name;
                        user.GioiThieu = introduce;
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Lưu thông tin thành công" });
                    }
                }
                else
                {
                    return Json(new { code = 400, msg = "user null" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult ChangePassInfo(string oldpass, string newpass)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (string.IsNullOrEmpty(newpass) || string.IsNullOrEmpty(oldpass))
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var user = db.USERs.FirstOrDefault(u => u.Id == us.Id);
                        if(!BCrypt.Net.BCrypt.Verify(oldpass, user.MatKhau))
                        {
                            return Json(new { code = 300, msg = "Mật khẩu cũ không đúng!" });
                        }
                        else
                        {
                            user.MatKhau = BCrypt.Net.BCrypt.HashPassword(newpass);
                            db.SubmitChanges();
                            return Json(new { code = 200, msg = "Lưu thông tin thành công" });
                        }
                    }
                }
                else
                {
                    return Json(new { code = 400, msg = "user null" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SaveAvatar(string url)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (string.IsNullOrEmpty(url))
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var user = db.USERs.FirstOrDefault(u => u.Id == us.Id);
                        user.Avatar = url;
                        db.SubmitChanges();
                        Session["User"] = user;
                        return Json(new { code = 200, msg = "Lưu ảnh thành công" });
                    }
                }
                else
                {
                    return Json(new { code = 400, msg = "user null" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteTypingResult(int[] data)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (data.Length == 0)
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var getprogessuser = db.USERPROGESSes.FirstOrDefault(u => u.UserID == us.Id);
                        for(int i = 0;i<data.Length;i++)
                        {
                            var result = db.TYPINGRESULTs.FirstOrDefault(r => r.ResultId == data[i]);
                            if(result != null)
                            {
                                getprogessuser.SoBaiKiemTra -= 1;
                                getprogessuser.SoTuDaGo -= result.TotalWords ?? 0;
                                db.TYPINGRESULTs.DeleteOnSubmit(result);
                            }    
                        }
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Xóa thành công" });
                    }
                }
                else
                {
                    return Json(new { code = 400, msg = "Vui lòng đăng nhập" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
    }
}