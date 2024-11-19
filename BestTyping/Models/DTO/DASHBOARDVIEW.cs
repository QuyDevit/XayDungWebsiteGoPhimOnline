using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class DASHBOARDVIEW
    {
        public int SumOnline { get; set; }
        public int SumUser { get; set; }
        public int SumExerciseText { get; set; }
        public int SumTextPractice { get; set; }
        public List<CLASSDATA> ListClass { get; set; }
        public List<RANKTABLE> ListResult { get; set; }
    }
    public class CLASSDATA
    {
        public string ClassName { get; set; }
        public string AvatarClassRoom { get; set; }
        public bool IsPrivate { get; set; }
        public int SumMember { get; set; }
        public long CreateDate { get; set; }

    }
}