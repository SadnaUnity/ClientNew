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
        var res = httpRequest.SendDataToServer(queryParams, "", "/room/" + playerData.GetRoomId().ToString(), "GET");
        if (res.Item1 == 200)
        {
            RoomDataDTO roomDataDto = JsonConvert.DeserializeObject<RoomDataDTO>(res.Item2);
            List<PosterDTO> postersDto = roomDataDto.room.posters;
            foreach (var posterDto in postersDto)
            {
                // Load the image from the URL and set it as the sprite for the SpriteRenderer
                StartCoroutine(LoadImageFromURL(posterDto.fileUrl,new Vector3(posterDto.position.x, posterDto.position.y, 0)));
                               
            } 
        }
        else
        {
            Debug.Log("Error get room id: " + playerData.GetRoomId());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadImageFromURL(string url, Vector3 position)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load image from URL: " + request.error);
            yield break;
        }

        Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        GameObject poster = new GameObject();
        poster.transform.position = position;
        poster.transform.localScale = new Vector3(20f, 20f, 20f);
        SpriteRenderer spriteRenderer = poster.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }



}



