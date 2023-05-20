using System;

[Serializable]

    public class RoomStatusDTO
    {
        public bool privacy { set; get; }
        public int managerId { set; get; } 
        public int roomId { set; get; }
        public string roomName { set; get; }
        public string roomMemberStatus { set; get; }
    
        public RoomStatusDTO(){}
    }
