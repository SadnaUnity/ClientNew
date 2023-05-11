using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.EventSystems;
using Button = UnityEngine.UIElements.Button;

public class PosterUploader : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_InputField posterNameIF;
    private HttpRequest httpRequest;
    private GameObject poster;
    private ImagePositionHandler imgPositionHandler;
    private byte[] posterImgFile;
    private Player playerData;
    
    public void Start()
    {
        panel.SetActive(false);
        httpRequest = new HttpRequest();
        poster = new GameObject("poster");
        poster.transform.SetParent(canvas.transform);
        playerData = PlayerDataManager.PlayerData;
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
    }

    public void ChoosePosterPosition()
    {
        // Get the position handler script on the poster game object
        imgPositionHandler = poster.AddComponent<ImagePositionHandler>();

        // Enable the position handler script
        imgPositionHandler.enabled = true;
    }
    public void LockPosterPosition()
    {
        imgPositionHandler.enabled = false;
        panel.SetActive(true);
    }

    public void SendPoster()
    {
        panel.SetActive(false);
        Vector3 posterPos = poster.transform.position;
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId()),
            new("roomId", 1),
            new("posterName", posterNameIF.text),
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