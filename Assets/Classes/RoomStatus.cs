using System;

public enum RoomMemberStatus
{
    MEMBER, NOT_A_MEMBER
}
public class RoomStatus
{
    private bool privacy;
    private int managerId;
    private int roomId;
    private string roomName;
    private RoomMemberStatus roomMemberStatus;

    public RoomStatus(RoomStatusDTO roomStatusDto)
    {
        this.privacy = roomStatusDto.privacy;
        this.managerId = roomStatusDto.managerId;
        this.roomId = roomStatusDto.roomId;
        this.roomName = roomStatusDto.roomName;
        this.roomMemberStatus = (RoomMemberStatus)Enum.Parse(typeof(RoomMemberStatus), roomStatusDto.roomMemberStatus);
    }

    public string GetRoomName()
    {
        return roomName;
    }

    public RoomMemberStatus GetRoomMemberStatus()
    {
        return roomMemberStatus;
    }

    public void SetRoomMemberStatus(RoomMemberStatus memberStatus)
    {
        this.roomMemberStatus = memberStatus;
    }
}
