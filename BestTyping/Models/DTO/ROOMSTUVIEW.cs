using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class ROOMSTUVIEW
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; }
        public CLASSROOM Room { get; set; }
        public List<TESTEDUTABLE> ListEvent { get; set; }
    }
}