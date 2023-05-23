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
        // Attach a Sprite Renderer component to the background game object
        SpriteRenderer backroundspriteRenderer = background.AddComponent<SpriteRenderer>();

        // Set the background image as the sprite for the Sprite Renderer
        backroundspriteRenderer.sprite =
            Resources.Load<Sprite>(
                "Images/backrounnds/vecteezy_big-shuttle-window-on-spaceship-with-view-of-other-planets_6951237");

        // Set the sorting layer of the background object to a lower value
        backroundspriteRenderer.sortingLayerName = "Background";
        backroundspriteRenderer.sortingOrder = -1;
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("roomId", playerData.GetRoomId()),
            new("userId", playerData.GetUserId())
        };
        var res = httpRequest.SendDataToServer(queryParams, "", "/getIntoRoom", "POST");
        if (res.Item1 != 200)
        {
            Debug.Log("Error get room id: " + playerData.GetRoomId());
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}



