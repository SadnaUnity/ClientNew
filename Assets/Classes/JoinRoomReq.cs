namespace Classes
{
    public class JoinRoomReq
    {
        private int userId;
        private int roomId; 
        private string username;

        //private string requestStatus;

        public JoinRoomReq(JoinRoomReqDTO joinRoomReqDto)
        {
            this.userId = joinRoomReqDto.userId;
            this.roomId = joinRoomReqDto.roomId;
            this.username = joinRoomReqDto.username;

          //  this.requestStatus = joinRoomReqDto.requestStatus;
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