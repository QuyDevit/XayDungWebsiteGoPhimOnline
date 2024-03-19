using BestTyping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Controllers
{
    public class TypingGameController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: TypingGame

        [HttpPost]
        public JsonResult HandlerSaveResultGame(int score, long timestamp, int level, int exerciseid)
        {
            try
            {
                var user = (USER)Session["User"];
                if (user == null) return Json(new { code = 500, msg = "Vui lòng đăng nhập để lưu kết quả" });

                var result = db.TYPINGRESULTGAMEs.FirstOrDefault(r => r.UserID == user.Id && r.ExerciseId == exerciseid);
                int? maxScore = null;

                if (result == null)
                {
                    result = new TYPINGRESULTGAME { ExerciseId = exerciseid, UserID = user.Id, Timestamp = timestamp };
                    db.TYPINGRESULTGAMEs.InsertOnSubmit(result);
                }
                else
                {
                    maxScore = level == 1 ? result.Score : level == 2 ? result.Score2 : result.Score3;
                }

                if (!maxScore.HasValue || score > maxScore)
                {
                    if (level == 1) result.Score = score;
                    else if (level == 2) result.Score2 = score;
                    else result.Score3 = score;

                    db.SubmitChanges();
                    return Json(new { code = 200, msg = "Lưu thành công" });
                }
                else
                {
                    return Json(new { code = 400, msg = "Vui lòng cải thiện tốc độ để lưu kết quả" });
                }
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }
    }
}