using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class PosterUploader : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TMP_InputField posterNameIf;
    [SerializeField] private Button uploadPosterBtn;
    [SerializeField] private Button choosePosterPositionBtn;
    [SerializeField] private Button lockPosterPositionBtn;
    [SerializeField] private Button sendPosterBtn;
    
    private HttpRequest httpRequest;
    private GameObject poster;
    private ImagePositionHandler imgPositionHandler;
    private byte[] posterImgFile;
    private Player playerData;
    
    public void Start()
    {
        ChangeInteractable(1);
        httpRequest = new HttpRequest();
        poster = new GameObject("poster");
        poster.transform.SetParent(canvas.transform);
        playerData = PlayerDataManager.PlayerData;
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
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                Image img = poster.AddComponent<Image>();
                img.sprite = sprite;
                poster.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                posterImgFile = bytes;
            }
        }
        ChangeInteractable(2);
    }

    public void ChoosePosterPosition()
    {
        // Get the position handler script on the poster game object
        imgPositionHandler = poster.AddComponent<ImagePositionHandler>();

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
        Vector2 posterPos = poster.transform.position;
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId()),
            new("roomId", 1),
            new("posterName", posterNameIf.text),
            new("xPos", posterPos.x),
            new("yPos", posterPos.y)
        };
        var res = httpRequest.SendDataToServer(queryParams, "file", posterImgFile, "/poster");
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
}
public class ImagePositionHandler : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
   
}