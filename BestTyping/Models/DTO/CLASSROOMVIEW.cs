using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class CLASSROOMVIEW
    {
        public int RoomId { get; set; }

        public string ClassName { get; set; }
        public bool Status { get; set; }
        public string JoinCode { get; set; }
        public int SumPeopleJoin { get; set; }

        public string CreateDate { get; set; }
    }
}