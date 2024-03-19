using BestTyping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Controllers
{
    public class TypingTestAdvancedController : Controller
    {
        // GET: TypingTestAdvanced
        DataBestTypingDataContext db = new DataBestTypingDataContext();

        [HttpPost]
        public JsonResult HandleSaveResult(int sumword, float accuracy, int exercise, int wpm, int mistakes, int correctwords, long timestamp,int totalcharacters)
        {
            try
            {
                USER getUser = (USER)Session["User"];

                if (getUser == null)
                {
                    return Json(new { code = 500, msg = "Vui lòng đăng nhập để lưu kết quả" });
                }
                var checkUserprogess = db.USERPROGESSes.FirstOrDefault(u => u.UserID == getUser.Id);

                if (checkUserprogess == null)
                {
                    CreateUserProgress(getUser, sumword, wpm);
                }
                else
                {
                    if (wpm <= checkUserprogess.WPMTotNhat - 5)
                    {
                        return Json(new { code = 400, msg = "Vui lòng cải thiện tốc độ để lưu kết quả" });
                    }
                    UpdateUserProgress(checkUserprogess, sumword, wpm);
                }
                SaveTypingResult(getUser.Id, exercise, wpm, mistakes, accuracy, sumword, timestamp, correctwords,totalcharacters);

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
                CuocThiThamGia = 0,
                WPMTotNhat = wpm
            };

            db.USERPROGESSes.InsertOnSubmit(userprogess);
        }

        private void UpdateUserProgress(USERPROGESS checkUserprogess, int sumword, int wpm)
        {
            if (checkUserprogess.WPMTotNhat < wpm)
            {
                checkUserprogess.WPMTotNhat = wpm;
                checkUserprogess.SoBaiKiemTra += 1;
                checkUserprogess.SoTuDaGo += sumword;
            }
            else
            {
                checkUserprogess.SoBaiKiemTra += 1;
                checkUserprogess.SoTuDaGo += sumword;
            }
        }

        private void SaveTypingResult(int userId, int exercise, int wpm, int mistakes, float accuracy, int sumword, long timestamp, int correctwords,int totalcharacters)
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
                KeyStrokes = totalcharacters
            };

            db.TYPINGRESULTs.InsertOnSubmit(typingresult);
        }
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
                var getExerciseTexts = db.EXERCISETEXTs.Where(t => t.LanguageID == languageId && t.ExerciseID == exerciseid).ToList();

                // Chọn ngẫu nhiên một mục từ danh sách các mục đã lấy được
                var random = new Random();
                var words = new List<string>();

                // Chọn ngẫu nhiên một mục từ danh sách các mục đã lấy được
                var randomExerciseText = getExerciseTexts[random.Next(getExerciseTexts.Count)];
                var ExerciseTextID = randomExerciseText.ExerciseTextID;

                // Tạo mảng chứa từng từ được phân tách bằng khoảng trắng
                var wordArray = randomExerciseText.Text.Split(' ');

                // Xáo trộn các từ trong danh sách
                var shuffledWords = wordArray.OrderBy(x => random.Next()).ToList();

                // Thêm từng từ đã xáo trộn vào mảng words
                words.AddRange(shuffledWords);

                return Json(new { code = 200, msg = "Lấy dữ liệu thành công", data = words, exercisetextid = ExerciseTextID });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
    }
}