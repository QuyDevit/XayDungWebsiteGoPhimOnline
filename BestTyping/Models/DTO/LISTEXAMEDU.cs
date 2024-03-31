using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class LISTEXAMEDU
    {
        public int TestIDChoose { get; set; }
        public int RoomIdChoose { get; set; }
        public List<TESTEDU> ListTest { get; set; }
        public List<CLASSTEST> ListClassByTest { get; set; }
        public List<USEREXAMEDU> ListUserExam { get; set; }
    }
}