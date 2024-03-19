using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class RESULTCOMPETITIONVIEWMODEL
    {
        public List<TABLERESULTCOMPETITION> ListResultCompetition { get; set; }
        public COMPETITION Competition { get; set; }
    }
}