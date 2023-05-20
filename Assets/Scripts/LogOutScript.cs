using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogOutScript : MonoBehaviour
{
    private HttpRequest httpRequest;
    private int userId;
    public void Start()
    {
        httpRequest = new HttpRequest();
        userId = PlayerDataManager.PlayerData.GetUserId();
    }

    public void Logout()
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", userId),
        };
        var res = httpRequest.SendDataToServer(queryParams, "", "/logout", "POST");
        
        if (res.Item1 == 200)
        {
            SceneManager.LoadScene("First");
        }
        else
        {
            Debug.Log("Error with logout, code " + res.Item1);
        }
    }
}
