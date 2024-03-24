using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TESTEDUTABLE
    {
        public int ID { get; set; }
        public bool Status { get; set; }

        public string TitleTest { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string CodeLink { get; set; }
        public string ClassName { get; set; }
    }
}