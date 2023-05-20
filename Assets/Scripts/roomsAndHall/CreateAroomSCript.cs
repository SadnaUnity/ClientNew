using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAroomSCript : MonoBehaviour
{
    [SerializeField] private TMP_InputField RoomName;

    [SerializeField] private TMP_Dropdown Capacity;

    [SerializeField] private Toggle Private;
   
    private HttpRequest httpRequest;
    private Player playerData;
    
    // Start is called before the first frame update
    void Start()
    {   
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        int i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickedBtn()
    {
     
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("managerId", playerData.GetUserId()),
            new("roomName", RoomName.text),
            //new("privacy", Private.isOn)


            
        };
        var res = httpRequest.SendDataToServer(queryParams, "", "/room", "POST");
        if (res.Item1 == 200)
        {
            RoomStatusDTO roomStatusDto = JsonConvert.DeserializeObject<RoomStatusDTO>(res.Item2);
            PlayerDataManager.PlayerData.SetRoomId(roomStatusDto.roomId);
            SceneManager.LoadScene("Room");

        }
        else
        {
            Debug.Log("error");
        }

    }
    
}
