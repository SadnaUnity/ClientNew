using UnityEngine;


public class Position
{
    private float x;
    private float y;

    public Position(PositionDTO positionDto)
    {
        this.x = positionDto.x;
        this.y = positionDto.y;
    }
    public Position(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
    public float GetX()
    {
        return x;
    }
    public float GetY()
    {
        return y;
    }
}