using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TEXTEDUVIEW
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public string CreateDate { get; set; }
        public bool Status { get; set; }
    }
}