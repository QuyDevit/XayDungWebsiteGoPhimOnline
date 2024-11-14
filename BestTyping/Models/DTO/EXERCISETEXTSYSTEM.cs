using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class EXERCISETEXTSYSTEM
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public string ExerciseName { get; set; }
        public string ExerciseText { get; set; }
        public string Language { get; set; }
    }
}