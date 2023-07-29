using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HallScript : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text[] doorSigns;
    [SerializeField] private Image[] themeImages;
    
    private HttpRequest httpRequest;
    private List<int> keys;
    private Vector3[] doorPositions; 
    private Dictionary<int, Tuple<string, string>> roomsForHall;
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
        httpRequest = new HttpRequest();
        GetHall();
        //LoadBackground()
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
            HallDTO hallDTO = JsonConvert.DeserializeObject<HallDTO>(res.Item2);
            foreach (RoomStatusDTO roomStatusDto in hallDTO.roomStatuses)
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
                Vector3 pos = new Vector3(956f, 185f, 0);
                curPlayer.transform.position = pos;
                MovingScript movingScript = movingController.GetComponent<MovingScript>();
                movingScript.SetMousePosition(pos);
                SendPosition(pos);
                popupWindow.ShowPopup();
                roomToJoin = room;
            }
            
        }
        
    }

    /*public void Door0Btn()
    {
        GetIntoRoom(keys[0]);
    }
    public void Door1Btn()
    {
        GetIntoRoom(keys[1]);
    }
    public void Door2Btn()
    {
        GetIntoRoom(keys[2]);
    }
    public void Door3Btn()
    {
        GetIntoRoom(keys[3]);
    }
    public void Door4Btn()
    {
        GetIntoRoom(keys[4]);
    }
    public void Door5Btn()
    {
        GetIntoRoom(keys[5]);
    }*/
    
    public void GetIntoRoom(int room)
    {
        if (IsRoomMember(room))
        {
            PlayerDataManager.PlayerData.SetRoomId(room);
            SceneManager.LoadScene("Room");
        }
        else
        {
            curPlayer.transform.position = new Vector3(956f, 185f, 0);
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
            HallDTO hallDto = JsonConvert.DeserializeObject<HallDTO>(res.Item2);
            Hall hall = new Hall(hallDto);
            roomsForHall = hall.getRoomsForHall();
            keys = new List<int>();
            foreach (var roomId in roomsForHall.Keys)
            {
                keys.Add(roomId);
            }
            //add room names and images to UI
            for (int i = 0; i < doorSigns.Length; i++)
            {
                doorSigns[i].text = roomsForHall[keys[i]].Item1;
                string imgUrl = roomsForHall[keys[i]].Item2;
                if (imgUrl != null)
                {
                    themeImages[i].gameObject.SetActive(true);
                    StartCoroutine(LoadImageCoroutine(imgUrl, i));
                }
                
            }
            
        }
    }
    private IEnumerator LoadImageCoroutine(string imgUrl, int index)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imgUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                if (texture != null)
                {
                    themeImages[index].sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                }
                else
                {
                    Debug.LogError("Failed to load the image from URL: " + imgUrl);
                }
            }
            else
            {
                Debug.LogError("Failed to load image from URL: " + imgUrl + "\nError: " + www.error);
            }
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

    private void LoadBackground()
    {
        // Create a background game objectGameObject background = new GameObject("Background");
        background.transform.SetParent(canvas.transform);
        // Attach a Sprite Renderer component to the background game object
        SpriteRenderer backroundspriteRenderer = background.AddComponent<SpriteRenderer>();

        // Set the background image as the sprite for the Sprite Renderer
        backroundspriteRenderer.sprite = Resources.Load<Sprite>("Images/backrounnds/1193"); 

        // Set the sorting layer of the background object to a lower value
        backroundspriteRenderer.sortingLayerName = "Background";
        backroundspriteRenderer.sortingOrder = -1;
    }
}
