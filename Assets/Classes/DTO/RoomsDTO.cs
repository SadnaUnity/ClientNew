using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomsDTO
{
    public string message { set; get; }
    public Dictionary<int, string> roomsData { set; get; }
    
    public RoomsDTO()
    {
        
    }
    
   
}
