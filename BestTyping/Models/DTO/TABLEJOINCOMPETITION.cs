using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TABLEJOINCOMPETITION
    {
        public string CountryAvartar { get; set; }
        public string Joincode { get; set; }
        public int SumPeople { get; set; }
        public int NumberOfTestPerpormed { get; set; }
        public string TimeCountDown { get; set; }
    }
}