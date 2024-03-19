using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class RANKTABLE
    {
        public int Wpm { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int Keystrokes { get; set; }
        public string TimeLastResult { get; set; }     
    }
}