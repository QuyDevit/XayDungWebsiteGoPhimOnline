using BestTyping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Controllers
{
    public class CompetitionsTypingController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: CompetitionsTyping

        [HttpPost]
        public JsonResult HandleCreateCompetition(bool isPrivate ,string language,long createdate, int exerciseid)
        {
            try
            {
                USER getUser = (USER)Session["User"];
                
                if (getUser == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để tạo cuộc thi" });
                }
                else
                {
                    var getLanguage = db.EXERCISELANGUAGEs.FirstOrDefault(l => l.LanguageName==language);
                    int languageId = getLanguage.LanguageID;
                    var getExerciseTexts = db.EXERCISETEXTs.Where(t => t.LanguageID == languageId && t.ExerciseID == exerciseid).ToList();                  
                    if (getExerciseTexts.Count() == 0)
                    {
                        return Json(new { code = 400, msg = "Ngôn ngữ này hiện chưa có" });
                    }
                    else
                    {
                        // Chọn ngẫu nhiên một mục từ danh sách các mục đã lấy được
                        var random = new Random();
                        // Chọn ngẫu nhiên một mục từ danh sách các mục đã lấy được

                        var randomExerciseText = getExerciseTexts[random.Next(getExerciseTexts.Count)];
                        int ExerciseTextID = randomExerciseText.ExerciseTextID;
                        string joincode = GenerateLinkCode();
                        COMPETITION competition = new COMPETITION();
                        competition.UserCreate = getUser.Id;
                        competition.IsPrivate = isPrivate;
                        competition.PeopleJoin = 0;
                        competition.NumberOfTestsPerformed = 0;
                        competition.CreateDate = createdate;
                        competition.LanguageId = languageId;
                        competition.ExerciseTextID = ExerciseTextID;
                        competition.JoinCode = joincode;
                        competition.isOpen = true;
                        db.COMPETITIONs.InsertOnSubmit(competition);
                        db.SubmitChanges();
                        return Json(new { code = 200, data = joincode, msg = "Tạo cuộc thi thành công" });
                    }
                  

                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        private string GenerateLinkCode()
        {
            var random = new Random();
            var code = "";
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var length = 11; 

            do
            {
                code = new string(Enumerable.Repeat(characters, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (db.COMPETITIONs.Any(c => c.JoinCode == code)); // Đảm bảo mã là duy nhất trong DB

            return code;
        }
     
    }
}