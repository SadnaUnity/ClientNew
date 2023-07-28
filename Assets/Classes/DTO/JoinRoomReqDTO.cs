using System;
using Classes;

[Serializable]

    public class JoinRoomReqDTO
    {
        public int userId { set; get; }
        public int roomId { set; get; }
        public  string requestStatus { set; get; }
        public string username { set; get; }

        public JoinRoomReqDTO(){}

        public JoinRoomReqDTO(JoinRoomReq joinRoomReq)
        {
            this.userId = joinRoomReq.GetUserId();
            this.username = joinRoomReq.GetUsername();
            this.requestStatus = joinRoomReq.GetStatus();
            this.roomId = joinRoomReq.GetRoomID();
        }

    }
