using UnityEngine;

[System.Serializable]
public class Position
{
    [SerializeField] private int id;
    [SerializeField] private float x;
    [SerializeField] private float y;

    public Position(PositionDTO positionDto)
    {
        this.id = positionDto.id;
        this.x = positionDto.x;
        this.y = positionDto.y;
    }
    public Position(int id, float x, float y)
    {
        this.id = id;
        this.x = x;
        this.y = y;
    }

    public int GetId()
    {
        return id;
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