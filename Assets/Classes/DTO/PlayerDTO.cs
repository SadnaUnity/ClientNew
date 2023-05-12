using System;
using UnityEngine;

[Serializable]
public class PlayerDTO
{ 
   public int userId { set; get; }
   public string message{ set; get; }
   public AvatarDTO avatar{ set; get; }
   public string username { set; get; }
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
