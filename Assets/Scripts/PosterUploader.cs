using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;
using UnityEngine.EventSystems;

public class PosterUploader : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private HttpRequest httpRequest;
    private GameObject poster;
    
    public void Start()
    {
        httpRequest = new HttpRequest();
        poster = new GameObject("poster");
        poster.transform.SetParent(canvas.transform);
    }
    public void UploadPoster()
    {
        var imgFile = ChooseImg();
        if (imgFile != null) 
        {
            //var res = SendImg(imgFile);
        }
    }

    public void ChoosePosterPosition()
    {
        // Add a UI button to the scene that will call this method when clicked
        // In the inspector, drag and drop the "poster" game object onto the "Poster" field

        // Get the position handler script on the poster game object
        ImagePositionHandler positionHandler = poster.AddComponent<ImagePositionHandler>();

        // Enable the position handler script
        positionHandler.enabled = true;
    }

    private byte[] ChooseImg()
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
                return bytes;
            }
        }
        return null;
    }
    private Tuple<long, string> SendImg(byte[] imgFile)
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", 1),
            new("roomId", 1),
            new("posterName", "ere")
        };
        return httpRequest.SendDataToServer("posterrrname", 1, 1, "file", imgFile, "/poster");

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