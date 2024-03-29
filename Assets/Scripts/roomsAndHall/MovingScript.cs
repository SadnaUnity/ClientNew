using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Classes.DTO;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MovingScript : MonoBehaviour
{
    private string rsc;
    private Vector3 mousePosition;
    public float speed;
    private HttpRequest httpReq;
    private Player playerData;
    private int playerId;
    private GameObject curPlayer;
    private Dictionary<int, GameObject> playersById;
    private Dictionary<int, bool> playersOnScreen;
    private float avatarSize = 4.25f;
    
    // Start is called before the first frame update
    void Start()
    {
        rsc = "/updatePosition";
        speed = 1000f;
        httpReq = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        playerId = playerData.GetUserId();
        curPlayer = new GameObject(playerData.GetUserId().ToString());

        playersById = new Dictionary<int, GameObject>()
        {
            { playerId, curPlayer }
        };

        playersOnScreen = new Dictionary<int, bool>()
        {
            { playerId, true }
        };
        
        // Add a SpriteRenderer component to the new GameObject
        SpriteRenderer spriteRenderer = curPlayer.AddComponent<SpriteRenderer>();
        
        // Assign the sprite to the SpriteRenderer component and load img
        spriteRenderer.sprite = Resources.Load<Sprite>(GetAvatarPath(playerData.GetAvatar()));

        spriteRenderer.transform.localScale = new Vector3(avatarSize, avatarSize, avatarSize);

        curPlayer.transform.position = new Vector3(957, 90, 0);  
        spriteRenderer.sortingOrder = 1;

        mousePosition = curPlayer.transform.position;

        SendPosition(curPlayer.transform.position);
        // Start coroutine to get other players' positions every second

    }

    // Update is called once per frame
    void Update()
    {
       MovePlayer();
       GetOtherPlayersPositions();
       // Debug.Log(curPlayer.transform.position);

    }

    public void MovePlayer()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(1))
        {
            // Get the position of the mouse click in world coordinates
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f; // Set z-axis to 0 to ensure the image remains in 2D space

            // Send a POST request to update the player's position on the server
            SendPosition(mousePosition);
        }
       
        // Move the player towards the mouse position
        curPlayer.transform.position = Vector3.MoveTowards(curPlayer.transform.position, mousePosition, speed * Time.deltaTime);

    }

    public void SetMousePosition(Vector3 pos)
    {
        mousePosition = new Vector3(pos.x, pos.y, pos.z);
    }
    void LateUpdate()
    {
        // Update the camera's position to follow the curPlayerTransform
        Vector3 newPosition = new Vector3(curPlayer.transform.position.x, curPlayer.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
    }

    private void SendPosition(Vector3 pos)
    {
        string jsonPos = JsonConvert.SerializeObject(new PositionDTO(pos.x, pos.y));
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerId)
        };
        var res = httpReq.SendDataToServer(queryParams, jsonPos, rsc, "POST");
    }
    private void GetOtherPlayersPositions()
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerId)
        };
        var res = httpReq.SendDataToServer(queryParams, "", "/getPositions", "GET");
        if (res.Item1 == 200)
        {
            // Parse the JSON response into a list of players
            PosDataDTO playersPositions = JsonConvert.DeserializeObject<PosDataDTO>(res.Item2);
            List<Tuple<Avatar, Position>> avatarPositions = GetAllAvatarsPositions(playersPositions);

            InitPlayerOnScreen();
            
            // Update the positions of all other players in the game scene
            foreach (Tuple<Avatar, Position> avatarPosition in avatarPositions)
            {
                int id = avatarPosition.Item1.GetId();
                playersOnScreen[id] = true;
                if (id != playerId)
                {
                    //If not on screen yet
                    if (!playersById.ContainsKey(id))
                    {
                        playersById.Add(id, CreateGameObject(avatarPosition));
                    }
                    
                    //Already on screen - move him slowly towards target
                    else
                    {
                        Vector3 targetPosition = new Vector3(avatarPosition.Item2.GetX(), avatarPosition.Item2.GetY(),0f);
                        while (playersById[id].transform.position != targetPosition)
                        {
                            playersById[id].transform.position = Vector3.MoveTowards(playersById[id].transform.position, targetPosition, speed * Time.deltaTime);
                            
                        }
                    }
                }
            }

            RemoveLogoutPlayerFromScreen();
        }
        else
        {
            Debug.Log("GetPositions FAIL!: " + res.Item2);
        }

    }

    private void RemoveLogoutPlayerFromScreen()
    {
        List<int> idsToRemove = new List<int>();
        foreach (int key in playersOnScreen.Keys)
        {
            if (!playersOnScreen[key])
            {
                GameObject playerToDelete = playersById[key];
                Destroy(playerToDelete);
                playersById.Remove(key);
                idsToRemove.Add(key);
            }
        }

        foreach (var id in idsToRemove)
        {
            playersOnScreen.Remove(id);
        }
        
    }

    private void InitPlayerOnScreen()
    {
        for (int i = 0; i < playersOnScreen.Count; i++)
        {
            var key = playersOnScreen.Keys.ElementAt(i);
            playersOnScreen[key] = false;
        }

    }

    private List<Tuple<Avatar, Position>> GetAllAvatarsPositions(PosDataDTO playersPositions)
    {
        List<Tuple<Avatar, Position>> res = new List<Tuple<Avatar, Position>>();
        List<AvatarPositionDTO> avatarPositions = playersPositions.avatarPositions;

        foreach (AvatarPositionDTO avatarPosition in avatarPositions)
        {
            res.Add(new Tuple<Avatar, Position>(new Avatar(avatarPosition.avatar), new Position(avatarPosition.position)));
        }

        return res;
    }
    private GameObject CreateGameObject(Tuple<Avatar, Position> avatarPosition)
    {
        GameObject character = new GameObject(avatarPosition.Item1.GetId().ToString());

        // Add a SpriteRenderer component to the new GameObject
        SpriteRenderer spriteRenderer = character.AddComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = 1;
        // Assign the sprite to the SpriteRenderer component and load img
        spriteRenderer.sprite = Resources.Load<Sprite>(GetAvatarPath(avatarPosition.Item1));

        spriteRenderer.transform.localScale = new Vector3(avatarSize, avatarSize, avatarSize);
        
        character.transform.position = new Vector3(avatarPosition.Item2.GetX(), avatarPosition.Item2.GetY());
        
        return character;
    }
    private string GetAvatarPath(Avatar avatar)
    {
        AvatarColor avatarColor = avatar.GetColor();
        AvatarAccessory avatarAccessory = avatar.GetAccessory();
        string path = "Images/Final Avatars";

        switch (avatarColor)
        {
            case AvatarColor.BLUE:
                path += "/blue";
                break;
            case AvatarColor.GREEN:
                path += "/green";
                break;
            case AvatarColor.PINK:
                path += "/pink";
                break;
            case AvatarColor.YELLOW:
                path += "/yellow";
                break;
            default:
                Debug.Log("Error with color");
                break;
        }

        switch (avatarAccessory)
        {
            case AvatarAccessory.COOK_HAT:
                path += "Chef";
                break;
            case AvatarAccessory.HEART_GLASSES:
                path += "Hart";
                break;
            case AvatarAccessory.NORMAL_GLASSES:
                path += "Glass";
                break;
            case AvatarAccessory.SANTA_HAT:
                path += "Santa";
                break;
            case AvatarAccessory.EMPTY:
                break;
            default:
                Debug.Log("Error with accessory");
                break;
        }

        return path;
    }

    public GameObject GetCurPlayer()
    {
        return curPlayer;
    }
    
}
