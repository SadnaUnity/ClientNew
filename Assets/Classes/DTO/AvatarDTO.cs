using System;
using UnityEngine;

[Serializable]
public class AvatarDTO
{
    public string accessory { set; get; }
    public string color{ set; get; }

    public AvatarDTO()
    {
    }

    public AvatarDTO(Avatar avatar)
    {
        accessory = avatar.GetAccessory().ToString();
        color = avatar.GetColor().ToString();
    }
}
