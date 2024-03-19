using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TESTTABLE
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int TestIn24H { get; set; }
        public int TestInAllTime { get; set; }     
    }
}