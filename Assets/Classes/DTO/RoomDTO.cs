using System;
using System.Collections.Generic;
using JetBrains.Annotations;

[Serializable]
public class RoomDTO
{
    public bool privacy { set; get; }
    public int managerId { set; get; }
    public int maxCapacity { set; get; }
    public int roomId { set; get; }
    public string roomName { set; get; }
    [CanBeNull] public string description { set; get; }
    public List<PosterDTO> posters { set; get; }

    public RoomDTO(){}
}

   
