using UnityEngine;

public class Player
{
    private int id;
    private string username;
    private Avatar avatar;

    public Player(){}
    public Player(PlayerDTO playerDTO)
    {
        id = playerDTO.userId;
        username = playerDTO.username;
        avatar = new Avatar(playerDTO.avatar);
    }
    public int GetId()
    {
        return id;
    }

    public Avatar GetAvatar()
    {
        return avatar;
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void SetAvatar(Avatar avatar)
    {
        this.avatar = avatar;
    }
}
