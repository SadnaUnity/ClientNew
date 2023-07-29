using System;
using JetBrains.Annotations;

[Serializable]

    public class RoomStatusDTO
    {
        public bool privacy { set; get; }
        public int managerId { set; get; } 
        public int roomId { set; get; }
        public string roomName { set; get; }
        
        [CanBeNull] public string description { set; get; }
        [CanBeNull] public string imageUrl { set; get; }
        public string roomMemberStatus { set; get; }
        
        [CanBeNull] public string requestStatus { set; get; }
    
        public RoomStatusDTO(){}
    }
