namespace Classes
{
    public class JoinRoomReq
    {
        private int userId;
        private int roomId; 
       // private string requestStatus;

        public JoinRoomReq(JoinRoomReqDTO joinRoomReqDto)
        {
            this.userId = joinRoomReqDto.userId;
            this.roomId = joinRoomReqDto.roomId;
           // this.requestStatus = joinRoomReqDto.requestStatus;
        }

        public int GetUserId()
        {
            return userId;
        }

        public int GetRoomID()
        {
            return roomId;
        }
        
    }
}