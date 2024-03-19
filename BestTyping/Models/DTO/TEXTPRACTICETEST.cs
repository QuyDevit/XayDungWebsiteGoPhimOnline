using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TEXTPRACTICETEST
    {
        public string Title { get; set; }
        public string CreateDate { get; set; }
        public string UserCreate { get; set; }
        public int TextLength { get; set; }
        public int PeopleIsCompleted { get; set; }
        public string Text { get; set; }
        public bool UserIsLike { get; set; }
        public int Rank { get; set; }
        public float Rating { get; set; }
        public string JoinCode { get; set; }
        public int RatingByUser { get; set; }
    }
}