using BestTyping.Models;
using BestTyping.Models.DTO;
using BestTyping.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class ExerciseTextSystemController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: Admin/ExerciseTextSystem
        public ActionResult Index()
        {
            var listText = (from t in db.EXERCISETEXTs
                            join e in db.EXERCISEs on t.ExerciseID equals e.ExerciseId
                            join l in db.EXERCISELANGUAGEs on t.LanguageID equals l.LanguageID
                            select new EXERCISETEXTSYSTEM
                            {
                                Id = t.ExerciseTextID,
                                ExerciseName = e.Title,
                                ExerciseText = t.Text,
                                Language = l.LanguageName,
                                Status = t.Status ?? true
                            }).ToList();
            return View(listText);
        }
        public ActionResult TextBasic()
        {
            var listText = (from t in db.EXERCISETEXTs
                            join e in db.EXERCISEs on t.ExerciseID equals e.ExerciseId
                            join l in db.EXERCISELANGUAGEs on t.LanguageID equals l.LanguageID
                            where t.ExerciseID == 1
                            select new EXERCISETEXTSYSTEM
                            {
                                Id = t.ExerciseTextID,
                                ExerciseName = e.Title,
                                ExerciseText = t.Text,
                                Language = l.LanguageName,
                                Status = t.Status ?? true

                            }).ToList();
            return View(listText);
        }
        public ActionResult TextAdvanced()
        {
            var listText = (from t in db.EXERCISETEXTs
                            join e in db.EXERCISEs on t.ExerciseID equals e.ExerciseId
                            join l in db.EXERCISELANGUAGEs on t.LanguageID equals l.LanguageID
                            where t.ExerciseID == 2
                            select new EXERCISETEXTSYSTEM
                            {
                                Id = t.ExerciseTextID,
                                ExerciseName = e.Title,
                                ExerciseText = t.Text,
                                Language = l.LanguageName,
                                Status = t.Status ?? true
                            }).ToList();
            return View(listText);
        }
        public ActionResult TextCompetition()
        {
            var listText = (from t in db.EXERCISETEXTs
                            join e in db.EXERCISEs on t.ExerciseID equals e.ExerciseId
                            join l in db.EXERCISELANGUAGEs on t.LanguageID equals l.LanguageID
                            where t.ExerciseID == 4
                            select new EXERCISETEXTSYSTEM
                            {
                                Id = t.ExerciseTextID,
                                ExerciseName = e.Title,
                                ExerciseText = t.Text,
                                Language = l.LanguageName,
                                Status = t.Status ?? true
                            }).ToList();
            return View(listText);
        }
        public ActionResult TextGame()
        {
            var listText = (from t in db.EXERCISETEXTs
                            join e in db.EXERCISEs on t.ExerciseID equals e.ExerciseId
                            join l in db.EXERCISELANGUAGEs on t.LanguageID equals l.LanguageID
                            where t.ExerciseID == 6
                            select new EXERCISETEXTSYSTEM
                            {
                                Id = t.ExerciseTextID,
                                ExerciseName = e.Title,
                                ExerciseText = t.Text,
                                Language = l.LanguageName,
                                Status = t.Status ?? true
                            }).ToList();
            return View(listText);
        }
        public ActionResult AddText()
        {
            ViewBag.Language = db.EXERCISELANGUAGEs.ToList();
            return View();
        }
        public ActionResult EditText(int id)
        {
            ViewBag.Language = db.EXERCISELANGUAGEs.ToList();
            var findtext = db.EXERCISETEXTs.SingleOrDefault(x => x.ExerciseTextID == id);
            return View(findtext);
        }
        #region handle
        public JsonResult LockText(int id)
        {
            var finduser = db.EXERCISETEXTs.Where(x => x.ExerciseTextID == id).SingleOrDefault();
            if (finduser == null)
            {
                return Json(new { code = 400, msg = "Không tồn tại!" });
            }
            finduser.Status = !finduser.Status;
            db.SubmitChanges();
            return Json(new { code = 200 });
        }
        public JsonResult CreateText(int languageid, string text, List<int> arrcategory, bool display)
        {
            if (string.IsNullOrEmpty(text) || arrcategory.Count < 1)
            {
                return Json(new { code = 400, msg = "Vui lòng nhập đầy đủ thông tin!" });
            }
            foreach (int id in arrcategory) {
                var newtext = new EXERCISETEXT
                {
                    LanguageID = languageid,
                    Text = text.ToLower(),
                    ExerciseID = id,
                    Status = true
                };
                db.EXERCISETEXTs.InsertOnSubmit(newtext);
            }

            db.SubmitChanges();
            return Json(new { code = 200 });
        }
        public JsonResult SaveText(int id, int languageid, string text, List<int> arrcategory, bool display)
        {
            if (string.IsNullOrEmpty(text) || arrcategory.Count < 1)
            {
                return Json(new { code = 400, msg = "Vui lòng nhập đầy đủ thông tin!" });
            }
            var findText = db.EXERCISETEXTs.SingleOrDefault(x => x.ExerciseTextID == id);
            if (findText == null)
            {
                return Json(new { code = 400, msg = "Không tìm thấy văn bản!" });
            }
            findText.Text = text;
            findText.LanguageID = languageid;
            findText.ExerciseID = arrcategory[0];
            findText.Status = display;
            db.SubmitChanges();
            return Json(new { code = 200 });
        }
        #endregion
    }
}