using BestTyping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Controllers
{
    public class TypingTestCustomController : Controller
    {
        // GET: TypingTestCustom
        DataBestTypingDataContext db = new DataBestTypingDataContext();

        [HttpPost]
        public JsonResult HandleShowUserProgess()
        {
            try
            {
                USER getUser = (USER)Session["User"];

                if (getUser == null)
                {
                    return Json(new { code = 500, msg = "Người dùng không hợp lệ" });
                }
                else
                {
                    var getProgess = db.USERPROGESSes.FirstOrDefault(u => u.UserID == getUser.Id);
                    if (getProgess != null)
                    {
                        return Json(new { code = 200, data = getProgess, msg = "Lưu thành công" });
                    }
                    else
                    {
                        return Json(new { code = 400, data = getProgess, msg = "chưa có userprogess" });
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetExerciseText(int exerciseid, string language)
        {
            try
            {
                var getLanguage = db.EXERCISELANGUAGEs.FirstOrDefault(l => l.LanguageName==language);
                int languageId = getLanguage.LanguageID;
                var getExerciseTexts = db.EXERCISETEXTs.Where(t => t.LanguageID == languageId && t.ExerciseID == exerciseid).FirstOrDefault();

                var randomExerciseText = getExerciseTexts;

                // Tạo mảng chứa từng từ được phân tách bằng khoảng trắng
                var wordArray = randomExerciseText.Text.Split(' ').ToList();


                return Json(new { code = 200, msg = "Lấy dữ liệu thành công", data = wordArray });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
    }
}