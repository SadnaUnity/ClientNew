using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HallScript : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text door0,door1,door2,door3,door4,door5;
    private HttpRequest httpRequest;
    private List<int> keys;
    private Vector3[] doorPositions; 
    private Dictionary<int, string> roomsForHall;
    private GameObject curPlayer;
    private Player playerData;
    private Dictionary<int, RoomStatus> roomStatuses;
    public popUpWindow popupWindow;
    private int roomToJoin;


    [SerializeField] private GameObject movingController;
    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerDataManager.PlayerData;
        // Create a background game objectGameObject background = new GameObject("Background");
        background.transform.SetParent(canvas.transform);
        // Attach a Sprite Renderer component to the background game object
        SpriteRenderer backroundspriteRenderer = background.AddComponent<SpriteRenderer>();

        // Set the background image as the sprite for the Sprite Renderer
        backroundspriteRenderer.sprite = Resources.Load<Sprite>("Images/backrounnds/1193"); 

        // Set the sorting layer of the background object to a lower value
        backroundspriteRenderer.sortingLayerName = "Background";
        backroundspriteRenderer.sortingOrder = -1;
        
        httpRequest = new HttpRequest();
        GetHall();
        
        MovingScript movingScript = movingController.GetComponent<MovingScript>();
        curPlayer = movingScript.GetCurPlayer();
        GetDoors();
    }

    private void GetHall()
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        var res = httpRequest.SendDataToServer(queryParams, "", "/hall", "GET");
        if (res.Item1 == 200)
        {
            roomStatuses = new Dictionary<int, RoomStatus>();
            RoomsDTO roomsDto = JsonConvert.DeserializeObject<RoomsDTO>(res.Item2);
            foreach (RoomStatusDTO roomStatusDto in roomsDto.roomStatuses)
            {
                roomStatuses.Add(roomStatusDto.roomId, new RoomStatus(roomStatusDto));
            }

            //roomStatuses[5].SetRoomMemberStatus(RoomMemberStatus.MEMBER);
        }
        else
        {
            Debug.Log("error in GetHall()");
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnterARoom();

    }    
    private void EnterARoom()
    {
        Boolean enteredARoom = false;
        int room = 1;
        
        //check if moved down the hall
        if (Vector3.Distance(curPlayer.transform.position,new Vector3(957,646,0))< 100f)
        {
            SceneManager.LoadScene("Moving");
        }
        //check if entered door0
        else if (Vector3.Distance(curPlayer.transform.position,doorPositions[0])< 100f)
        {
            enteredARoom = true;
            room = keys[0];

        }
        //check if entered door1
        else if (Vector3.Distance(curPlayer.transform.position,doorPositions[1])< 100f)
        {
            enteredARoom = true;
            room = keys[1];
        }
        //check if entered door2
        else if (Vector3.Distance(curPlayer.transform.position,doorPositions[2])< 100f)
        {
            enteredARoom = true;
            room = keys[2];
        }
        //check if entered door3
        else if (Vector3.Distance(curPlayer.transform.position,doorPositions[3])< 100f)
        {
            enteredARoom = true;
            room = keys[3];
        }
        //check if entered door4
        else if (Vector3.Distance(curPlayer.transform.position,doorPositions[4])< 100f)
        {
            enteredARoom = true;
            room = keys[4];
        }
        //check if entered door5
        else if (Vector3.Distance(curPlayer.transform.position,doorPositions[5])< 100f)
        {
            enteredARoom = true;
            room = keys[5];
        }

        if (enteredARoom)
        {
            
            if (IsRoomMember(room))
            {
                PlayerDataManager.PlayerData.SetRoomId(room);
                SceneManager.LoadScene("Room");
            }
            else
            {
               SendPosition(curPlayer.transform.position);
               
                popupWindow.ShowPopup();
                roomToJoin = room;
            }
            
        }
        
    }

    public void Door0Btn()
    {
        GetIntoRoom(keys[0]);
    }
    public void Door1Btn()
    {
        GetIntoRoom(keys[1]);
    }public void Door2Btn()
    {
        GetIntoRoom(keys[2]);
    }public void Door3Btn()
    {
        GetIntoRoom(keys[3]);
    }public void Door4Btn()
    {
        GetIntoRoom(keys[4]);
    }
    public void Door5Btn()
    {
        GetIntoRoom(keys[5]);
    }
    private void GetIntoRoom(int room)
    {
        if (IsRoomMember(room))
        {
            PlayerDataManager.PlayerData.SetRoomId(room);
            SceneManager.LoadScene("Room");
        }
        else
        {
            SendPosition(curPlayer.transform.position);
               
            popupWindow.ShowPopup();
            roomToJoin = room;
        }
    }
    

    public void membershipBtn()
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        var res = httpRequest.SendDataToServer(queryParams, "", $"/joinRoom/{roomToJoin}","POST"); 
        if (res.Item1 == 200)
        {
            Debug.Log("request was sent successfully");
            popupWindow.HidePopup();
        }
        else
        {
            Debug.Log("request error");
        }
    }

    private bool IsRoomMember(int roomId)
    {
        if (roomStatuses[roomId].GetRoomMemberStatus() == RoomMemberStatus.MEMBER)
            return true;
        else
        {
            return false;
        }
    }

    private void GetDoors()
    {
        //set doors positions
        doorPositions = new Vector3[]
        {
            new Vector3(1814, 528, 0), new Vector3(1527, 625, 0), new Vector3(1318, 625, 0), new Vector3(114, 568, 0),
            new Vector3(405, 629, 0), new Vector3(610, 645,0)
        };
        
        //get all rooms
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        var res = httpRequest.SendDataToServer(queryParams, "", "/hall", "GET");
        if (res.Item1 == 200)
        {
            RoomsDTO roomsDto = JsonConvert.DeserializeObject<RoomsDTO>(res.Item2);
            Rooms allRooms = new Rooms(roomsDto);
            roomsForHall = allRooms.getRoomsForHall();
            keys = new List<int>();
            foreach (var VARIABLE in roomsForHall.Keys)
            {
                keys.Add(VARIABLE);
            }
            //add room names to UI
            door0.text = roomsForHall[keys[0]].ToString();
            door1.text = roomsForHall[keys[1]].ToString();
            door2.text = roomsForHall[keys[2]].ToString();
            door3.text = roomsForHall[keys[3]].ToString();
            door4.text = roomsForHall[keys[4]].ToString();
            door5.text = roomsForHall[keys[5]].ToString();

        }

       

    }
    public void closePopUp()
    { 
         popupWindow.HidePopup();
    }
    private void SendPosition(Vector3 pos)
    {
        string jsonPos = JsonConvert.SerializeObject(new PositionDTO(pos.x + 100f, pos.y + 50f));
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        var res = httpRequest.SendDataToServer(queryParams, jsonPos, "/updatePosition", "POST");
    }
}
