using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Toggle = UnityEngine.UI.Toggle;

public class PosterUploader : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_InputField posterNameIf;
    [SerializeField] private Button uploadPosterBtn;
    [SerializeField] private Button choosePosterPositionBtn;
    [SerializeField] private Button lockPosterPositionBtn;
    [SerializeField] private Button sendPosterBtn;
    [SerializeField] private GameObject toggleGroup;
    [SerializeField] private Toggle togglePrefab;
    
    private Toggle activeToggle;
    private Dictionary<Toggle, int> toggleToPosterId;
    private HttpRequest httpRequest;
    private GameObject tmpPoster;
    private ImagePositionHandler imgPositionHandler;
    private byte[] posterImgFile;
    private Player playerData;
    private Dictionary<int, GameObject> posterIdToGameObject;
    private Dictionary<int, string> posterIdToPosterName;

    public void Start()
    {
        ChangeInteractable(1);
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        posterIdToGameObject = new Dictionary<int, GameObject>();
        posterIdToPosterName = new Dictionary<int, string>();
        GetRoomPosters();
        toggleToPosterId = new Dictionary<Toggle, int>();
    }

    private void ChangeInteractable(int i)
    {
        switch (i)
        {
            case 1:
                uploadPosterBtn.interactable = true;
                choosePosterPositionBtn.interactable = false;
                lockPosterPositionBtn.interactable = false;
                sendPosterBtn.interactable = false;
                posterNameIf.interactable = false;
                break;
            case 2:
                uploadPosterBtn.interactable = false;
                choosePosterPositionBtn.interactable = true;
                lockPosterPositionBtn.interactable = false;
                sendPosterBtn.interactable = false;
                posterNameIf.interactable = false;
                break;
            case 3:
                uploadPosterBtn.interactable = false;
                choosePosterPositionBtn.interactable = false;
                lockPosterPositionBtn.interactable = true;
                sendPosterBtn.interactable = false;
                posterNameIf.interactable = false;
                break;
            case 4:
                uploadPosterBtn.interactable = false;
                choosePosterPositionBtn.interactable = false;
                lockPosterPositionBtn.interactable = false;
                sendPosterBtn.interactable = true;
                posterNameIf.interactable = true;
                break;
            default:
                break;
        }
    }
    public void UploadPoster()
    {
        // Show file dialog to choose an image file
        string[] extensions = { "jpg", "jpg", "png", "png" };
        string path = UnityEditor.EditorUtility.OpenFilePanelWithFilters("Choose an image file", "", extensions);

        if (!string.IsNullOrEmpty(path))
        {
            byte[] bytes = File.ReadAllBytes(path);
            
            // Load the image from the path
            Texture2D texture = LoadTextureFromFile(path);

            if (texture != null)
            {
                tmpPoster = new GameObject();
                tmpPoster.transform.SetParent(canvas.transform);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                Image img = tmpPoster.AddComponent<Image>();
                img.sprite = sprite;
                tmpPoster.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                posterImgFile = bytes;
            }
        }
        ChangeInteractable(2);
    }
    public void ChoosePosterPosition()
    {
        // Get the position handler script on the poster game object
        imgPositionHandler = tmpPoster.AddComponent<ImagePositionHandler>();

        // Enable the position handler script
        imgPositionHandler.enabled = true;
        ChangeInteractable(3);
    }
    public void LockPosterPosition()
    {
        imgPositionHandler.enabled = false;
        ChangeInteractable(4);
    }
    public void SendPoster()
    {
        Vector2 posterPos = tmpPoster.transform.position;
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("posterName", posterNameIf.text),
            new("xPos", posterPos.x),
            new("yPos", posterPos.y),
            new("roomId", playerData.GetRoomId()),
            new("userId", playerData.GetUserId()),
        };
        var res = httpRequest.SendDataToServer(queryParams, "file", posterImgFile, "/poster");
        if (res.Item1 == 200)
        {
            PosterDataDTO posterDataDto = JsonConvert.DeserializeObject<PosterDataDTO>(res.Item2);
            tmpPoster.name = posterDataDto.poster.posterName;
            posterIdToGameObject.Add(posterDataDto.posterId, tmpPoster);
            posterIdToPosterName.Add(posterDataDto.posterId, posterDataDto.poster.posterName);
            posterNameIf.text = "";
            ChangeInteractable(1);
        }
        else
        {
            Debug.Log("Error uploading poster!");
        }
        
    }
    private Texture2D LoadTextureFromFile(string filePath)
    {
        Texture2D texture = null;

        if (!string.IsNullOrEmpty(filePath))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(filePath);
            texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture.LoadImage(bytes);
        }

        return texture;
    }
    public void ChoosePosterToDelete()
    {
        float buttonHeight = togglePrefab.GetComponent<RectTransform>().rect.height;
        float spacing = 5f; // Adjust the vertical spacing between buttons
        int count = 0;
    
        foreach (int key in posterIdToPosterName.Keys)
        {
            Toggle instantiatedToggle = Instantiate(togglePrefab, toggleGroup.transform);
            instantiatedToggle.transform.SetParent(toggleGroup.transform, false);
            instantiatedToggle.onValueChanged.AddListener(OnToggleValueChanged);
            Text toggleText = instantiatedToggle.GetComponentInChildren<Text>();
            toggleText.text = posterIdToPosterName[key];
            instantiatedToggle.isOn = false; // Set isOn property to false initially
            toggleToPosterId.Add(instantiatedToggle, key); // Modify dictionary entry

            RectTransform toggleRect = instantiatedToggle.GetComponent<RectTransform>();
            toggleRect.anchorMin = new Vector2(0f, 1f); // Set the anchor to top-left corner
            toggleRect.anchorMax = new Vector2(0f, 1f); // Set the anchor to top-left corner
            toggleRect.pivot = new Vector2(0f, 1f); // Set the pivot to top-left corner
            toggleRect.anchoredPosition = new Vector2(0f, -count * (buttonHeight + spacing));
            count++;
            toggleRect.sizeDelta = new Vector2(toggleRect.sizeDelta.x, buttonHeight);
        }
    }


    private void OnToggleValueChanged(bool isOn)
    {
        Toggle clickedToggle = EventSystem.current.currentSelectedGameObject.GetComponent<Toggle>();

        if (isOn)
        {
            foreach (Toggle toggle in toggleGroup.GetComponentsInChildren<Toggle>())
            {
                if (toggle != clickedToggle)
                {
                    toggle.isOn = false;
                }
            }
        }
    }
    
    public void DeletePoster()
    {
        int selectedPosterId = -1;
        foreach (Toggle toggle in toggleToPosterId.Keys)
        {
            if (toggle.isOn)
            { 
                selectedPosterId = toggleToPosterId[toggle];
                List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
                {
                    new("userId", PlayerDataManager.PlayerData.GetUserId())
                };
                var res = httpRequest.SendDataToServer(queryParams, "", $"/deletePoster/{selectedPosterId}", "POST");
                if (res.Item1 == 200)
                {
                    Debug.Log("Poster was deleted successfully");
                }
                else if (res.Item1 == 401)
                {
                    Debug.Log("User is not allowed to delete poster!");
                }
                else
                {
                    Debug.Log("Error deleting poster!");
                }
                break;
            }
        }
        DeletePosterFromRoom(selectedPosterId);
        DestroyAllToggles();
    }

    private void DestroyAllToggles()
    {
        foreach (Toggle toggle in toggleToPosterId.Keys)
        {
            Destroy(toggle.gameObject);
        }
        toggleToPosterId.Clear();
    }


    private void DeletePosterFromRoom(int selectedPosterId)
    {
        GameObject posterGoToDelete = posterIdToGameObject[selectedPosterId];
    
        // Check if the GameObject exists
        if (posterGoToDelete != null)
        {
            // Destroy the GameObject
            Destroy(posterGoToDelete);
        
            // Remove the entry from the dictionary
            posterIdToGameObject.Remove(selectedPosterId);
            posterIdToPosterName.Remove(selectedPosterId);
        }
    }



    private void GetRoomPosters()
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("roomId", playerData.GetRoomId()),
            new("userId", playerData.GetUserId())
        };
        string rsc = $"/room/{playerData.GetRoomId()}";
        var res = httpRequest.SendDataToServer(queryParams, "", rsc, "GET"); 
        if (res.Item1 == 200)
        {
            RoomDataDTO roomDataDto = JsonConvert.DeserializeObject<RoomDataDTO>(res.Item2);
            List<PosterDTO> postersDto = roomDataDto.room.posters;
            foreach (var posterDto in postersDto)
            {
                // Load the image from the URL and set it as the sprite for the SpriteRenderer
                StartCoroutine(LoadImageFromURL(posterDto.fileUrl,new Vector3(posterDto.position.x, posterDto.position.y, 0), posterDto));
            } 
        }
        else
        {
            Debug.Log("Error get room id: " + playerData.GetRoomId());
        }
    }
    IEnumerator LoadImageFromURL(string url, Vector3 position, PosterDTO posterDto)
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

        GameObject poster = new GameObject(posterDto.posterId.ToString());
        poster.transform.position = position;
        poster.transform.localScale = new Vector3(20f, 20f, 20f);
        SpriteRenderer spriteRenderer = poster.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        
        posterIdToGameObject.Add(posterDto.posterId, poster);
        posterIdToPosterName.Add(posterDto.posterId, posterDto.posterName);
        
    }

}
public class ImagePositionHandler : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
   
}