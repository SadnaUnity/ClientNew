using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

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
    
    // Start is called before the first frame update
    void Start()
    {
        rsc = "/updatePosition";
        speed = 1000f;
        httpReq = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        playerId = playerData.GetId();
        curPlayer = new GameObject(playerData.GetId().ToString());
        playersById = new Dictionary<int, GameObject>()
        {
            {playerId, curPlayer}
        };

        // Add a SpriteRenderer component to the new GameObject
        SpriteRenderer spriteRenderer = curPlayer.AddComponent<SpriteRenderer>();
        
        // Assign the sprite to the SpriteRenderer component and load img
        spriteRenderer.sprite = Resources.Load<Sprite>(GetAvatarPath(playerData.GetAvatar()));

        spriteRenderer.transform.localScale = new Vector3(8f, 8f, 4f);

        curPlayer.transform.position = new Vector3(0, 0, 0);  
        mousePosition = curPlayer.transform.position;

        SendPosition(curPlayer.transform.position);
        // Start coroutine to get other players' positions every second
        StartCoroutine(GetOtherPlayersPositions());
    }

    // Update is called once per frame
    void Update()
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

    private void SendPosition(Vector3 pos)
    {
        PosDataDTO posDataDto = new PosDataDTO(new PlayerDTO(playerData), new PositionDTO(playerId, pos.x, pos.y));
        string jsonPos = JsonConvert.SerializeObject(posDataDto);
        var res = httpReq.SendDataToServer(null, jsonPos, rsc, "POST");
        
    }
    IEnumerator GetOtherPlayersPositions()
    {
        while (true)
        {
            var res = httpReq.SendDataToServer(null, "", "/getPositions", "GET");
            if (res.Item1 == 200)
            {
                // Parse the JSON response into a list of players
                var playersPositions = JsonConvert.DeserializeObject<Dictionary<string, PosDataDTO>>(res.Item2);

                // Update the positions of all other players in the game scene
                foreach (string idStr in playersPositions.Keys)
                {
                    int id = int.Parse(idStr);
                    if (id != playerId)
                    {
                        //If not on screen yet
                        if (!playersById.ContainsKey(id))
                        {
                            playersById.Add(id, CreateGameObject(playersPositions[idStr]));
                        }
                        //Already on screen - move him slowly towards target
                        else
                        {
                            Position positionData = new Position(playersPositions[idStr].positionDto);
                            Vector3 targetPosition = new Vector3(positionData.GetX(), positionData.GetY(), 0f);
                            while (playersById[id].transform.position != targetPosition)
                            {
                                playersById[id].transform.position = Vector3.MoveTowards(playersById[id].transform.position, targetPosition, speed * Time.deltaTime);
                                yield return null;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("GET request failed: " + res.Item2);
            }

            // Wait for one second before sending another GET request
            yield return new WaitForSeconds(0.5f);
        }
    }


    private GameObject CreateGameObject(PosDataDTO posDataDto)
    {
        GameObject character = new GameObject(posDataDto.playerDto.userId.ToString());

        // Add a SpriteRenderer component to the new GameObject
        SpriteRenderer spriteRenderer = character.AddComponent<SpriteRenderer>();
        
        // Assign the sprite to the SpriteRenderer component and load img
        spriteRenderer.sprite = Resources.Load<Sprite>(GetAvatarPath(new Avatar(posDataDto.playerDto.avatar)));

        spriteRenderer.transform.localScale = new Vector3(8f, 8f, 4f);
        
        PositionDTO positionDto = posDataDto.positionDto;
        character.transform.position = new Vector3(positionDto.x, positionDto.y);
        
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
}
