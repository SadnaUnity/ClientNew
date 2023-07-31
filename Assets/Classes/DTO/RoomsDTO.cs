using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomsDTO
{
    public string message { set; get; }
    public List <RoomStatusDTO> roomStatuses { set; get; }
    
    public RoomsDTO()
    {
        
    }
    
   
}
