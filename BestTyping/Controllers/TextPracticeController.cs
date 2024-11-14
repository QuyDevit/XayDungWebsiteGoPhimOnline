using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BestTyping.Models;
using BestTyping.Models.DTO;
using Newtonsoft.Json;

namespace BestTyping.Controllers
{
    public class TextPracticeController : Controller
    {
        private static readonly HttpClient _client = new HttpClient();
        private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";
        DataBestTypingDataContext db = new DataBestTypingDataContext();
        // GET: TextPractice
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreateTextPractice(string title,string language, string content,bool isprivate,long createat,int textlength)
        {
            try
            {
                USER user = (USER)Session["User"];
                if(user == null)
                {
                    return Json(new { code = 400, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    if(string.IsNullOrEmpty(title) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(content) || (isprivate != true && isprivate != false) || createat<=0 || textlength<=0)
                    {
                        return Json(new { code = 400, msg = "Vui lòng điền đầy đủ thông tin" });
                       
                    }
                    else
                    {
                        var getlanguage = db.EXERCISELANGUAGEs.FirstOrDefault(l => l.LanguageName.Equals(language));                        
                        TEXTPRACTICE text = new TEXTPRACTICE();
                        text.UserCreate = user.Id;
                        text.Title = title;
                        text.PeopleIsCompleted = 0;
                        text.Rating = 0;
                        text.CreatedAt = createat;
                        text.Text = content;
                        text.ListUserLike = "[]";
                        text.TextLength = textlength;
                        text.JoinCode = GenerateLinkCode();
                        text.LanguageID = getlanguage.LanguageID;
                        text.IsPrivate = isprivate;
                        text.ListUserRating = "[]";
                        db.TEXTPRACTICEs.InsertOnSubmit(text);
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Tạo văn bản thành công" });
                    }
                  
                }
                
            }
            catch(Exception ex)
            {
                return Json(new { code = 500,msg = ex.Message });
            }
           
        }
        [HttpPost]
        public JsonResult EditTextPracticeApi(string codejoin,string title, string language, string content, bool isprivate, int textlength)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 400, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(content) || (isprivate != true && isprivate != false))
                    {
                        return Json(new { code = 400, msg = "Vui lòng điền đầy đủ thông tin" });
                    }
                    else
                    {
                        var getlanguage = db.EXERCISELANGUAGEs.FirstOrDefault(l => l.LanguageName.Equals(language));
                        var getTextPractice = db.TEXTPRACTICEs.FirstOrDefault(t => t.JoinCode == codejoin && t.UserCreate == user.Id);
                        if(getTextPractice == null)
                        {
                            return Json(new { code = 500, msg = "Không tồn tại văn bản này" });
                        }
                        else
                        {
                            getTextPractice.LanguageID = getlanguage.LanguageID;
                            getTextPractice.IsPrivate = isprivate;
                            getTextPractice.Text = content;
                            getTextPractice.Title = title;
                            getTextPractice.TextLength = textlength;
                            db.SubmitChanges();
                            return Json(new { code = 200, msg = "Lưu văn bản thành công" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult LikeTextPracticeByUser(string codejoin)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 400, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    var getTextPractice = db.TEXTPRACTICEs.FirstOrDefault(t => t.JoinCode == codejoin);
                    if(getTextPractice == null)
                    {
                        return Json(new { code = 500, msg = "Văn bản không tồn tại" });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(getTextPractice.ListUserLike))
                        {
                            getTextPractice.ListUserLike = "[]"; 
                        }
                        var data = JsonConvert.DeserializeObject<List<USERFAVORITE>>(getTextPractice.ListUserLike);
                        var existingLike = data.FirstOrDefault(l => l.IdUser == user.Id);
                        if(existingLike != null)
                        {
                            data.Remove(existingLike);
                        }else
                        {
                            long currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            data.Add(new USERFAVORITE { IdUser = user.Id, JoinCode = codejoin, DateLike = currentTimestamp });
                        }
                        getTextPractice.ListUserLike = JsonConvert.SerializeObject(data);
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Like văn bản thành công" });
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
            var length = 20;

            do
            {
                code = new string(Enumerable.Repeat(characters, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            } while (db.TEXTPRACTICEs.Any(c => c.JoinCode == code)); // Đảm bảo mã là duy nhất trong DB

            return code;
        }
        [HttpPost]
        public JsonResult SaveResultTypingTextPractice(string codejoin,int rating)
        {
            try
            {
                USER user = (USER)Session["User"];
                if (user == null)
                {
                    return Json(new { code = 400, msg = "Vui lòng đăng nhập để tiếp tục" });
                }
                else
                {
                    var getTextPractice = db.TEXTPRACTICEs.FirstOrDefault(t => t.JoinCode == codejoin);
                    if (string.IsNullOrEmpty(getTextPractice.ListUserRating))
                    {
                        getTextPractice.ListUserRating = "[]";
                    }
                    var data = JsonConvert.DeserializeObject<List<USERCOMPLETED>>(getTextPractice.ListUserRating);
                    int sumrating = 0;
                    int peoplerating = 0;
                    float avgrating = 0;
                    foreach (var item in data)
                    {
                        if(item.IdUser != user.Id)
                        {
                            peoplerating += 1;
                            sumrating += item.Rating;
                        }
                    }
                    var existingRating = data.FirstOrDefault(l => l.IdUser == user.Id);
                    if (existingRating != null)
                    {
                        existingRating.Rating = rating;
                        sumrating += rating;  // Cộng rating mới
                        peoplerating += 1;
                        avgrating = (float)sumrating / peoplerating;
                        getTextPractice.Rating = avgrating;
                    }
                    else
                    {
                        getTextPractice.PeopleIsCompleted += 1;
                        sumrating += rating;  // Cộng rating mới
                        peoplerating += 1;
                        avgrating = (float)sumrating / peoplerating;
                        getTextPractice.Rating = avgrating;
                        data.Add(new USERCOMPLETED { IdUser = user.Id, JoinCode = codejoin, Rating = rating });
                    }
                    getTextPractice.ListUserRating = JsonConvert.SerializeObject(data);
                    db.SubmitChanges();
                    return Json(new { code = 200, msg = "Đã hoàn thành văn bản thành công" ,avgratingnew = avgrating });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }

        }

        [HttpPost]
        public JsonResult DeleteTextPractice(string data)
        {
            try
            {
                USER us = (USER)Session["User"];
                if (us != null)
                {
                    if (string.IsNullOrEmpty(data))
                    {
                        return Json(new { code = 500, msg = "Đã có lỗi xảy ra" });
                    }
                    else
                    {
                        var text = db.TEXTPRACTICEs.FirstOrDefault(t => t.JoinCode == data);
                        if(text != null)
                        {
                            db.TEXTPRACTICEs.DeleteOnSubmit(text);
                        }
                        db.SubmitChanges();
                        return Json(new { code = 200, msg = "Xóa thành công" });
                    }
                }
                else
                {
                    return Json(new { code = 400, msg = "Vui lòng đăng nhập" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = ex.Message });
            }
        }
        [HttpPost]
        public async Task<JsonResult> Chat(string content,string language)
        {
            try
            {
                var ask = $"Viết một (đoạn văn, bài thơ, lời bài hát,...) về chủ đề \"{content}\" bằng tiếng \"{language}\" ";
                var request = new ChatRequest
                {
                    Model = "gemini-1.5-pro",
                    MaxTokens = 2048,
                    Messages = new[]
                {
                    new Message
                    {
                        Role = "user",
                        Content = ask
                    }
                }
                };
                var responseContent = await GenerateResponse(request);
                return Json(responseContent);
            }
            catch (Exception ex)
            {
                return Json(new { code = 400 });
            }
        }
        public async Task<string> GenerateResponse(ChatRequest request)
        {
            var apiKey = ConfigurationManager.AppSettings["config:ApiKey"];
            int maxTokens = request.MaxTokens <= 0 ? 2048 : request.MaxTokens;

            var geminiRequest = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = request.Messages[0].Content
                        }
                    }
                }
            },
                generationConfig = new
                {
                    maxOutputTokens = maxTokens,
                    temperature = 0.7,
                    topP = 0.8,
                    topK = 40
                }
            };

            var requestUrl = $"{GEMINI_API_URL}?key={apiKey}";
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(geminiRequest),
                Encoding.UTF8,
                "application/json"
            );

            var geminiResponse = await _client.PostAsync(requestUrl, requestContent);
            var responseContent = await geminiResponse.Content.ReadAsStringAsync();

            if (!geminiResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API error: {responseContent}");
            }

            return responseContent;
        }
    }
}