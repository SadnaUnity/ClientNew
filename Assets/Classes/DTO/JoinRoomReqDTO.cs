using System;

[Serializable]

    public class JoinRoomReqDTO
    {
        public int userId { set; get; }
        public int roomId { set; get; }
        public  string requestStatus { set; get; }
        public string username { set; get; }

        public JoinRoomReqDTO(){}
    }
