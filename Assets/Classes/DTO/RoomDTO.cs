using System;
using System.Collections.Generic;

[Serializable]
    public class RoomDTO
    {
        public bool privacy { set; get; }
        public int maxCapacity { set; get; }
        public int managerId { set; get; }
        public int roomId { set; get; }
        public List<PosterDTO> posters { set; get; }
        public String roomName { set; get; }

        public RoomDTO(){}
    }

   
