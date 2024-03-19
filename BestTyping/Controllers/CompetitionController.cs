using BestTyping.Models;
using BestTyping.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Controllers
{
    public class CompetitionController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
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
        // GET: Competition
        public ActionResult Competitions(string codejoin)
        {
            var getcompetition = db.COMPETITIONs.FirstOrDefault(c => c.JoinCode == codejoin);
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // Sử dụng DateTimeOffset để tránh vấn đề múi giờ và chuyển đổi sang Unix milliseconds
            var twentyFourHoursAgo = currentTime - (24 * 60 * 60 * 1000); // Trừ đi 24 giờ tính bằng milliseconds
            if (getcompetition != null)
            {
                if (getcompetition.CreateDate - twentyFourHoursAgo <= 0)
                {
                    getcompetition.isOpen = false;
                    db.SubmitChanges();
                    return RedirectToAction("CompetitionsTyping", "Home");
                }
                else
                {
                    var listResultCompetition = db.TYPINGRESULTs
                             .Where(t => t.JoinCode == codejoin)
                             .GroupBy(t => t.UserID)
                             .Select(g => new
                             {
                                 UserID = g.Key,
                                 MaxWPM = g.Max(tr => tr.WPM),
                                 Timestamp = g.First(tr => tr.WPM == g.Max(t => t.WPM)).Timestamp
                             })
                             .OrderByDescending(result => result.MaxWPM)
                             .ToList();
                    List<TABLERESULTCOMPETITION> list = new List<TABLERESULTCOMPETITION>();
                    foreach (var item in listResultCompetition)
                    {
                        var getuser = db.USERs.FirstOrDefault(u => u.Id == item.UserID);
                        if (getuser != null)
                        {
                            TABLERESULTCOMPETITION result = new TABLERESULTCOMPETITION();
                            result.Name = getuser.HoTen;
                            result.Avatar = getuser.Avatar;
                            result.WPM = item.MaxWPM ?? 0;
                            result.TimeLastResult = ConvertToTimeAgo(item.Timestamp ?? 0);
                            list.Add(result);
                        }
                    }
                    RESULTCOMPETITIONVIEWMODEL resultview = new RESULTCOMPETITIONVIEWMODEL();
                    resultview.Competition = getcompetition;
                    resultview.ListResultCompetition = list;

                    return View(resultview);
                }
            }
            else
            {
                return RedirectToAction("CompetitionsTyping", "Home");
            }
        }
        [HttpPost]
        public JsonResult CloseCompetition(string codejoin)
        {
            try
            {
                var competition = db.COMPETITIONs.FirstOrDefault(c => c.JoinCode == codejoin);

                competition.isOpen = false;
                db.SubmitChanges();

                return Json(new { code = 200, msg = "Khóa cuộc thi thành công"});
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult HandleSaveResult(int sumword, float accuracy, int exercise, int wpm, int mistakes, int correctwords, long timestamp, int totalcharacters,string joincode)
       {
            try
            {
                USER getUser = (USER)Session["User"];
                
                if (getUser == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để lưu kết quả" });
                }
                var checkUserprogess = db.USERPROGESSes.FirstOrDefault(u => u.UserID == getUser.Id);
                int? resultcompetition = db.TYPINGRESULTs.Where(l => l.JoinCode == joincode && l.UserID == getUser.Id).Max(r => (int?)r.WPM);
                if (checkUserprogess == null)
                {
                    CreateUserProgress(getUser, sumword, wpm);
                }
                else
                {
                    if (resultcompetition.HasValue)
                    {
                        if(wpm <= resultcompetition.Value - 5)
                        { 
                            return Json(new { code = 400, msg = "Vui lòng cải thiện tốc độ để lưu kết quả" }); 
                        }    
                    }
                    UpdateUserProgress(checkUserprogess, sumword, wpm,joincode);
                    UpdateCompetition(joincode, getUser.Id);
                }
                SaveTypingResult(getUser.Id, exercise, wpm, mistakes, accuracy, sumword, timestamp, correctwords, totalcharacters,joincode);

                db.SubmitChanges();

                return Json(new { code = 200, msg = "Lưu thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        private void CreateUserProgress(USER getUser, int sumword, int wpm)
        {
            USERPROGESS userprogess = new USERPROGESS
            {
                UserID = getUser.Id,
                SoTuDaGo = sumword,
                SoBaiKiemTra = 1,
                CuocThiThamGia = 1,
                WPMTotNhat = wpm
            };

            db.USERPROGESSes.InsertOnSubmit(userprogess);
        }

        private void UpdateUserProgress(USERPROGESS checkUserprogess, int sumword, int wpm,string joincode)
        {
            var getUserbyCompetition = db.TYPINGRESULTs.FirstOrDefault(t => t.JoinCode == joincode);

            if (checkUserprogess.WPMTotNhat < wpm)
            {
                checkUserprogess.WPMTotNhat = wpm;
                checkUserprogess.SoBaiKiemTra += 1;
                checkUserprogess.SoTuDaGo += sumword;
                if (getUserbyCompetition == null)
                {
                    checkUserprogess.CuocThiThamGia += 1;
                }
            }
            else
            {
                if (getUserbyCompetition == null)
                {
                    checkUserprogess.CuocThiThamGia += 1;
                }
                checkUserprogess.SoBaiKiemTra += 1;
                checkUserprogess.SoTuDaGo += sumword;
            }
        }
        private void UpdateCompetition(string joincode,int userid)
        {
            var competition = db.COMPETITIONs.FirstOrDefault(c => c.JoinCode == joincode);
            var checkUserCompetition = db.TYPINGRESULTs.FirstOrDefault(c => c.JoinCode == joincode && c.UserID == userid);
            if(checkUserCompetition == null)
            {
                competition.PeopleJoin += 1;
            }           
            competition.NumberOfTestsPerformed += 1;
        }

        private void SaveTypingResult(int userId, int exercise, int wpm, int mistakes, float accuracy, int sumword, long timestamp, int correctwords, int totalcharacters,string joincode)
        {
            accuracy = (float)Math.Round(accuracy, 2);
            TYPINGRESULT typingresult = new TYPINGRESULT
            {
                UserID = userId,
                ExerciseTextID = exercise,
                WPM = wpm,
                Mistakes = mistakes,
                Accuracy = accuracy,
                TotalWords = sumword,
                Timestamp = timestamp,
                CorrectWords = correctwords,
                KeyStrokes = totalcharacters,
                JoinCode = joincode
            };

            db.TYPINGRESULTs.InsertOnSubmit(typingresult);
        }
    }
}