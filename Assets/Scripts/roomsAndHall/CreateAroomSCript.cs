using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAroomSCript : MonoBehaviour
{
    [SerializeField] private TMP_InputField RoomName;
    [SerializeField] private Toggle Private;
    [SerializeField] private Image[] backgrounds;
    
    private HttpRequest httpRequest;
    private Player playerData;
    private int chosenBackIndex;
    private float shadow;
    
    // Start is called before the first frame update
    void Start()
    {
        shadow = 154f / 255f;
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        foreach (var back in backgrounds)
        {
            back.color = new Color(shadow, shadow, shadow);
        }
        backgrounds[0].color = new Color(1, 1, 1);
        chosenBackIndex = 0;
    }

    public void CreateARoomBtn()
    {
     
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("roomName", RoomName.text),
        };
        List<KeyValuePair<string, object>> body = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("managerId", playerData.GetUserId()),
            new KeyValuePair<string, object>("privacy", Private.isOn),
            new KeyValuePair<string, object>("description", "hi")

        };

        Dictionary<string, object> jsonBody = body.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        string bodyString = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);
        
        var res = httpRequest.SendDataToServer(queryParams, bodyString, "/room", "POST");
        if (res.Item1 == 200)
        {
            RoomStatusDTO roomStatusDto = JsonConvert.DeserializeObject<RoomStatusDTO>(res.Item2);
            PlayerDataManager.PlayerData.SetRoomId(roomStatusDto.roomId);
            httpRequest = new HttpRequest();
            List<KeyValuePair<string, object>> queryPar = new List<KeyValuePair<string, object>>
            {
                new("userId", playerData.GetUserId())
            };
            httpRequest.SendDataToServer(queryPar, "", "/getOutFromRoom", "POST");
            SceneManager.LoadScene("Moving");
        }
        else
        {
            Debug.Log("error");
        }

    }

    public void ChooseImgThemeBtn()
    {
        
    }
    public void ClickedBackBtn()
    {
        httpRequest = new HttpRequest();
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        httpRequest.SendDataToServer(queryParams, "", "/getOutFromRoom", "POST");
        SceneManager.LoadScene("Moving");
        
    }
    public void OnImageClick(int clickedIndex)
    {
        backgrounds[clickedIndex].color = new Color(1, 1, 1);
        chosenBackIndex = clickedIndex;
        
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (i != clickedIndex)
            {
                backgrounds[i].color = new Color(shadow, shadow, shadow);;
            }
        }
    }
    
}
