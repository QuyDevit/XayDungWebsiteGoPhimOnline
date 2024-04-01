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
        public int WPM { get; set; }
        public int CorrectWords { get; set; }
        public int WrongWords { get; set; }
        public int KeyStrokes { get; set; }
        public int WrongCharacters { get; set; }
        public int CorrectCharacters { get; set; }

    }
}