
    using System.Collections.Generic;
    using Classes;

    public class RoomRequests
    {
        private List<JoinRoomReq> joinRoomRequests;

        public RoomRequests(RequstesDTO requstesDto)
        {
            if (requstesDto != null)
            {
                joinRoomRequests = new List<JoinRoomReq>();
                foreach (var req in requstesDto.joinRoomRequests)
                {
                    joinRoomRequests.Add(new JoinRoomReq(req));
                }
            }
        }

        public  List<JoinRoomReq> GetJoinRoonmReq()
        {
            return joinRoomRequests;
        }
    }
