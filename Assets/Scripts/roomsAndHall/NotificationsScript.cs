using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class NotificationsScript : MonoBehaviour
{
    
    private HttpRequest httpRequest;
    private Player playerData;
    [SerializeField] private GameObject popUpWindow;
    [SerializeField] private TMP_Text textObject1;
    [SerializeField] private TMP_Text textObject2;
    [SerializeField] private GameObject newNotificationAlert;

    private RoomRequests roomRequests;
    private bool popUpIsOn = false;

    // Start is called before the first frame update
    void Start()
    {
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        StartCoroutine(CallNotificationsFunctionRepeatedly());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickedBtn()
{
    if (popUpIsOn)
    {
        popUpWindow.SetActive(false);
        popUpIsOn = false;
        TMP_Text[] messageTextObjects = popUpWindow.GetComponentsInChildren<TMP_Text>();
        foreach (TMP_Text messageTextObject in messageTextObjects)
        {
            Destroy(messageTextObject.gameObject);
        }
    }
    else
    {
        popUpWindow.SetActive(true);
        popUpIsOn = true;
        getNotifications();

    }
}

    private void getNotifications()
    {
         List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("managerId", playerData.GetUserId())
        };
        var res = httpRequest.SendDataToServer(queryParams, "", "/waitingJoinRoomRequests", "GET");
        int y = 0;

        if (res.Item1 == 200)
        {
            RequstesDTO requstesDto = JsonConvert.DeserializeObject<RequstesDTO>(res.Item2);
            if (requstesDto.joinRoomRequests != null && requstesDto.joinRoomRequests.Count > 0)
            {
                roomRequests = new RoomRequests(requstesDto);
                for (int i = 0; i < requstesDto.joinRoomRequests.Count; i++)
                {
                    var requst = requstesDto.joinRoomRequests[i];

                    TMP_Text newMsg = Instantiate(textObject1, popUpWindow.transform);
                    newMsg.text = requst.username.ToString() + " Wants to be a member in room " +
                                  requst.roomId.ToString();
                    newMsg.fontSize = 18;

                    // Set the position of the new text object within the popup window
                    RectTransform newMsgRectTransform = newMsg.GetComponent<RectTransform>();
                    newMsgRectTransform.anchoredPosition = new Vector2(1458f, 521f + y);
                    // Create local variables for the button listeners
                    
                    Button[] buttons = newMsg.GetComponentsInChildren<Button>();
                    foreach (Button button in buttons)
                    {
                        if (button.tag == "accept")
                        {
                            button.onClick.AddListener(() => ApproveOrDeclien(requst,"APPROVED"));
                            
                        }
                        else if (button.tag =="decline")
                        {
                            button.onClick.AddListener(() => ApproveOrDeclien(requst,"DECLINED"));

                        }

                    }
                   

                  y += 100;
                }
            }
        }

        List<KeyValuePair<string, object>> queryParams2 = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        var res2 = httpRequest.SendDataToServer(queryParams2, "", "/completedRequests", "GET");
        if (res2.Item1 == 200)
        {
            RequstesDTO requstesDto = JsonConvert.DeserializeObject<RequstesDTO>(res2.Item2);
            if (requstesDto.joinRoomRequests != null)
            {
                roomRequests = new RoomRequests(requstesDto);
                for (int i = 0; i < requstesDto.joinRoomRequests.Count; i++)
                {
                    var requst = requstesDto.joinRoomRequests[i];

                    TMP_Text newMsg2 = Instantiate(textObject2, popUpWindow.transform);
                    newMsg2.text = "your request to join room " + requst.roomId + " has been " + requst.requestStatus;
                    newMsg2.fontSize = 18;

                    // Set the position of the new text object within the popup window
                    RectTransform newMsgRectTransform = newMsg2.GetComponent<RectTransform>();
                    newMsgRectTransform.anchoredPosition = new Vector2(1010f, 521f + y);
                    
                    Button seenButton = newMsg2.GetComponentInChildren<Button>();
                    seenButton.onClick.AddListener(() => seenRequest(i));
                    
                    y += 100;
                }
            }
        }
    }

    private void ApproveOrDeclien(JoinRoomReqDTO request, string str)
    {
        JoinRoomReq requestToApprove = new JoinRoomReq(request);
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("managerId", playerData.GetUserId())
        };
       
        requestToApprove.SetStatus(str);
            
        List<JoinRoomReqDTO> body = new List<JoinRoomReqDTO>();
        body.Add(new JoinRoomReqDTO(requestToApprove));
        //Dictionary<string, object> jsonBody = body.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        string bodyString = JsonConvert.SerializeObject(body, Formatting.Indented);

        var res = httpRequest.SendDataToServer(queryParams, bodyString, "/handlePendingJoinRequests", "POST");
        if (res.Item1 == 200)
        {
            Debug.Log("success");
            clickedBtn();
            clickedBtn();
        }
    }

  

    private void seenRequest(int index)
    {
        JoinRoomReq requestToApprove = roomRequests.GetJoinRoonmReq()[index - 1];

        List<JoinRoomReqDTO> approvedRequests = new List<JoinRoomReqDTO>();
        approvedRequests.Add(new JoinRoomReqDTO(requestToApprove));

        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
    
        string bodyString = JsonConvert.SerializeObject(approvedRequests, Formatting.Indented);
    
        var res = httpRequest.SendDataToServer(queryParams, bodyString, "/approveRequest", "POST");
        if (res.Item1 == 200)
        {
            Debug.Log("Completed request seen and approved.");
            clickedBtn();
            clickedBtn();        }
        else
        {
            Debug.LogError("Error while trying to approve completed request.");
        }

        SceneManager.LoadScene("Moving");
    }

    private IEnumerator CallNotificationsFunctionRepeatedly()
    {
        while (true) // This will keep the coroutine running indefinitely
        {
            // Call your function here
            NewNotification();

            // Wait for 5 seconds
            yield return new WaitForSeconds(5f);
        }
    }

    private void NewNotification()
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("managerId", playerData.GetUserId())
        };
        var res = httpRequest.SendDataToServer(queryParams, "", "/waitingJoinRoomRequests", "GET");

        
        
        List<KeyValuePair<string, object>> queryParams2 = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId())
        };
        var res2 = httpRequest.SendDataToServer(queryParams2, "", "/completedRequests", "GET");
        if (res2.Item1 == 200 &&  res.Item1 == 200)
        {
            RequstesDTO requstesDto = JsonConvert.DeserializeObject<RequstesDTO>(res.Item2);

            RequstesDTO requstesDto2 = JsonConvert.DeserializeObject<RequstesDTO>(res2.Item2);
            if (requstesDto2.joinRoomRequests != null && requstesDto2.joinRoomRequests.Count > 0
                || requstesDto.joinRoomRequests != null && requstesDto.joinRoomRequests.Count > 0)
            {
                newNotificationAlert.SetActive(true);
            }
            else
            {
                newNotificationAlert.SetActive(false);
            }
            
        }
        
        

    }

}
