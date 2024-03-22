using BestTyping.Models;
using BestTyping.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Controllers
{
    public class DashBoardStudentController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: DashBoardStudent

        public ActionResult DashboardStudent()
        {
            USER user = (USER)Session["User"];
            if(user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var listClass = db.CLASSROOMs.ToList();
                var list = new List<CLASSROOM>();
                foreach(var item in listClass)
                {
                    var dataUserJoin = JsonConvert.DeserializeObject<List<USERROOM>>(item.ListUserJoin);
                    var checkusernow = dataUserJoin.FirstOrDefault(u => u.UserId == user.Id);
                    if(checkusernow != null)
                    {
                        list.Add(item);
                    }
                }

                return View(list);
            }          
        }
        public ActionResult _PartialSideBarStu()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult CheckCodeRoom(string data)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (string.IsNullOrEmpty(data))
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var room = db.CLASSROOMs.FirstOrDefault(t => t.JoinCode == data);
                        if (room != null)
                        {
                            int flag = 0;
                            if(room.PassClassRoom == null)
                            {
                                flag = 0;
                            }
                            else
                            {
                                flag = 1;
                            }
                            var dataUserJoin = JsonConvert.DeserializeObject<List<USERROOM>>(room.ListUserJoin);
                            var checkusernow = dataUserJoin.FirstOrDefault(u => u.UserId == us.Id);
                            if(checkusernow == null && room.UserCreate != us.Id)
                            {
                                return Json(new { code = 200, msg = "Thành công", status = room.IsPrivate,pass = flag, id = room.ClassRoomId });
                            }
                            else
                            {
                                return Json(new { code = 400, msg = "Bạn đã tham gia lớp này rồi" });
                            }
                            
                        }
                        else
                        {
                            return Json(new { code = 500, msg = "Mã không hợp lệ!" });
                        }
                    }
                }
                else
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GiveRequestJoin(int data)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (data == 0)
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var room = db.CLASSROOMs.FirstOrDefault(t => t.ClassRoomId == data);
                        if (room != null)
                        {
                            var dataUserJoin = JsonConvert.DeserializeObject<List<USERROOM>>(room.ListUserJoin);
                            var checkuserjoin = dataUserJoin.FirstOrDefault(u => u.UserId == us.Id);
                            var dataUserRequest = JsonConvert.DeserializeObject<List<USERROOM>>(room.ListUserRequest);
                            var checkusernow = dataUserRequest.FirstOrDefault(u => u.UserId == us.Id);
                            if(checkusernow == null && checkuserjoin == null)
                            {
                                long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                dataUserRequest.Add(new USERROOM { UserId = us.Id, UserName = us.HoTen, UserAvatar = us.Avatar, Email = us.Email, DateJoin = currentTimestamp });
                            }
                            room.ListUserRequest = JsonConvert.SerializeObject(dataUserRequest);
                            db.SubmitChanges();
                            return Json(new { code = 200, msg = "Đã gửi yêu cầu thành công" });
                        }
                        else
                        {
                            return Json(new { code = 400, msg = "Phòng không tồn tại" });
                        }
                    }
                }
                else
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult DeleteRequestJoin(int data,int roomid)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (data == 0)
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var room = db.CLASSROOMs.FirstOrDefault(t => t.ClassRoomId == roomid);
                        if (room != null)
                        {
                            if (string.IsNullOrEmpty(room.ListUserRequest))
                            {
                                room.ListUserRequest = "[]";
                            }
                            var dataUserRequest = JsonConvert.DeserializeObject<List<USERROOM>>(room.ListUserRequest);
                            var checkusernow = dataUserRequest.FirstOrDefault(u => u.UserId == data);
                            if (checkusernow != null)
                            {
                                dataUserRequest.Remove(checkusernow);
                            }
                            room.ListUserRequest = JsonConvert.SerializeObject(dataUserRequest);
                            db.SubmitChanges();
                            return Json(new { code = 200, msg = "Hủy thành công" });
                        }
                        else
                        {
                            return Json(new { code = 400, msg = "Phòng không tồn tại" });
                        }
                    }
                }
                else
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult AcceptRequestJoin(int data, int roomid)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (data == 0)
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var room = db.CLASSROOMs.FirstOrDefault(t => t.ClassRoomId == roomid);
                        if (room != null)
                        {
                            if (string.IsNullOrEmpty(room.ListUserRequest))
                            {
                                room.ListUserRequest = "[]";
                            }
                            var dataUserJoin = JsonConvert.DeserializeObject<List<USERROOM>>(room.ListUserJoin);
                            var checkuserjoin = dataUserJoin.FirstOrDefault(u => u.UserId == data);
                            var dataUserRequest = JsonConvert.DeserializeObject<List<USERROOM>>(room.ListUserRequest);
                            var checkusernow = dataUserRequest.FirstOrDefault(u => u.UserId == data);
                            if (checkusernow != null)
                            {
                                dataUserRequest.Remove(checkusernow);
                                if (checkuserjoin == null)
                                {
                                    var user = db.USERs.FirstOrDefault(u => u.Id == data);
                                    long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                    dataUserJoin.Add(new USERROOM { UserId = user.Id, UserName = user.HoTen, UserAvatar = user.Avatar, Email = user.Email, DateJoin = currentTimestamp });
                                }
                            } 
                            room.ListUserRequest = JsonConvert.SerializeObject(dataUserRequest);
                            room.ListUserJoin = JsonConvert.SerializeObject(dataUserJoin);
                            db.SubmitChanges();
                            return Json(new { code = 200, msg = "Đã duyệt thành viên" });
                        }
                        else
                        {
                            return Json(new { code = 400, msg = "Phòng không tồn tại" });
                        }
                    }
                }
                else
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
    }
}