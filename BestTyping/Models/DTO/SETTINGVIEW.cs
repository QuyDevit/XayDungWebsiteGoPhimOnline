using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class SETTINGVIEW
    {
        public USER User { get; set; }
        public USERPROGESS Userprogess { get; set; }
        public List<TYPINGRESULTSETTING> ListResult { get; set; }
    }
}