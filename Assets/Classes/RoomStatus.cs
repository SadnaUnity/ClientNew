
    public class RoomStatus
    {
        private bool privacy;
        private int managerId;
        private int roomId;
        private string roomName;
        private string roomMemberStatus;

        public RoomStatus(RoomStatusDTO roomStatusDto)
        {
            this.privacy = roomStatusDto.privacy;
            this.managerId = roomStatusDto.managerId;
            this.roomId = roomStatusDto.roomId;
            this.roomName = roomStatusDto.roomName;
            this.roomMemberStatus = roomStatusDto.roomMemberStatus;
        }

        public string GetRoomName()
        {
            return roomName;
        }
    }
