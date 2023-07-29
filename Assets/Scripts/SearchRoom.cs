using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class SearchRoom : MonoBehaviour
{
    private HttpRequest httpRequest;
    private Player playerData;
    
    [SerializeField] private TMP_InputField roomInputField;
    [SerializeField] private TMP_Text errorMsg;
    [SerializeField] private GameObject hallController;

    void Start()
    {
        playerData = PlayerDataManager.PlayerData;
        httpRequest = new HttpRequest();
    }

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
            HallDTO hallDTO = JsonConvert.DeserializeObject<HallDTO>(res.Item2);
            Dictionary<int, RoomStatus> allRooms = new Hall(hallDTO).GetAllRooms();
            string roomName = roomInputField.text;
            roomInputField.text = "";
            GetIntoRoomIfExists(roomName, allRooms);
        }
    }

    private void GetIntoRoomIfExists(string roomName, Dictionary<int, RoomStatus> allRooms)
    {
        int roomId = FindRoomId(roomName, allRooms);
        if (roomId != -1)
        {
            HallScript hallScript = hallController.GetComponent<HallScript>();
            hallScript.GetIntoRoom(roomId);
        }
        else
        {
            errorMsg.text = "Room Does Not Exists!";
        }
    }

    private int FindRoomId(string roomName, Dictionary<int, RoomStatus> allRooms)
    {
        foreach (int roomId in allRooms.Keys)
        {
            if (allRooms[roomId].GetRoomName().Equals(roomName))
                return roomId;
        }

        return -1;
    }
    
}

