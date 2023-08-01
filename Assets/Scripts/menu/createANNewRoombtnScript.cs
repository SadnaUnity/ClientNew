using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class createANNewRoombtnScript : MonoBehaviour
{
    private HttpRequest httpRequest;
    private Player playerData;
    // Start is called before the first frame update
    void Start()
    {
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
    }

    public void ClickedBtn()
    {
        SceneManager.LoadScene("CreateANewRoom");
    }
    
}
