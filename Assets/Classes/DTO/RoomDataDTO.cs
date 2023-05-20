using System;

[Serializable]
public class RoomDataDTO
{
    public string message { set; get; }
    public int roomId { set; get; }
    public RoomDTO room { set; get; }
    
    public RoomDataDTO(){}
    
}
