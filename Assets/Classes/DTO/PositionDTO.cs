using System;
using UnityEngine;

[Serializable]
public class PositionDTO
{
    public float x{ set; get; }
    public float y{ set; get; }
    public PositionDTO()
    {
    }

    public PositionDTO(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
