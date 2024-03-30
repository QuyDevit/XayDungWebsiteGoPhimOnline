using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TESTEDUTABLE
    {
        public int IDTest { get; set; }
        public int IDRoom { get; set; }
        public bool Status { get; set; }

        public string TitleTest { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string CodeLink { get; set; }
        public string ClassName { get; set; }
        public string ExamDuration { get; set; }
        public int MaxAttempts { get; set; }
        public bool CheckView { get; set; }
        public TYPINGRESULTEDU Result { get; set; }
        public int CountResultByUser { get; set; }
        public List<string> TextTest { get; set; }
    }
}