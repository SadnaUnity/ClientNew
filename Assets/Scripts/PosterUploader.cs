using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class PosterUploader : MonoBehaviour
{
    [SerializeField] private Image image;
    private HttpRequest httpRequest;
    public void Start()
    {
        httpRequest = new HttpRequest();
    }
    public void OnButtonClick()
    {
        // Show file dialog to choose an image file
        string[] extensions = { "jpg", "jpg", "png", "png" };
        string path = UnityEditor.EditorUtility.OpenFilePanelWithFilters("Choose an image file", "", extensions);

        if (!string.IsNullOrEmpty(path))
        {
            List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
            {
                new("userId", 1),
                new("roomId", 1),
                new("posterName", "ere")
            };
            // Load image from file and set it to the Image component
            byte[] bytes = File.ReadAllBytes(path);
            var res = httpRequest.SendDataToServer("posterrrname", 1, 1, "file", bytes, "/poster");
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;
        }
    }
}