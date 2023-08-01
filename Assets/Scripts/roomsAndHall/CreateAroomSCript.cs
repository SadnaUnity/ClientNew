using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    private byte[] themeImgFile;
    
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
        playerData = PlayerDataManager.PlayerData;
        string backName = GetBackgroundName(chosenBackIndex);
        
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("roomName", RoomName.text),
        };
        List<KeyValuePair<string, object>> body = new List<KeyValuePair<string, object>>
        {
            new ("managerId", playerData.GetUserId()),
            new ("privacy", Private.isOn),
            new ("description", "hi"),
            new ("background", backName)
        };

        Dictionary<string, object> jsonBody = body.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        string bodyString = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);
        
        var res = httpRequest.SendDataToServer(queryParams, bodyString, "/room", "POST");
        
        if (res.Item1 == 200)
        {
            RoomDataDTO roomDataDto = JsonConvert.DeserializeObject<RoomDataDTO>(res.Item2);
            int roomId = roomDataDto.room.roomId;
            PlayerDataManager.PlayerData.SetRoomId(roomId);
            SendThemeImg(roomId);
        }
        else
        {
            Debug.Log("error");
        }

    }

    private void SendThemeImg(int roomId)
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        
        var res = httpRequest.SendDataToServer(queryParams, "file", themeImgFile, $"/roomImage/{roomId}");
        if (res.Item1 == 200)
        {
            Debug.Log("theme image upload success");
            //TODO: load hall
            SceneManager.LoadScene("Moving");
        }
        else
        {
            Debug.Log("theme image upload ERROR!");
        }
    }

    private string GetBackgroundName(int i)
    {
        return i switch
        {
            0 => "BACKGROUND_1",
            1 => "BACKGROUND_2",
            2 => "BACKGROUND_3",
            3 => "BACKGROUND_4",
            _ => ""
        };
    }

    public void ChooseImgThemeBtn()
    {
        // Show file dialog to choose an image file
        string[] extensions = { "jpg", "jpg", "png", "png" };
        string path = UnityEditor.EditorUtility.OpenFilePanelWithFilters("Choose an image file", "", extensions);

        if (!string.IsNullOrEmpty(path))
        {
            themeImgFile = File.ReadAllBytes(path);
        }
    }
    public void ClickedBackBtn()
    {
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
