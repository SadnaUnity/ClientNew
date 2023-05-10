
using System;
using System.Collections.Generic;
using Classes.DTO;
using UnityEngine;

[Serializable]
public class PosDataDTO
{
    public string message { set; get; }
    public List<AvatarPositionDTO> avatarPositions { set; get; }

    public PosDataDTO()
    {
    }

   
    
}
