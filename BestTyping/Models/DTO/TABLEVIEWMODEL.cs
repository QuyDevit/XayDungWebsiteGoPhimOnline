using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TABLEVIEWMODEL
    {
        public List<RANKTABLE> ListRankTable { get; set; }
        public List<TESTTABLE> ListTestTable { get; set; }
        public List<GLOBALTABLE> ListTestGlobalTable { get; set; }
    }
}