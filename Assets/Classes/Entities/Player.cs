using System.Collections.Generic;
using Classes;
using UnityEngine;

public class Player
{
    private int userId;
    private int roomId;
    private string username;
    private Avatar avatar;

    public Player(){}
    public Player(PlayerDTO playerDTO)
    {
        roomId = 1;
        userId = playerDTO.userId;
        username = playerDTO.username;
        avatar = new Avatar(playerDTO.avatar);
    }
    public int GetUserId()
    {
        return userId;
    }

    public int GetRoomId()
    {
        return roomId;
    }
    public Avatar GetAvatar()
    {
        return avatar;
    }

    public void SetUserId(int id)
    {
        this.userId = id;
    }
    public void SetRoomId(int id)
    {
        this.roomId = id;
    }

    public void SetAvatar(Avatar avatar)
    {
        this.avatar = avatar;
    }
}
