using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BestTyping.Models.DTO
{
    public class ROOMSETTINGVIEW
    {
        public int RoomId { get; set; }
        public string AvatarClassRoom { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string JoinCode { get; set; }
        public string PassClassRoom { get; set; }
        public List<USERROOM> ListMember { get; set; }
        public List<USERROOM> ListUserRequest { get; set; }
    }
}