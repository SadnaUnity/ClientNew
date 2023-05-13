using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHallScript : MonoBehaviour
{
    private Player player;
    private HttpRequest httpRequest;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerDataManager.PlayerData;
    }

    public void ClickedBtn()
    {
        httpRequest = new HttpRequest();
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", player.GetUserId())
        };
        httpRequest.SendDataToServer(queryParams, "", "/getOutFromRoom", "POST");
        SceneManager.LoadScene("Moving");

    }
}
