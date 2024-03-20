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
    public class DashBoardEduController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        public static string ConvertTimestampToDate(long timestamp)
        {

            long timestampInSeconds = timestamp / 1000;

  
            if (timestampInSeconds < -62135596800 || timestampInSeconds > 253402300799)
            {
                throw new ArgumentOutOfRangeException(nameof(timestamp), "Timestamp is out of range.");
            }

            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestampInSeconds);
            var dateTime = dateTimeOffset.LocalDateTime; // Hoặc .UtcDateTime nếu bạn muốn thời gian UTC

            // Định dạng ngày tháng theo yêu cầu: ngày/tháng/năm
            string formattedDate = dateTime.ToString("dd/MM/yyyy");

            return formattedDate;
        }
        public ActionResult DashboardEdu()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                return View();
            }
        }

        public ActionResult SettingClassRoom(int id)
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var getRoom = db.CLASSROOMs.FirstOrDefault(r => r.UserCreate == user.Id && r.ClassRoomId == id);
                if(getRoom == null)
                {
                    return RedirectToAction("CheckTyping", "Home");
                }
                else
                {
                    ROOMSETTINGVIEW room = new ROOMSETTINGVIEW();
                    room.RoomId = getRoom.ClassRoomId;
                    room.AvatarClassRoom = getRoom.AvatarClassRoom;
                    room.ClassName = getRoom.ClassName;
                    room.Description = getRoom.Description;
                    room.IsPrivate = getRoom.IsPrivate ?? true;
                    room.JoinCode = getRoom.JoinCode;
                    room.PassClassRoom = getRoom.PassClassRoom;
                    var dataUserJoin = JsonConvert.DeserializeObject<List<USERROOM>>(getRoom.ListUserJoin);
                    var dataUserRequest = JsonConvert.DeserializeObject<List<USERROOM>>(getRoom.ListUserRequest);
                    room.ListMember = dataUserJoin;
                    room.ListUserRequest = dataUserRequest;

                    var listemail = new List<USERFILTER>();
                    var listuser = db.USERs.ToList();
                    foreach (var item in listuser)
                    {
                        var checkusernow = dataUserJoin.FirstOrDefault(u => u.UserId == item.Id);
                        if (item.Id != user.Id && checkusernow == null)
                        {
                            USERFILTER u = new USERFILTER();
                            u.name = item.HoTen;
                            u.email = item.Email;
                            listemail.Add(u);
                        }
                    }
                    
                    room.ListUserFilter = JsonConvert.SerializeObject(listemail);

                    return View(room);
                }
            }
        }
        public ActionResult ManageGroupEdu()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var listClassByUser = db.CLASSROOMs.Where(c => c.UserCreate == user.Id).OrderByDescending(c=>c.ClassRoomId).ToList();
                List<CLASSROOMVIEW> list = new List<CLASSROOMVIEW>();
                foreach(var item in listClassByUser)
                {

                    var data = JsonConvert.DeserializeObject<List<USERROOM>>(item.ListUserJoin);
                    CLASSROOMVIEW room = new CLASSROOMVIEW();
                    room.RoomId = item.ClassRoomId;
                    room.ClassName = item.ClassName;
                    room.CreateDate = ConvertTimestampToDate(item.CreateDate ?? 0);
                    room.Status = item.IsPrivate ?? true;
                    room.JoinCode = item.JoinCode;
                    room.SumPeopleJoin = data.Count();
                    list.Add(room);
                }
                return View(list);
            }
        }

        [HttpPost]
        public JsonResult CreateClassRoom(string name, string description, bool isprivate, long createdate)
        {
            try
            {
                USER user = (USER) Session["User"];
                if(user == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    if (string.IsNullOrEmpty(name) || (isprivate != true && isprivate != false))
                    {
                        return Json(new { code = 400, msg = "Vui lòng điền thông tin đầy đủ" });
                    }
                    else
                    {
                        CLASSROOM room = new CLASSROOM();
                        room.UserCreate = user.Id;
                        room.ClassName = name;
                        room.AvatarClassRoom = "https://firebasestorage.googleapis.com/v0/b/login-besttyping.appspot.com/o/Group%2Fdefault-group.jpg?alt=media&token=36fc580f-e759-440e-b6f9-7c23f169df09";
                        room.Description = description;
                        room.IsPrivate = isprivate;
                        room.ListUserJoin = "[]";
                        room.ListUserRequest = "[]";
                        room.CreateDate = createdate;
                        db.CLASSROOMs.InsertOnSubmit(room);
                        db.SubmitChanges();
                        List<USERFILTER> listemail = new List<USERFILTER>();
                        int newRoomID = room.ClassRoomId;
                        var listuser = db.USERs.ToList();
                        foreach(var item in listuser)
                        {
                            if(item.Id != user.Id)
                            {
                                USERFILTER u = new USERFILTER();
                                u.name = item.HoTen;
                                u.email = item.Email;
                                listemail.Add(u);
                            }
                        }
                        
                        return Json(new { code = 200, msg = "Lấy dữ liệu thành công" , roomID = newRoomID, list = listemail });
                    }
                }               
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }
        [HttpPost]
        public JsonResult AddUserClassRoom(string[] data,int roomid)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    if (data.Length == 0)
                    {
                        return Json(new { code = 400, msg = "Vui lòng điền thông tin đầy đủ" });
                    }
                    else
                    {
                        var getRoom = db.CLASSROOMs.FirstOrDefault(r => r.ClassRoomId == roomid);
                        if(getRoom == null)
                        {
                            return Json(new { code = 500, msg = "Phòng không tồn tại" });
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(getRoom.ListUserJoin ))
                            {
                                getRoom.ListUserJoin = "[]";
                            }
                            var datajs = JsonConvert.DeserializeObject<List<USERROOM>>(getRoom.ListUserJoin);
                            for (int i = 0;i<data.Length; i++)
                            {
                                var getUser = db.USERs.FirstOrDefault(u => u.Email == data[i]);
                                if(getUser != null)
                                {
                                    var existingUser = datajs.FirstOrDefault(l => l.UserId == getUser.Id);
                                    if(existingUser == null)
                                    {
                                        long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                        datajs.Add(new USERROOM { UserId = getUser.Id,UserName = getUser.HoTen,UserAvatar = getUser.Avatar, Email = getUser.Email, DateJoin = currentTimestamp });
                                    }
                                }
                            }
                            getRoom.ListUserJoin = JsonConvert.SerializeObject(datajs);
                            db.SubmitChanges();
                            return Json(new { code = 200, msg = "Thêm thành công" });
                        }                   
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }

        [HttpPost]
        public JsonResult DeleteClassRoom(int data)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    var getroom = db.CLASSROOMs.FirstOrDefault(r => r.ClassRoomId == data && r.UserCreate == user.Id);
                    if(getroom == null)
                    {
                        return Json(new { code = 500, msg = "không hợp lệ" });
                    }
                    else
                    {
                        db.CLASSROOMs.DeleteOnSubmit(getroom);
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Xóa dữ liệu thành công" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }

        [HttpPost]
        public JsonResult SaveAvatarGroup(int data,string url)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    var getroom = db.CLASSROOMs.FirstOrDefault(r => r.ClassRoomId == data && r.UserCreate == user.Id);
                    if (getroom == null)
                    {
                        return Json(new { code = 500, msg = "không hợp lệ" });
                    }
                    else
                    {
                        getroom.AvatarClassRoom = url;
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Lưu dữ liệu thành công" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }

        [HttpPost]
        public JsonResult SaveDescriptionGroup(int data, string description)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    var getroom = db.CLASSROOMs.FirstOrDefault(r => r.ClassRoomId == data && r.UserCreate == user.Id);
                    if (getroom == null)
                    {
                        return Json(new { code = 500, msg = "không hợp lệ" });
                    }
                    else
                    {
                        getroom.Description = description;
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Lưu dữ liệu thành công" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }
        [HttpPost]
        public JsonResult ChangeStatusRoom(int data, bool status)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    var getroom = db.CLASSROOMs.FirstOrDefault(r => r.ClassRoomId == data && r.UserCreate == user.Id);
                    if (getroom == null)
                    {
                        return Json(new { code = 500, msg = "không hợp lệ" });
                    }
                    else
                    {
                        getroom.IsPrivate = status;
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Lưu dữ liệu thành công" });
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }
    }
}