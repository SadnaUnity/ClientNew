using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

[Serializable]
public enum AvatarColor
{
    PINK, BLUE, GREEN, YELLOW
}
[Serializable]
public enum AvatarAccessory
{
    HEART_GLASSES, SANTA_HAT, NORMAL_GLASSES, COOK_HAT, EMPTY
}

public class Avatar
{
    private AvatarAccessory accessory;
    private AvatarColor color;

    public Avatar(AvatarDTO avatarDTO)
    {
        accessory = (AvatarAccessory)Enum.Parse(typeof(AvatarAccessory), avatarDTO.accessory);
        color = (AvatarColor)Enum.Parse(typeof(AvatarColor), avatarDTO.color);
    }
    public AvatarAccessory GetAccessory()
    {
        return accessory;
    }
    
    public AvatarColor GetColor()
    {
        return color;
    }

    public void SetAccessory(AvatarAccessory accessory)
    {
        this.accessory = accessory;
    }
    
    public void SetColor(AvatarColor color)
    {
        this.color = color;
    }
}
