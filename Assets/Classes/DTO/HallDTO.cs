using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HallDTO
{
    public string message { set; get; }
    public List <RoomStatusDTO> roomStatuses { set; get; }
    
    public HallDTO()
    {
        
    }
    
   
}
