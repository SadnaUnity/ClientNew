using System;
using UnityEngine;

[Serializable]
public class PositionDTO
{
    public int id{ set; get; }
    public float x{ set; get; }
    public float y{ set; get; }
    public PositionDTO()
    {
    }

    public PositionDTO(int id, float x, float y)
    {
        this.id = id;
        this.x = x;
        this.y = y;
    }
}
