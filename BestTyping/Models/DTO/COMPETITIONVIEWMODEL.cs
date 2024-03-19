using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class COMPETITIONVIEWMODEL
    {
        public List<EXERCISELANGUAGE> ListLanguageTable { get; set; }
        public List<TABLEJOINCOMPETITION> ListJoinCompetitionTable { get; set; }
    }
}