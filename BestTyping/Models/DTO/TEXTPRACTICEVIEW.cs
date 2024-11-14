using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TEXTPRACTICEVIEW
    {
        public int Id { get; set; }
        public string Avatart { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
        public double Rating { get; set; }
        public bool Status { get; set; }
    }
}