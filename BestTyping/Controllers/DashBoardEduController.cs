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
        public ActionResult DashboardEdu()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var testbyuser = db.TESTEDUs.Where(u => u.UserCreate == user.Id).ToList();
                var textbyuser = db.TEXTTESTEDUs.Where(u => u.UserCreate == user.Id).ToList();
                var groupbyuser = db.CLASSROOMs.Where(u => u.UserCreate == user.Id).ToList();
                var view = new DASHBOARDEDU();
                view.SumTest = testbyuser.Count();
                view.SumText = textbyuser.Count();
                view.SumGroup = groupbyuser.Count();
                return View(view);
            }
        }
        public ActionResult TestEdu()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var view = new TESTEDUVIEW();
                var classrooombyuser = db.CLASSROOMs.Where(u => u.UserCreate == user.Id).ToList();
                var textedubyuser = db.TEXTTESTEDUs.Where(u => u.UserCreate == user.Id).ToList();
                List<CLASSROOMVIEW> list = new List<CLASSROOMVIEW>();
                foreach (var item in classrooombyuser)
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
                view.ListRoom = list;
                view.ListTextEdu = textedubyuser;
                return View(view);
            }
        }
        [HttpPost]
        public JsonResult CreateTestEdu(int textid,bool random,string titletest,int[] arrlist, long timestart,long timeend,int examduration,string pass,int maxattempts,long createdate)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (textid <= 0 || (random != true && random!= false) || arrlist.Length == 0 || timestart <= 0 || timeend <= 0 || examduration <= 0 || string.IsNullOrEmpty(pass) || maxattempts <= 0 || string.IsNullOrEmpty(titletest))
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        TESTEDU test = new TESTEDU();
                        test.UserCreate = us.Id;
                        test.TextID = textid;
                        test.TitleTest = titletest;
                        test.IsRandom = random;
                        test.DateStart = timestart;
                        test.DateEnd = timeend;
                        test.ExamDuration = examduration;
                        test.CodeLink = GenerateCodeTest();
                        test.PassTest = pass;
                        test.Status = true;
                        test.MaxAttempts = maxattempts;
                        test.CreateDate = createdate;
                        test.ListClass = JsonConvert.SerializeObject(arrlist.Select(id => new CLASSTEST { IdRoom = id, ClassName = db.CLASSROOMs.FirstOrDefault(c => c.ClassRoomId == id)?.ClassName }));
                        db.TESTEDUs.InsertOnSubmit(test);
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Tạo thành công" });
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
        public ActionResult MyTestEdu()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var listtestbyuser = db.TESTEDUs.Where(u => u.UserCreate == user.Id).OrderByDescending(u => u.ID).ToList();
                var list = new List<TESTEDUTABLE>();
                foreach(var item in listtestbyuser)
                {
                    var dataroom = JsonConvert.DeserializeObject<List<CLASSTEST>>(item.ListClass);
                    var firstItem = dataroom.FirstOrDefault();
                    long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    var eventtest = db.TESTEDUs.FirstOrDefault(t => t.ID == item.ID);
                    if (currentTimestamp > item.DateEnd)
                    {
                        eventtest.Status = false;
                    }
                    db.SubmitChanges();
                    var i = new TESTEDUTABLE();
                    i.IDRoom = firstItem.IdRoom;
                    i.IDTest = item.ID;
                    i.Status = item.Status ?? true;
                    i.CodeLink = item.CodeLink;
                    i.TitleTest = item.TitleTest;
                    i.TimeStart = ConvertTimestampToDateTest(item.DateStart ?? 0);
                    i.TimeEnd = ConvertTimestampToDateTest(item.DateEnd ?? 0);
                    list.Add(i);
              
                }
                return View(list);
            }
        }

        public ActionResult ListMyTestEdu()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var listtestbyuser = db.TESTEDUs.Where(u => u.UserCreate == user.Id).OrderByDescending(u => u.CreateDate).ToList();
                var list = new List<TESTEDUTABLE>();
                foreach (var item in listtestbyuser)
                {
                    var dataroom = JsonConvert.DeserializeObject<List<CLASSTEST>>(item.ListClass);
                    var firstItem = dataroom.FirstOrDefault();
                    long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    var eventtest = db.TESTEDUs.FirstOrDefault(t => t.ID == item.ID);
                    if (currentTimestamp > item.DateEnd)
                    {
                        eventtest.Status = false;
                    }
                    db.SubmitChanges();
                    var i = new TESTEDUTABLE();
                    i.IDRoom = firstItem.IdRoom;
                    i.IDTest = item.ID;
                    i.Status = item.Status ?? true;
                    i.CodeLink = item.CodeLink;
                    i.TitleTest = item.TitleTest;
                    i.TimeStart = ConvertTimestampToDateTest(item.DateStart ?? 0);
                    i.TimeEnd = ConvertTimestampToDateTest(item.DateEnd ?? 0);
                    list.Add(i);

                }
                return View(list);
            }
        }
        [HttpPost]
        public JsonResult GetListClassByTest(int data)
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
                    var getTest = db.TESTEDUs.FirstOrDefault(t => t.ID == data);
                    var listclassbyfirsttest = JsonConvert.DeserializeObject<List<CLASSTEST>>(getTest.ListClass);

                    return Json(new { code = 200,list = listclassbyfirsttest, msg = "Lấy dữ liệu thành công" });
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }
        public ActionResult ExamListEdu(int? key = null, int? data = null)
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                if (key == null && data == null)
                {
                    var gettest = db.TESTEDUs.Where(t => t.UserCreate == user.Id).OrderByDescending(t => t.ID).ToList();
                    var firsttest = gettest.FirstOrDefault();
                    if (firsttest != null)
                    {
                        var listclassbyfirsttest = JsonConvert.DeserializeObject<List<CLASSTEST>>(firsttest.ListClass);

                        var listuserexam = new List<USEREXAMEDU>();
                        var getfirstclassbytest = db.CLASSROOMs.FirstOrDefault(c => c.ClassRoomId == listclassbyfirsttest.FirstOrDefault().IdRoom);

                        var listuserclass = JsonConvert.DeserializeObject<List<USERROOM>>(getfirstclassbytest.ListUserJoin);
                        foreach (var item in listuserclass)
                        {
                            var us = new USEREXAMEDU();
                            us.UserId = item.UserId;
                            us.Email = item.Email;
                            us.Avatar = item.UserAvatar;
                            us.UserName = item.UserName;
                            us.TitleTest = firsttest.TitleTest;
                            us.ClassName = getfirstclassbytest.ClassName;
                            var getlistresult = db.TYPINGRESULTEDUs.Where(r => r.UserID == item.UserId && r.TestId == firsttest.ID).ToList();
                            us.UserTestAttempts = getlistresult.Count();
                            us.MaxTestAttempts = firsttest.MaxAttempts ?? 0;
                            listuserexam.Add(us);
                        }
                        var view = new LISTEXAMEDU();
                        view.ListClassByTest = listclassbyfirsttest;
                        view.TestIDChoose = firsttest.ID;
                        view.RoomIdChoose = listclassbyfirsttest.FirstOrDefault().IdRoom;
                        view.ListUserExam = listuserexam;
                        view.ListTest = gettest;
                        return View(view);
                    }
                    else
                    {
                        var emptyModel = new LISTEXAMEDU();
                        return View(emptyModel);
                    }
                }
                else
                {
                    var gettest = db.TESTEDUs.Where(t => t.UserCreate == user.Id).OrderByDescending(t => t.ID).ToList();
                    var gettestbykey = db.TESTEDUs.FirstOrDefault(t => t.ID == key);
                    var listclassbyfirsttest = JsonConvert.DeserializeObject<List<CLASSTEST>>(gettestbykey.ListClass);

                    var listuserexam = new List<USEREXAMEDU>();
                    CLASSROOM getclassbydata = null;
                    if (data != null)
                    {
                        getclassbydata = db.CLASSROOMs.FirstOrDefault(c => c.ClassRoomId == data);
                    }
                    else
                    {
                        getclassbydata = db.CLASSROOMs.FirstOrDefault(c => c.ClassRoomId == listclassbyfirsttest.FirstOrDefault().IdRoom);
                    }
                        
                    var listuserclass = JsonConvert.DeserializeObject<List<USERROOM>>(getclassbydata.ListUserJoin);
                    foreach (var item in listuserclass)
                    {
                        var us = new USEREXAMEDU();
                        us.UserId = item.UserId;
                        us.Email = item.Email;
                        us.Avatar = item.UserAvatar;
                        us.UserName = item.UserName;
                        us.TitleTest = gettestbykey.TitleTest;
                        us.ClassName = getclassbydata.ClassName;
                        var getlistresult = db.TYPINGRESULTEDUs.Where(r => r.UserID == item.UserId && r.TestId == key).ToList();
                        us.UserTestAttempts = getlistresult.Count();
                        us.MaxTestAttempts = gettestbykey.MaxAttempts ?? 0;
                        listuserexam.Add(us);
                    }
                    var view = new LISTEXAMEDU();
                    view.ListClassByTest = listclassbyfirsttest;
                    view.TestIDChoose = gettestbykey.ID;
                    view.RoomIdChoose = data ?? 0;
                    view.ListUserExam = listuserexam;
                    view.ListTest = gettest;
                    return View(view);
                }
            }
        }

        public ActionResult TextTestEdu()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var getLanguage = db.EXERCISELANGUAGEs.ToList();
                return View(getLanguage);
            }
        }
        public ActionResult EditTextTestEdu(int id)
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var gettext = db.TEXTTESTEDUs.FirstOrDefault(t=>t.ID == id && t.UserCreate == user.Id);
                if(gettext == null)
                {
                    return RedirectToAction("MyTextTestEdu", "DashBoardEdu");
                }
                else
                {
                    var getLanguage = db.EXERCISELANGUAGEs.ToList();
                    var getLanguagebyText = db.EXERCISELANGUAGEs.FirstOrDefault(t => t.LanguageID == gettext.LanguageID);
                    EDITTEXTPRACTICEVIEW edittext = new EDITTEXTPRACTICEVIEW();
                    edittext.ID = gettext.ID;
                    edittext.IsPrivate = gettext.IsPrivate ?? false;
                    edittext.Text = gettext.Text;
                    edittext.Language = getLanguagebyText.LanguageName;
                    edittext.Title = gettext.Title;
                    edittext.ListLanguage = getLanguage;
                    return View(edittext);
                }
            }
        }
        [HttpPost]
        public JsonResult EditTextEduApi(int data, string title, string language, string content, bool isprivate)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 400, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(content) || (isprivate != true && isprivate != false))
                    {
                        return Json(new { code = 400, msg = "Vui lòng điền đầy đủ thông tin" });
                    }
                    else
                    {
                        var getlanguage = db.EXERCISELANGUAGEs.FirstOrDefault(l => l.LanguageName.Equals(language));
                        var getTextEdu = db.TEXTTESTEDUs.FirstOrDefault(t => t.ID == data && t.UserCreate == user.Id);
                        if (getTextEdu == null)
                        {
                            return Json(new { code = 500, msg = "Không tồn tại văn bản này" });
                        }
                        else
                        {
                            getTextEdu.LanguageID = getlanguage.LanguageID;
                            getTextEdu.IsPrivate = isprivate;
                            getTextEdu.Text = content;
                            getTextEdu.Title = title;
                            db.SubmitChanges();
                            return Json(new { code = 200, msg = "Lưu văn bản thành công" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult DeleteTextEdu(int data)
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
                        var text = db.TEXTTESTEDUs.FirstOrDefault(t => t.ID == data);
                        if (text != null)
                        {
                            db.TEXTTESTEDUs.DeleteOnSubmit(text);
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
        [HttpPost]
        public JsonResult CreateTextTestEdu(string title, string language, string content, bool isprivate, long createat)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 400, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(content) || (isprivate != true && isprivate != false) || createat <= 0)
                    {
                        return Json(new { code = 400, msg = "Vui lòng điền đầy đủ thông tin" });

                    }
                    else
                    {
                        var getlanguage = db.EXERCISELANGUAGEs.FirstOrDefault(l => l.LanguageName.Equals(language));
                        TEXTTESTEDU text = new TEXTTESTEDU();
                        text.UserCreate = user.Id;
                        text.Title = title;
                        text.CreateDate = createat;
                        text.Text = content;
                        text.LanguageID = getlanguage.LanguageID;
                        text.IsPrivate = isprivate;
                        db.TEXTTESTEDUs.InsertOnSubmit(text);
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Tạo văn bản thành công" });
                    }

                }

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }

        }
        public ActionResult MyTextTestEdu()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("CheckTyping", "Home");
            }
            else
            {
                var mytext = db.TEXTTESTEDUs.Where(t => t.UserCreate == user.Id).OrderByDescending(t => t.ID);
                var list = new List<TEXTEDUVIEW>();
                foreach(var item in mytext)
                {
                    var text = new TEXTEDUVIEW();
                    text.ID = item.ID;
                    text.Text = item.Text;
                    text.Title = item.Title;
                    text.CreateDate = ConvertTimestampToDate(item.CreateDate ?? 0);
                    text.Status = item.IsPrivate ?? true;
                    list.Add(text);
                }

                return View(list);
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
        [HttpPost]
        public JsonResult CreateCodeRoom(int data)
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
                        var code = GenerateCode();
                        getroom.JoinCode = code;
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Tạo mã nhóm thành công" ,data = code});
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }
        [HttpPost]
        public JsonResult DeleteCodeRoom(int data)
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
                        getroom.JoinCode = null;
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Xóa mã nhóm thành công"});
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }

        [HttpPost]
        public JsonResult SavePassRoom(int data,string pass,int check)
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
                        if(check == 1)
                        {
                            getroom.PassClassRoom = pass;
                        }
                        else
                        {
                            getroom.PassClassRoom = null;
                        }
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Lưu thành công"});
                    }
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }
        private string GenerateCodeTest()
        {
            var random = new Random();
            var code = "";
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var length = 25;

            do
            {
                code = new string(Enumerable.Repeat(characters, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (db.TESTEDUs.Any(c => c.CodeLink == code)); // Đảm bảo mã là duy nhất trong DB

            return code;
        }
        private string GenerateCode()
        {
            var random = new Random();
            var code = "";
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var length = 7;

            do
            {
                code = new string(Enumerable.Repeat(characters, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (db.CLASSROOMs.Any(c => c.JoinCode == code)); // Đảm bảo mã là duy nhất trong DB

            return code;
        }
    }
}