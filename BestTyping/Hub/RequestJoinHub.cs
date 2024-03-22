using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using BestTyping.Models;
using BestTyping.Models.DTO;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;

namespace BestTyping
{
    public class RequestJoinHub : Hub
    {
        DataBestTypingDataContext db = new DataBestTypingDataContext();

        public void SendRequsetJoinList(int idroom)
        {
            var getroom = db.CLASSROOMs.FirstOrDefault(r => r.ClassRoomId == idroom);
            if (getroom != null)
            {
                var dataUserRequest = JsonConvert.DeserializeObject<List<USERROOM>>(getroom.ListUserRequest);
                // Gửi danh sách sinh viên tới tất cả clients
                Clients.All.updateRequestJoinTable(dataUserRequest, idroom); // Gửi cả idroom
            }
            else
            {
                // Nếu không tìm thấy phòng, gửi một danh sách rỗng
                Clients.All.updateRequestJoinTable(new List<USERROOM>(), idroom);
            }
        }
    }
}