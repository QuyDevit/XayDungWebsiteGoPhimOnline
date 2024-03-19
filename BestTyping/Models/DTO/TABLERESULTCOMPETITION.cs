using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TABLERESULTCOMPETITION
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int WPM { get; set; }
        public string TimeLastResult { get; set; }
    }
}