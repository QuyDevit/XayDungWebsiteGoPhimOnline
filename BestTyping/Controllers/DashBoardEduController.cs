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
        // GET: DashBoardEdu
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
                        room.Description = description;
                        room.AvatarClassRoom = "";
                        room.IsPrivate = isprivate;
                        room.ListUserJoin = "[]";
                        room.CreateDate = createdate;
                        db.CLASSROOMs.InsertOnSubmit(room);
                        db.SubmitChanges();
                        List<USERFILTER> listemail = new List<USERFILTER>();
                        int newRoomID = room.ClassRoomId;
                        var listuser = db.USERs.ToList();
                        foreach(var item in listuser)
                        {
                            USERFILTER u = new USERFILTER();
                            u.name = item.HoTen;
                            u.email = item.Email;
                            listemail.Add(u);
                        }
                        
                        return Json(new { code = 200, msg = "Lấy dữ liệu thành công" , roomID = newRoomID, list = listemail });
                    }
                }               
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "CHấp nhận bỏ" });
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
                                        datajs.Add(new USERROOM { UserId = getUser.Id, Email = getUser.Email, DateJoin = currentTimestamp });
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
                return Json(new { code = 500, msg = "CHấp nhận bỏ" });
            }
        }
    }
}