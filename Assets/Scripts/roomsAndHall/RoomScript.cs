using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.Unity.VisualStudio.Editor;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RoomScript : MonoBehaviour
{

    private HttpRequest httpRequest;
    private Player playerData;
    [SerializeField] private GameObject background;

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
        var res = httpRequest.SendDataToServer(queryParams, "", "/getIntoRoom", "POST");
        if (res.Item1 == 200)
        {
            RoomDataDTO roomDataDto = JsonConvert.DeserializeObject<RoomDataDTO>(res.Item2);
            LoadBackground(roomDataDto.room.background);
        }
    }

    private void LoadBackground(string roomBackground)
    {
        // Attach a Sprite Renderer component to the background game object
        SpriteRenderer backgroundSpriteRenderer = background.AddComponent<SpriteRenderer>();

        string backPath = "";
        switch (roomBackground)
        {
            case "BACKGROUND_1": backPath = "back1"; break;
            case "BACKGROUND_2": backPath = "back2"; break;
            case "BACKGROUND_3": backPath = "back3"; break;
            case "BACKGROUND_4": backPath = "back4"; break;
            default: break;
        }
        // Set the background image as the sprite for the Sprite Renderer
        backgroundSpriteRenderer.sprite =
            Resources.Load<Sprite>($"Images/backrounnds/{backPath}");

        // Set the sorting layer of the background object to a lower value
        backgroundSpriteRenderer.sortingLayerName = "Background";
        backgroundSpriteRenderer.sortingOrder = -1;
        playerData = PlayerDataManager.PlayerData;
    }

    // Update is called once per frame
    void Update()
    {

    }
}



