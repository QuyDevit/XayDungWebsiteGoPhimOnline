using BestTyping.Models;
using BestTyping.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BestTyping.Areas.Admin.Controllers
{
    [CustomAuthorize]
    public class ManageAccountController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: Admin/ManageAccount
        public ActionResult Index()
        {
            var listaccount =  db.USERs.Where(x=>x.Id != 6).ToList();
            return View(listaccount);
        }
        public JsonResult LockAccount(int id)
        {
            var finduser = db.USERs.Where(x => x.Id == id).SingleOrDefault();
            if(finduser == null)
            {
                return Json(new { code = 400, msg = "Không tồn tại!" });
            }
            finduser.IsEnable = !finduser.IsEnable;
            db.SubmitChanges();
            return Json(new { code = 200 });
        }
    }
}