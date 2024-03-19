using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class GAMETYPING
    {
        public string TextArr { get; set; }
        public List<TABLERESULTGAME> ListResultGame { get; set; }
    }
}