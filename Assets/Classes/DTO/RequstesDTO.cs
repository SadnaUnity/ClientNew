using System;
using System.Collections.Generic;
[Serializable]

    public class RequstesDTO
    {
        public List<JoinRoomReqDTO> joinRoomRequests { set; get; }

        public RequstesDTO()
        {
            joinRoomRequests = new List<JoinRoomReqDTO>();
            
        }
    }

