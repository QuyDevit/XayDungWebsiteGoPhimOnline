using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TYPINGRESULTSETTING
    {
        public int ResultID { get; set; }
        public int WPM { get; set; }
        public int CorrectWords { get; set; }

        public int KeyStrokes { get; set; }
        public int Mistakes { get; set; }

        public string Timestamp { get; set; }
    }
}