using BestTyping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BestTyping.Models.DTO;
using System.Text;
using Newtonsoft.Json;

namespace BestTyping.Controllers
{
    public class HomeController : Controller
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult _PartialSideBarEdu()
        {
            return PartialView();
        }


        public ActionResult CheckTyping()
        {
            var maxWPMByUser = db.TYPINGRESULTs
                              .GroupBy(t => t.UserID)
                              .Select(g => new
                              {
                                  UserID = g.Key,
                                  MaxWPM = g.Max(tr => tr.WPM),
                                  KeyStrokes = g.First(tr => tr.WPM == g.Max(t => t.WPM)).KeyStrokes,
                                  Timestamp = g.First(tr => tr.WPM == g.Max(t => t.WPM)).Timestamp
                              })
                              .OrderByDescending(result => result.MaxWPM)
                              .ToList();

            List<RANKTABLE> list = new List<RANKTABLE>();
            foreach (var item in maxWPMByUser)
            {
                var user = db.USERs.FirstOrDefault(u => u.Id == item.UserID);
                if (user != null)
                {
                    RANKTABLE rankItem = new RANKTABLE
                    {
                        Avatar = user.Avatar,
                        Name = user.HoTen,
                        Wpm = item.MaxWPM ?? 0,
                        Keystrokes = item.KeyStrokes ?? 0,
                        TimeLastResult = item.Timestamp.HasValue ? ConvertToTimeAgo(item.Timestamp.Value) : string.Empty
                    };
                    list.Add(rankItem);
                }
            }

            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long timestamp24hAgo = currentTimestamp - 86400000;

            var countTestByUser = db.TYPINGRESULTs
                .GroupBy(t => t.UserID)
                .Select(g => new
                {
                    UserID = g.Key,
                    CountTestIn24h = g.Count(tr => tr.Timestamp >= timestamp24hAgo),
                    CountTestAllTime = g.Count()
                })
                .ToList();

            List<TESTTABLE> listTest = new List<TESTTABLE>();
            foreach (var item in countTestByUser)
            {
                var user = db.USERs.FirstOrDefault(u => u.Id == item.UserID);
                if (user != null)
                {
                    TESTTABLE testItem = new TESTTABLE
                    {
                        Avatar = user.Avatar,
                        Name = user.HoTen,
                        TestIn24H = item.CountTestIn24h,
                        TestInAllTime = item.CountTestAllTime
                    };
                    listTest.Add(testItem);
                }
            }

            var countGlobalTestByCountry = (from l in db.EXERCISELANGUAGEs
                         join e in db.EXERCISETEXTs on l.LanguageID equals e.LanguageID into et
                         from e in et.DefaultIfEmpty()
                         join t in db.TYPINGRESULTs on e.ExerciseTextID equals t.ExerciseTextID into te
                         from t in te.DefaultIfEmpty()
                         group t by l.LanguageID into g
                         select new
                         {
                             TotalCount = g.Count(t => t.ExerciseTextID != null),
                             LanguageID = g.Key
                         }).OrderByDescending(x=>x.TotalCount);
            List<GLOBALTABLE> listTestGlobal = new List<GLOBALTABLE>();
            foreach (var item in countGlobalTestByCountry)
            {
                var language = db.EXERCISELANGUAGEs.FirstOrDefault(u => u.LanguageID == item.LanguageID);
                if (language != null)
                {
                    GLOBALTABLE testGlobalItem = new GLOBALTABLE
                    {
                        CountryAvartar = language.LanguageAvatar,
                        Language = language.LanguageName,
                        SumTest = item.TotalCount
                    };
                    listTestGlobal.Add(testGlobalItem);
                }
            }
            var viewModel = new TABLEVIEWMODEL
            {
                ListRankTable = list,
                ListTestTable = listTest,
                ListTestGlobalTable = listTestGlobal
            };
            return View(viewModel);
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
        public ActionResult CheckTypingAdvanced()
        {
            var maxWPMByUser = db.TYPINGRESULTs
                              .GroupBy(t => t.UserID)
                              .Select(g => new
                              {
                                  UserID = g.Key,
                                  MaxWPM = g.Max(tr => tr.WPM),
                                  KeyStrokes = g.First(tr => tr.WPM == g.Max(t => t.WPM)).KeyStrokes,
                                  Timestamp = g.First(tr => tr.WPM == g.Max(t => t.WPM)).Timestamp
                              })
                              .OrderByDescending(result => result.MaxWPM)
                              .ToList();

            List<RANKTABLE> list = new List<RANKTABLE>();
            foreach (var item in maxWPMByUser)
            {
                var user = db.USERs.FirstOrDefault(u => u.Id == item.UserID);
                if (user != null)
                {
                    RANKTABLE rankItem = new RANKTABLE
                    {
                        Avatar = user.Avatar,
                        Name = user.HoTen,
                        Wpm = item.MaxWPM ?? 0,
                        Keystrokes = item.KeyStrokes ?? 0,
                        TimeLastResult = item.Timestamp.HasValue ? ConvertToTimeAgo(item.Timestamp.Value) : string.Empty
                    };
                    list.Add(rankItem);
                }
            }

            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long timestamp24hAgo = currentTimestamp - 86400000;

            var countTestByUser = db.TYPINGRESULTs
                .GroupBy(t => t.UserID)
                .Select(g => new
                {
                    UserID = g.Key,
                    CountTestIn24h = g.Count(tr => tr.Timestamp >= timestamp24hAgo),
                    CountTestAllTime = g.Count()
                })
                .ToList();

            List<TESTTABLE> listTest = new List<TESTTABLE>();
            foreach (var item in countTestByUser)
            {
                var user = db.USERs.FirstOrDefault(u => u.Id == item.UserID);
                if (user != null)
                {
                    TESTTABLE testItem = new TESTTABLE
                    {
                        Avatar = user.Avatar,
                        Name = user.HoTen,
                        TestIn24H = item.CountTestIn24h,
                        TestInAllTime = item.CountTestAllTime
                    };
                    listTest.Add(testItem);
                }
            }

            var countGlobalTestByCountry = (from l in db.EXERCISELANGUAGEs
                                            join e in db.EXERCISETEXTs on l.LanguageID equals e.LanguageID into et
                                            from e in et.DefaultIfEmpty()
                                            join t in db.TYPINGRESULTs on e.ExerciseTextID equals t.ExerciseTextID into te
                                            from t in te.DefaultIfEmpty()
                                            group t by l.LanguageID into g
                                            select new
                                            {
                                                TotalCount = g.Count(t => t.ExerciseTextID != null),
                                                LanguageID = g.Key
                                            }).OrderByDescending(x => x.TotalCount);
            List<GLOBALTABLE> listTestGlobal = new List<GLOBALTABLE>();
            foreach (var item in countGlobalTestByCountry)
            {
                var language = db.EXERCISELANGUAGEs.FirstOrDefault(u => u.LanguageID == item.LanguageID);
                if (language != null)
                {
                    GLOBALTABLE testGlobalItem = new GLOBALTABLE
                    {
                        CountryAvartar = language.LanguageAvatar,
                        Language = language.LanguageName,
                        SumTest = item.TotalCount
                    };
                    listTestGlobal.Add(testGlobalItem);
                }
            }
            var viewModel = new TABLEVIEWMODEL
            {
                ListRankTable = list,
                ListTestTable = listTest,
                ListTestGlobalTable = listTestGlobal
            };
            return View(viewModel);
        }
        public ActionResult CheckTypingCustom()
        {
            return View();
        }
        public ActionResult Settings()
        {
            USER getuser = (USER)Session["User"];
            if (getuser != null)
            {
                List<TYPINGRESULTSETTING> list = new List<TYPINGRESULTSETTING>();
                var listresult = db.TYPINGRESULTs.Where(r=>r.JoinCode == null && r.UserID == getuser.Id).OrderByDescending(r=>r.ResultId).ToList();
                foreach(var i in listresult)
                {
                    var exercise = db.EXERCISETEXTs.FirstOrDefault(e => e.ExerciseTextID == i.ExerciseTextID);
                    if(exercise.ExerciseID == 1 || exercise.ExerciseID == 2)
                    {
                        TYPINGRESULTSETTING t = new TYPINGRESULTSETTING();
                        t.ResultID = i.ResultId;
                        t.WPM = i.WPM ?? 0;
                        t.KeyStrokes = i.KeyStrokes ?? 0;
                        t.Mistakes = i.Mistakes ?? 0;
                        t.CorrectWords = i.CorrectWords ?? 0;
                        t.Timestamp = ConvertToTimeAgo(i.Timestamp ?? 0);
                        list.Add(t);
                    }                   
                }
                var user = db.USERs.FirstOrDefault(u => u.Id == getuser.Id);
                SETTINGVIEW setting = new SETTINGVIEW();
                setting.User = user;
                setting.ListResult = list;
                var getProgess = db.USERPROGESSes.FirstOrDefault(p => p.UserID == getuser.Id);
                if (getProgess != null)
                {
                    setting.Userprogess = getProgess;
                    return View(setting);
                }
                else
                {
                    USERPROGESS userprogessnew = new USERPROGESS();
                    userprogessnew.SoBaiKiemTra = 0;
                    userprogessnew.SoTuDaGo = 0;
                    userprogessnew.WPMTotNhat = 0;
                    userprogessnew.CuocThiThamGia = 0;
                    setting.Userprogess = userprogessnew;
                    return View(setting);
                }
            }
            else
            {
                return RedirectToAction("CheckTyping", "Home");
            }
        }
        public ActionResult CompetitionsTyping()
        {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // Sử dụng DateTimeOffset để tránh vấn đề múi giờ và chuyển đổi sang Unix milliseconds
            var twentyFourHoursAgo = currentTime - (24 * 60 * 60 * 1000); // Trừ đi 24 giờ tính bằng milliseconds

            var listLanguage = db.EXERCISELANGUAGEs.OrderBy(l => l.LanguageName).ToList();

            var listCompetition = db.COMPETITIONs
                .Where(c => c.IsPrivate == false && c.CreateDate.HasValue && c.CreateDate >= twentyFourHoursAgo)
                .OrderBy(c => c.CreateDate)
                .ToList();

            List<TABLEJOINCOMPETITION> list = new List<TABLEJOINCOMPETITION>();
            foreach (var item in listCompetition)
            {
                var getlanguage = db.EXERCISELANGUAGEs.FirstOrDefault(l => l.LanguageID == item.LanguageId);
                TABLEJOINCOMPETITION join = new TABLEJOINCOMPETITION();
                join.CountryAvartar = getlanguage.LanguageAvatar;
                join.Joincode = item.JoinCode;
                join.SumPeople = item.PeopleJoin ?? 0;
                join.NumberOfTestPerpormed = item.NumberOfTestsPerformed ?? 0;
                join.TimeCountDown = ConvertToTimeAgo24hours(item.CreateDate ?? 0 - (twentyFourHoursAgo)); 
                list.Add(join);
            }

            COMPETITIONVIEWMODEL view = new COMPETITIONVIEWMODEL();
            view.ListLanguageTable = listLanguage;
            view.ListJoinCompetitionTable = list;

            return View(view);
        }
        public static string ConvertToTimeAgo24hours(long timestamp)
        {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var timeSpan = currentTime - timestamp;

            if (timestamp == 0)
                return "vừa xong";
            if (timeSpan < 0)
                return "hết thời gian";

            var timeLeft = 24 * 60 * 60 * 1000 - timeSpan; // Tính thời gian còn lại trong 24 giờ tính từ thời điểm timestamp

            if (timeLeft <= 0)
                return "hết thời gian";

            var hoursLeft = timeLeft / (60 * 60 * 1000);
            var minutesLeft = (timeLeft % (60 * 60 * 1000)) / (60 * 1000);
            var secondsLeft = ((timeLeft % (60 * 60 * 1000)) % (60 * 1000)) / 1000;

            if (hoursLeft > 0)
            {
                return $"{hoursLeft} giờ";
            }
            else if (minutesLeft > 0)
            {
                return $"{minutesLeft} phút";
            }
            else
            {
                return $"{secondsLeft} giây";
            }
        }

        public ActionResult TextPractice()
        {
            List<TEXTPRACTICEMODELVIEW> list = new List<TEXTPRACTICEMODELVIEW>();     
            var mytext = db.TEXTPRACTICEs.Where(t => t.IsPrivate == false).OrderByDescending(t => t.Rating).ToList();
            foreach(var item in mytext)
            {
                TEXTPRACTICEMODELVIEW text = new TEXTPRACTICEMODELVIEW();
                text.IsPrivate = item.IsPrivate ?? false;
                text.JoinCode = item.JoinCode;
                text.PeopleIsCompleted = item.PeopleIsCompleted ?? 0;
                text.TextLength = item.TextLength ?? 0;
                text.Rating = Convert.ToSingle(item.Rating ?? 0);
                text.Text = item.Text;
                text.Title = item.Title;
                text.CreatedAt = ConvertToTimeAgo(item.CreatedAt ?? 0);
                list.Add(text);
            }
            return View(list);
        }
        public ActionResult TextPracticeNew()
        {
            List<TEXTPRACTICEMODELVIEW> list = new List<TEXTPRACTICEMODELVIEW>();
            var mytext = db.TEXTPRACTICEs.Where(t=>t.IsPrivate == false).OrderByDescending(t => t.CreatedAt).ToList();
            foreach (var item in mytext)
            {
                TEXTPRACTICEMODELVIEW text = new TEXTPRACTICEMODELVIEW();
                text.IsPrivate = item.IsPrivate ?? false;
                text.JoinCode = item.JoinCode;
                text.PeopleIsCompleted = item.PeopleIsCompleted ?? 0;
                text.TextLength = item.TextLength ?? 0;
                text.Rating = Convert.ToSingle(item.Rating ?? 0);
                text.Text = item.Text;
                text.Title = item.Title;
                text.CreatedAt = ConvertToTimeAgo(item.CreatedAt ?? 0);
                list.Add(text);
            }
            return View(list);
        }
        public ActionResult TextPracticeLike()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("TextPractice", "Home");
            }
            else
            {
                List<TEXTPRACTICEMODELVIEW> list = new List<TEXTPRACTICEMODELVIEW>();
                var mytext = db.TEXTPRACTICEs.Where(t => t.IsPrivate == false).ToList();
                foreach (var item in mytext)
                {
                    var data = JsonConvert.DeserializeObject<List<USERFAVORITE>>(item.ListUserLike);
                    var existingLike = data.FirstOrDefault(l => l.IdUser == user.Id);
                    if(existingLike != null)
                    {
                        TEXTPRACTICEMODELVIEW text = new TEXTPRACTICEMODELVIEW();
                        text.IsPrivate = item.IsPrivate ?? false;
                        text.JoinCode = item.JoinCode;
                        text.PeopleIsCompleted = item.PeopleIsCompleted ?? 0;
                        text.TextLength = item.TextLength ?? 0;
                        text.Rating = Convert.ToSingle(item.Rating ?? 0);
                        text.Text = item.Text;
                        text.Title = item.Title;
                        text.CreatedAt = ConvertToTimeAgo(item.CreatedAt ?? 0);
                        list.Add(text);
                    }
                }
                return View(list);
            } 
        }
        public ActionResult TextPracticeTest(string codejoin,int rank)
        {
            USER user = (USER)Session["User"];
            var getPractice = db.TEXTPRACTICEs.FirstOrDefault(t => t.JoinCode == codejoin);
            var getUserbytPractice = db.USERs.FirstOrDefault(u => u.Id == getPractice.UserCreate);
            if (getPractice == null)
            {
                return RedirectToAction("TextPractice", "Home");
            }
            else
            {
                var data = JsonConvert.DeserializeObject<List<USERFAVORITE>>(getPractice.ListUserLike);
                var datarating = JsonConvert.DeserializeObject<List<USERCOMPLETED>>(getPractice.ListUserRating);
                var existingLike = data.FirstOrDefault(l => l.IdUser == user?.Id);
                var existingRating = datarating.FirstOrDefault(l => l.IdUser == user?.Id);
                TEXTPRACTICETEST test = new TEXTPRACTICETEST();
                test.CreateDate = ConvertToTimeAgo(getPractice.CreatedAt ?? 0);
                test.Title = getPractice.Title;
                test.UserCreate = getUserbytPractice.HoTen;
                test.TextLength = getPractice.TextLength ?? 0;
                test.PeopleIsCompleted = getPractice.PeopleIsCompleted ?? 0;
                test.Text = getPractice.Text;
                test.Rank = rank;
                test.Rating = Convert.ToSingle(getPractice.Rating ?? 0);
                test.JoinCode = getPractice.JoinCode;

                if (existingRating == null)
                {
                    test.RatingByUser = 0 ;
                }
                else
                {
                    test.RatingByUser = existingRating.Rating;
                }
                if (existingLike == null)
                {
                    test.UserIsLike = false;
                }
                else
                {
                    test.UserIsLike = true;
                }
                return View(test);
            }
        }
        public ActionResult MyTextPractice()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("TextPractice", "Home");
            }
            else
            {
                var mytext = db.TEXTPRACTICEs.Where(t=>t.UserCreate == user.Id).OrderByDescending(t=>t.ID);
                return View(mytext);
            }
        }
        public ActionResult CreateTextPractice()
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("TextPractice", "Home");
            }
            else
            {
                var getLanguage = db.EXERCISELANGUAGEs.ToList();
                return View(getLanguage);
            }   
        }
        public ActionResult EditTextPractice(string codejoin)
        {
            USER user = (USER)Session["User"];
            if (user == null)
            {
                return RedirectToAction("TextPractice", "Home");
            }
            else
            {
                var getTextPractice = db.TEXTPRACTICEs.FirstOrDefault(t => t.JoinCode == codejoin);
                if(getTextPractice == null)
                {
                    return RedirectToAction("CreateTextPractice", "Home");
                }
                else
                {
                    var getLanguage = db.EXERCISELANGUAGEs.ToList();
                    var getLanguagebyText = db.EXERCISELANGUAGEs.FirstOrDefault(t=>t.LanguageID == getTextPractice.LanguageID);
                    EDITTEXTPRACTICEVIEW edittext = new EDITTEXTPRACTICEVIEW();
                    edittext.IsPrivate = getTextPractice.IsPrivate ?? false;
                    edittext.Text = getTextPractice.Text;
                    edittext.JoinCode = getTextPractice.JoinCode;
                    edittext.Language = getLanguagebyText.LanguageName;
                    edittext.Title = getTextPractice.Title;
                    edittext.ListLanguage = getLanguage;

                    return View(edittext);
                }
            }
        }

        public ActionResult GameTyping()
        {
            var listresultgame = db.TYPINGRESULTGAMEs.Where(r=>r.ExerciseId == 6).OrderByDescending(r=>r.Score).ToList();

            List<TABLERESULTGAME> list = new List<TABLERESULTGAME>();
            foreach(var i in listresultgame)
            {
                var getuser = db.USERs.FirstOrDefault(u => u.Id == i.UserID);
                if(getuser != null)
                {
                    TABLERESULTGAME rs = new TABLERESULTGAME();
                    rs.Avar = getuser.Avatar;
                    rs.Name = getuser.HoTen;
                    rs.Score1 = i.Score ?? 0;
                    rs.Score2 = i.Score2 ?? 0;
                    rs.Score3 = i.Score3 ?? 0;
                    list.Add(rs);
                }
            }
            GAMETYPING view = new GAMETYPING();
            var getExerciseTexts = db.EXERCISETEXTs.Where(t=>t.ExerciseID == 6).ToList();
            var random = new Random();
            // Chọn ngẫu nhiên một mục từ danh sách các mục đã lấy được
            var randomExerciseText = getExerciseTexts[random.Next(getExerciseTexts.Count)];
            view.TextArr = randomExerciseText.Text;
            view.ListResultGame = list;
          
            return View(view);
        }
        public ActionResult Game2Typing()
        {
            var listresultgame = db.TYPINGRESULTGAMEs.Where(r => r.ExerciseId == 7).OrderByDescending(r => r.Score).ToList();

            List<TABLERESULTGAME> list = new List<TABLERESULTGAME>();
            foreach (var i in listresultgame)
            {
                var getuser = db.USERs.FirstOrDefault(u => u.Id == i.UserID);
                if (getuser != null)
                {
                    TABLERESULTGAME rs = new TABLERESULTGAME();
                    rs.Avar = getuser.Avatar;
                    rs.Name = getuser.HoTen;
                    rs.Score1 = i.Score ?? 0;
                    rs.Score2 = i.Score2 ?? 0;
                    rs.Score3 = i.Score3 ?? 0;
                    list.Add(rs);
                }
            }
            GAMETYPING view = new GAMETYPING();
            view.ListResultGame = list;

            return View(view);
        }
        public ActionResult Game3Typing()
        {
            var listresultgame = db.TYPINGRESULTGAMEs.Where(r => r.ExerciseId == 8).OrderByDescending(r => r.Score).ToList();

            List<TABLERESULTGAME> list = new List<TABLERESULTGAME>();
            foreach (var i in listresultgame)
            {
                var getuser = db.USERs.FirstOrDefault(u => u.Id == i.UserID);
                if (getuser != null)
                {
                    TABLERESULTGAME rs = new TABLERESULTGAME();
                    rs.Avar = getuser.Avatar;
                    rs.Name = getuser.HoTen;
                    rs.Score1 = i.Score ?? 0;
                    rs.Score2 = i.Score2 ?? 0;
                    rs.Score3 = i.Score3 ?? 0;
                    list.Add(rs);
                }
            }
            GAMETYPING view = new GAMETYPING();
            view.ListResultGame = list;

            return View(view);
        }
        public ActionResult Game4Typing()
        {
            var listresultgame = db.TYPINGRESULTGAMEs.Where(r => r.ExerciseId == 9).OrderByDescending(r => r.Score).ToList();

            List<TABLERESULTGAME> list = new List<TABLERESULTGAME>();
            foreach (var i in listresultgame)
            {
                var getuser = db.USERs.FirstOrDefault(u => u.Id == i.UserID);
                if (getuser != null)
                {
                    TABLERESULTGAME rs = new TABLERESULTGAME();
                    rs.Avar = getuser.Avatar;
                    rs.Name = getuser.HoTen;
                    rs.Score1 = i.Score ?? 0;
                    rs.Score2 = i.Score2 ?? 0;
                    rs.Score3 = i.Score3 ?? 0;
                    list.Add(rs);
                }
            }
            GAMETYPING view = new GAMETYPING();
            view.ListResultGame = list;

            return View(view);
        }

        public ActionResult _PartialSideBarTest()
        {
            var query = (from t in db.TYPINGRESULTs
                         join e in db.EXERCISETEXTs on t.ExerciseTextID equals e.ExerciseTextID
                         join c in db.EXERCISEs on e.ExerciseID equals c.ExerciseId
                         where c.ExerciseId == 1 || c.ExerciseId == 2
                         orderby t.ResultId descending
                         select new
                         {
                             t.UserID,
                             t.WPM,
                             e.ExerciseID,
                         }).Take(10);
            List<PROGRESSUSER> list = new List<PROGRESSUSER>();
            foreach (var item in query)
            {
                var exercise = db.EXERCISEs.FirstOrDefault(e => e.ExerciseId == item.ExerciseID);
                var user = db.USERs.FirstOrDefault(u => u.Id == item.UserID);
                PROGRESSUSER element = new PROGRESSUSER();
                element.Name = user.HoTen;
                element.Avartar = user.Avatar;
                element.ExerciseName = exercise.Title;
                element.WPM = item.WPM ?? 0;
                list.Add(element);
            }

            return PartialView(list);
        }
        [HttpPost]
        public JsonResult GetExerciseText(string codejoin)
        {
            try
            {

                var competition = db.COMPETITIONs.FirstOrDefault(c => c.JoinCode == codejoin);
                var getExerciseText = db.EXERCISETEXTs.FirstOrDefault(e => e.ExerciseTextID == competition.ExerciseTextID);

                var id = getExerciseText.ExerciseTextID;
                var words = getExerciseText.Text.Split(' ').ToList();

                return Json(new { code = 200, msg = "Lấy dữ liệu thành công", data = words, exercisetextid = id });
            }
            catch (Exception)
            {
                return Json(new { code = 500, msg = "Lỗi" });
            }
        }
    }
}