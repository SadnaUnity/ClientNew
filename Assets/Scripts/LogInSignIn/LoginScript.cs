using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

using TMPro;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LoginScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TMP_Text errorText;
    
    private string rsc;
    private HttpRequest httpReq;
    public void Start()
    {
        rsc = "/login";
        httpReq = new HttpRequest();
    }

    public void Login()
    {
        string usernameData = this.username.text;
        string passwordData = this.password.text;

        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("username", usernameData),
            new("password", passwordData)
        };

        var res = httpReq.SendDataToServer(queryParams, "", rsc, "POST");

        if(res.Item1 == 200)
        {
            PlayerDTO playerDto = JsonConvert.DeserializeObject<PlayerDTO>(res.Item2);
            PlayerDataManager.PlayerData = new Player(playerDto);
            Debug.Log("Login success");
            SceneManager.LoadScene("Moving");
        }
        else
        {
            PlayerDTO playerDto = JsonConvert.DeserializeObject<PlayerDTO>(res.Item2);
            errorText.text = playerDto.message;
            ClearFields();
        }
    }
    private void ClearFields()
    {
        username.text = "";
        password.text = "";
    }
    public void ClickedBack(){ 
        SceneManager.LoadScene("First");
    }

    
}

