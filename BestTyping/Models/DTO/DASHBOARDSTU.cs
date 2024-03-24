using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class DASHBOARDSTU
    {
        public List<CLASSROOM> ListClass { get; set; }
        public List<TESTEDUTABLE> ListEvent{ get; set; }
    }
}