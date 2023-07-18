using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class SearchRoom : MonoBehaviour
{
    private HttpRequest httpRequest;
    private Player playerData;
    [SerializeField] private TMP_Text errorTxt;
    [SerializeField] private TMP_InputField roomInputField;

    // Start is called before the first frame update
    public void EnterRoom()
    {
        //get all rooms
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        var res = httpRequest.SendDataToServer(queryParams, "", "/hall", "GET");
        if (res.Item1 == 200)
        {
            RoomsDTO roomsDto = JsonConvert.DeserializeObject<RoomsDTO>(res.Item2);
            Dictionary<int, RoomStatus> allRooms = new Rooms(roomsDto).GetAllRooms();
            string roomName = roomInputField.text;
            roomInputField.text = "";
            int roomId = GetRoomIdIfExists(roomName);

        }
    }
}

