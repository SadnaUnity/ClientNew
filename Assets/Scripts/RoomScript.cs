using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomScript : MonoBehaviour
{
    
    private HttpRequest httpRequest;
    private Player playerData;

    // Start is called before the first frame update
    void Start()
    {
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("roomId", playerData.GetRoomId()),
            new("userId", playerData.GetUserId())
        };
        httpRequest.SendDataToServer(queryParams, "", "/getIntoRoom", "POST");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
