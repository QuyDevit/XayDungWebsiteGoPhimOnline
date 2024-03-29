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
        public static string ConvertTimestampToDateTest(long timestamp)
        {
            long timestampInSeconds = timestamp / 1000;

            if (timestampInSeconds < -62135596800 || timestampInSeconds > 253402300799)
            {
                throw new ArgumentOutOfRangeException(nameof(timestamp), "Timestamp is out of range.");
            }

            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestampInSeconds);
            var dateTime = dateTimeOffset.LocalDateTime; // Hoặc .UtcDateTime nếu bạn muốn thời gian UTC

            // Định dạng ngày tháng theo yêu cầu: ngày/tháng/năm giờ:phút
            string formattedDate = dateTime.ToString("dd/MM/yyyy HH:mm");

            return formattedDate;
        }
        public static string ConvertTimestampToDateTestEvent(long timestamp)
        {
            long timestampInSeconds = timestamp / 1000;

            if (timestampInSeconds < -62135596800 || timestampInSeconds > 253402300799)
            {
                throw new ArgumentOutOfRangeException(nameof(timestamp), "Timestamp is out of range.");
            }

            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestampInSeconds);
            var dateTime = dateTimeOffset.LocalDateTime; // Hoặc .UtcDateTime nếu bạn muốn thời gian UTC

            // Định dạng ngày tháng theo yêu cầu mới: "HH:mm Thứ x ngày dd tháng MM năm yyyy"
            // Lưu ý: Định dạng này phụ thuộc vào cài đặt ngôn ngữ và vùng lãnh thổ của hệ thống
            // Bạn có thể cần chỉnh sửa định dạng để phù hợp với môi trường cụ thể của bạn
            string formattedDate = dateTime.ToString("HH:mm, 'Thứ' dddd 'ngày' dd 'tháng' MM 'năm' yyyy", new System.Globalization.CultureInfo("vi-VN"));

            return formattedDate;
        }
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
                foreach (var item in listClass)
                {
                    var dataUserJoin = JsonConvert.DeserializeObject<List<USERROOM>>(item.ListUserJoin);
                    var checkusernow = dataUserJoin.FirstOrDefault(u => u.UserId == user.Id);
                    if (checkusernow != null)
                    {
                        list.Add(item);
                    }
                }

                var listeventbyuser = db.TESTEDUs.ToList();
                List<TESTEDUTABLE> listevent = new List<TESTEDUTABLE>();
                foreach(var item in listeventbyuser)
                {
                    var data = JsonConvert.DeserializeObject<List<CLASSTEST>>(item.ListClass);
                    foreach(var room in list)
                    {
                        TESTEDUTABLE test = new TESTEDUTABLE();
                        var classroom = db.CLASSROOMs.FirstOrDefault(c => c.ClassRoomId == room.ClassRoomId);
                        var checkclass = data.FirstOrDefault(c => c.IdRoom == room.ClassRoomId);
                        if(checkclass != null)
                        {
                            test.IDRoom = classroom.ClassRoomId;
                            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            if (currentTimestamp > item.DateEnd)
                            {
                                test.Status = false;
                            }
                            else
                            {
                                test.Status = true;
                            }
                            test.TimeStart = ConvertTimestampToDateTest(item.DateStart ?? 0);
                            test.TimeEnd = ConvertTimestampToDateTest(item.DateEnd ?? 0);
                            test.ClassName = classroom.ClassName;
                            test.CodeLink = item.CodeLink;
                            listevent.Add(test);
                        }
                    }
                }
                var view = new DASHBOARDSTU();
                view.ListClass = list;
                view.ListEvent = listevent;

                return View(view);
            }          
        }
        public ActionResult RoomStu(int data)
        {
            USER us = (USER)Session["User"];
            if(us == null)
            {
                return RedirectToAction("DashboardStudent", "DashBoardStudent");
            }
            else
            {
                var getRoom = db.CLASSROOMs.FirstOrDefault(c => c.ClassRoomId == data);
                if (getRoom == null)
                {
                    return RedirectToAction("DashboardStudent", "DashBoardStudent");
                }
                else
                {
                    var datajs = JsonConvert.DeserializeObject<List<USERROOM>>(getRoom.ListUserJoin);
                    var checkusernow = datajs.FirstOrDefault(u => u.UserId == us.Id);
                    if(checkusernow == null)
                    {
                        return RedirectToAction("DashboardStudent", "DashBoardStudent");
                    }
                    else
                    {
                        var getuser = db.USERs.FirstOrDefault(u => u.Id == getRoom.UserCreate);
                    
                        var listeventbyclass = db.TESTEDUs.ToList();
                        List<TESTEDUTABLE> listevent = new List<TESTEDUTABLE>();
                        foreach (var item in listeventbyclass)
                        {
                            var dataclass = JsonConvert.DeserializeObject<List<CLASSTEST>>(item.ListClass);
                            var checkclass = dataclass.FirstOrDefault(c => c.IdRoom == getRoom.ClassRoomId);
                            if(checkclass != null)
                            {
                                TESTEDUTABLE test = new TESTEDUTABLE();
                                test.IDRoom = getRoom.ClassRoomId;
                                long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                if (currentTimestamp > item.DateEnd)
                                {
                                    test.Status = false;
                                }
                                else
                                {
                                    test.Status = true;
                                }
                                test.TimeStart = ConvertTimestampToDateTest(item.DateStart ?? 0);
                                test.TimeEnd = ConvertTimestampToDateTest(item.DateEnd ?? 0);
                                test.ClassName = getRoom.ClassName;
                                test.CodeLink = item.CodeLink;
                                listevent.Add(test);

                            }                               
                        }
                        ROOMSTUVIEW roomstu = new ROOMSTUVIEW();
                        roomstu.Name = getuser.HoTen;
                        roomstu.Email = getuser.Email;
                        roomstu.ClassName = getRoom.ClassName;
                        roomstu.Room = getRoom;
                        roomstu.ListEvent = listevent;
                        return View(roomstu);
                    }               
                }

            }

        }
        public ActionResult TestTypingEduAction()
        {
            return View();
        }
        public ActionResult _PartialSideBarStu()
        {
            return PartialView();
        }
        public ActionResult EventAction(string key,int idroom)
        {
            // Kiểm tra nếu key rỗng hoặc null
            if (string.IsNullOrEmpty(key) )
            {
                // Lấy URL của trang trước đó
                var returnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"); // Giả sử trang chủ là mặc định nếu không có trang trước

                // Chuyển hướng người dùng quay lại trang trước đó
                return Redirect(returnUrl);
            }
            else
            {
                USER us = (USER)Session["User"];
                if (us == null)
                {
                    return RedirectToAction("CheckTyping", "Home");
                }
                else
                {
                    var getevent = db.TESTEDUs.FirstOrDefault(e => e.CodeLink == key);
                    if(getevent == null)
                    {
                        var returnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"); // Giả sử trang chủ là mặc định nếu không có trang trước

                        // Chuyển hướng người dùng quay lại trang trước đó
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        var data = JsonConvert.DeserializeObject<List<CLASSTEST>>(getevent.ListClass);
                        var checkclass = data.FirstOrDefault(c => c.IdRoom == idroom);
                        if(checkclass == null)
                        {
                            var returnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"); // Giả sử trang chủ là mặc định nếu không có trang trước

                            // Chuyển hướng người dùng quay lại trang trước đó
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            var getclassbyevent = db.CLASSROOMs.FirstOrDefault(c => c.ClassRoomId == checkclass.IdRoom);
                            var getlistuserjoinbyclass = JsonConvert.DeserializeObject<List<USERROOM>>(getclassbyevent.ListUserJoin);
                            var checkuser = getlistuserjoinbyclass.FirstOrDefault(u => u.UserId == us.Id);
                            if(checkuser == null && us.Id != getevent.UserCreate)
                            {
                                var returnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index", "Home"); // Giả sử trang chủ là mặc định nếu không có trang trước
                                // Chuyển hướng người dùng quay lại trang trước đó
                                return Redirect(returnUrl);

                            }
                            else
                            {
                                var view = new TESTEDUTABLE();
                                view.ClassName = getclassbyevent.ClassName;
                                view.IDRoom = getclassbyevent.ClassRoomId;
                                view.CodeLink = getevent.CodeLink;
                                long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                if(currentTimestamp > getevent.DateEnd)
                                {
                                    view.Status = false;
                                }
                                else
                                {
                                    view.Status = true;
                                }
                                view.TitleTest = getevent.TitleTest;
                                view.TimeStart = ConvertTimestampToDateTestEvent(getevent.DateStart ?? 0);
                                view.TimeEnd = ConvertTimestampToDateTestEvent(getevent.DateEnd ?? 0);
                                view.ExamDuration = getevent.ExamDuration / 60 + " phút";
                                view.MaxAttempts = getevent.MaxAttempts ?? 0;
                                return View(view);
                            }
                        }
                    }
                }
            }
            
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
                            if(checkuserjoin != null)
                            {
                                return Json(new { code = 400, msg = "Bạn đã tham gia lớp này rồi" });
                            }
                            else
                            {
                                if(checkusernow == null)
                                {
                                    long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                                    dataUserRequest.Add(new USERROOM { UserId = us.Id, UserName = us.HoTen, UserAvatar = us.Avatar, Email = us.Email, DateJoin = currentTimestamp });
                                }
                                room.ListUserRequest = JsonConvert.SerializeObject(dataUserRequest);
                                db.SubmitChanges();
                                return Json(new { code = 200, msg = "Đã gửi yêu cầu thành công" });
                            }                       
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