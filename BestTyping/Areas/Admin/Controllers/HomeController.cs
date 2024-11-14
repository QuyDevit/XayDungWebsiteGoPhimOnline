using BestTyping.Models;
using BestTyping.Models.DTO;
using BestTyping.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var data = new DASHBOARDVIEW();
            data.SumUser = db.USERs.Count(x => x.Id != 6);
            data.SumExerciseText = db.EXERCISETEXTs.Count();
            data.SumTextPractice = db.TEXTPRACTICEs.Count();

            var classrooms = db.CLASSROOMs.Select(n => new {
                n.ClassName,
                n.AvatarClassRoom,
                n.IsPrivate,
                n.ListUserJoin,
                n.CreateDate
            }).ToList();

            data.ListClass = classrooms.Select(n => new CLASSDATA {
                ClassName = n.ClassName,
                AvatarClassRoom =n.AvatarClassRoom,
                IsPrivate=n.IsPrivate ?? true,
                SumMember = JsonConvert.DeserializeObject<List<USERROOM>>(n.ListUserJoin).Count,
                CreateDate= n.CreateDate?? 0
            }).ToList();

            var results = (from r in db.TYPINGRESULTs
                           join u in db.USERs on r.UserID equals u.Id
                           select new
                           {
                               u.Avatar,
                               u.HoTen,
                               r.KeyStrokes,
                               r.WPM,
                               r.Timestamp
                           }).ToList();

            data.ListResult = results.Select(r => new RANKTABLE
            {
                Avatar = r.Avatar,
                Name = r.HoTen,
                Keystrokes = r.KeyStrokes ?? 0,
                Wpm = r.WPM ?? 0,
                TimeLastResult = r.Timestamp.HasValue ? ConvertToTimeAgo(r.Timestamp.Value) : string.Empty
            }).ToList();


            return View(data);
        }
        public static string ConvertToTimeAgo(long timestamp)
        {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var timeSpan = currentTime - timestamp;

            if (timestamp == 0)
                return "vừa xong";
            if (timeSpan < 60000) // 60,000 milliseconds = 1 phút
                return "vừa xong";
            if (timeSpan < 3600000) // 3,600,000 milliseconds = 1 giờ
                return $"{timeSpan / 60000} phút trước";
            if (timeSpan < 86400000) // 86,400,000 milliseconds = 1 ngày
                return $"{timeSpan / 3600000} giờ trước";
            if (timeSpan < 2592000000) // 2,592,000,000 milliseconds = 30 ngày
                return $"{timeSpan / 86400000} ngày trước";
            if (timeSpan < 31536000000) // 31,536,000,000 milliseconds = 365 ngày
                return $"{timeSpan / 2592000000} tháng trước";
            return $"{timeSpan / 31536000000} năm trước";
        }
    }
}