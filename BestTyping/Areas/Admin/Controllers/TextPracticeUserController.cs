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
    public class TextPracticeUserController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: Admin/TextPracticeUser
        public ActionResult Index()
        {
            var listText = (from t in db.TEXTPRACTICEs 
                            join u in db.USERs on t.UserCreate equals u.Id
                            join l in db.EXERCISELANGUAGEs on t.LanguageID equals l.LanguageID
                            select new TEXTPRACTICEVIEW
                            {
                                Avatart=u.Avatar,
                                Name = u.HoTen,
                                Language = l.LanguageName,
                                Text = t.Text,
                                Title = t.Title,
                                Rating = t.Rating ?? 0,
                                Id = t.ID,
                                Status = t.Status ?? true
                            }).ToList();
            return View(listText);
        }
        public JsonResult LockTextPractice(int id)
        {
            var finduser = db.TEXTPRACTICEs.Where(x => x.ID == id).SingleOrDefault();
            if (finduser == null)
            {
                return Json(new { code = 400, msg = "Không tồn tại!" });
            }
            finduser.Status = !finduser.Status;
            db.SubmitChanges();
            return Json(new { code = 200 });
        }
    }
}