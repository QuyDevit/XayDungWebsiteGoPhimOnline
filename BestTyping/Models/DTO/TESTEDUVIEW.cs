using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class TESTEDUVIEW
    {
        public List<CLASSROOMVIEW> ListRoom { get; set; }
        public List<TEXTTESTEDU> ListTextEdu { get; set; }
    }
}