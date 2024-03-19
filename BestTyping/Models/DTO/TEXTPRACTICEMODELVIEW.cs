using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TEXTPRACTICEMODELVIEW
    {
        public int TextLength { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public int PeopleIsCompleted{ get; set; }
        public string CreatedAt { get; set; }
        public bool IsPrivate { get; set; }
        public float Rating { get; set; }
        public string JoinCode { get; set; }
    }
}