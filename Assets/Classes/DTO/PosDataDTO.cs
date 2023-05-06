
using System;
using UnityEngine;

[Serializable]
public class PosDataDTO
{
    public PlayerDTO playerDto{ set; get; }
    public PositionDTO positionDto{ set; get; }

    public PosDataDTO()
    {
    }

    public PosDataDTO(PlayerDTO playerDto, PositionDTO positionDto)
    {
        this.playerDto = playerDto;
        this.positionDto = positionDto;
    }
}
