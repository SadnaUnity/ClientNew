using UnityEngine;

public class Player
{
    private int id;
    private Avatar avatar;

    public Player(){}
    public Player(PlayerDTO playerDTO)
    {
        id = playerDTO.userId;
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
