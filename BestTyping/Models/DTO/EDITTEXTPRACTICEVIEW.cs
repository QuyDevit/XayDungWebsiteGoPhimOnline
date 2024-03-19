using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class EDITTEXTPRACTICEVIEW
    {
        public string Language { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsPrivate { get; set; }
        public string JoinCode { get; set; }
        public List<EXERCISELANGUAGE> ListLanguage { get; set; }
    }
}