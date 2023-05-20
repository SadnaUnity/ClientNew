using System;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class PlayerDTO
{ 
   [CanBeNull] public AvatarDTO avatar{ set; get; }
   public string message{ set; get; }
   public int userId { set; get; }
   [CanBeNull] public string username { set; get; }
   public PlayerDTO()
   {
   }

   public PlayerDTO(Player player)
   {
      userId = player.GetUserId();
      message = "";
      avatar = new AvatarDTO(player.GetAvatar());
   }
}
