using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class USEREXAMEDU
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string ClassName { get; set; }
        public string TitleTest { get; set; }
        public int UserTestAttempts { get; set; }
        public int MaxTestAttempts { get; set; }

    }
}