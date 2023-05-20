using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameTF;
    [SerializeField] private TMP_InputField passwordTF;
    [SerializeField] private TMP_InputField confirmPasswordTF;
    [SerializeField] private TMP_Text errorText;
    
    public Image avatar;
    public AvatarAccessory accessory;
    public AvatarColor color;
    void Start()
    {
        AvatarImage();        
    }

    public void ClickedNext()
    {
        if (usernameTF.text == "")
        {
            errorText.text = "username filed is empty!";
            ClearFields();
            return;
        }
        if (passwordTF.text == "" || confirmPasswordTF.text == "")
        {
            errorText.text = "password filed is empty!";
            ClearFields();
            return;
        }
        if (passwordTF.text != confirmPasswordTF.text)
        {
            errorText.text = "Passwords not equal!";
            ClearFields();
            return;
        }
        

        HttpRequest httpRequest = new HttpRequest();
        List<KeyValuePair<string, object>> queryParameters = new List<KeyValuePair<string, object>>
        {
            new("username", usernameTF.text),
            new("password", passwordTF.text),
            new("avatarColor", PlayerPrefs.GetInt("avatarColor")),
            new("avatarAccessory", PlayerPrefs.GetInt("avatarAccessory"))
        };

        var res = httpRequest.SendDataToServer(queryParameters, "", "/register", "POST");

        if (res.Item1 == 200)
        {
            PlayerDTO playerDto = JsonConvert.DeserializeObject<PlayerDTO>(res.Item2);
            PlayerDataManager.PlayerData = new Player(playerDto);
            Debug.Log("Register success");
            SceneManager.LoadScene("Moving");
        }
        else
        {
            PlayerDTO playerDto = JsonConvert.DeserializeObject<PlayerDTO>(res.Item2);
            errorText.text = playerDto.message;
            ClearFields();
        }
    }

    public void ClickedBack()
    {
        SceneManager.LoadScene("ChooseAccessory");
    }

    private void ClearFields()
    {
        usernameTF.text = "";
        passwordTF.text = "";
        confirmPasswordTF.text = "";
    }
    
    private void AvatarImage()
    {
        color = (AvatarColor)PlayerPrefs.GetInt("avatarColor");
        accessory = (AvatarAccessory)PlayerPrefs.GetInt("avatarAccessory");
        string imgUrl = "Images/Final Avatars/";

        switch (color)
        {
            case AvatarColor.BLUE:
                imgUrl += "blue";
                break;

            case AvatarColor.PINK:
                imgUrl += "pink";
                break;

            case AvatarColor.GREEN:
                imgUrl += "green";
                break;

            case AvatarColor.YELLOW:
                imgUrl += "yellow";
                break;
        }

        switch (accessory)
        {
            case AvatarAccessory.EMPTY:
                break;

            case AvatarAccessory.COOK_HAT:
                imgUrl += "Chef";
                break;

            case AvatarAccessory.SANTA_HAT:
                imgUrl += "Santa";
                break;

            case AvatarAccessory.HEART_GLASSES:
                imgUrl += "Hart";
                break;
            case AvatarAccessory.NORMAL_GLASSES:
                imgUrl += "Glass";
                break;
        }

        avatar.sprite = Resources.Load<Sprite>(imgUrl);
    }

}
