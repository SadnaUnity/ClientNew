using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum Status
{
    PENDING,DECLINED,APPROVED
}
public class NotificationsScript : MonoBehaviour
{
    
    private HttpRequest httpRequest;
    private Player playerData;
    [SerializeField] private GameObject popUpWindow;
    [SerializeField] private TMP_Text textObject;
    private RoomRequests roomRequests;
    private bool isOn;
    private string response;

    // Start is called before the first frame update
    void Start()
    {
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void clickedBtn()
    {
        if (!isOn)
        {
            getPendingReqs();
            popUpWindow.SetActive(true);
            isOn = true;
        }
        else
        {
            popUpWindow.SetActive(false);
            isOn = false;
        }

    }

    public void getPendingReqs()
    {
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
            {
                new("managerId", playerData.GetUserId())
            };
            var res = httpRequest.SendDataToServer(queryParams, "", "/waitingJoinRoomRequests", "GET");
            if (res.Item1 == 200)
            {
                response = res.Item2;
                int y = 0;
                RequstesDTO requstesDto = JsonConvert.DeserializeObject<RequstesDTO>(res.Item2);
                if (requstesDto.joinRoomRequests != null)
                {
                    roomRequests = new RoomRequests(requstesDto);
                    
                                    for (int i = 0; i < requstesDto.joinRoomRequests.Count; i++)
                                    {
                                        
                                        var requst = requstesDto.joinRoomRequests[i];
                    
                                        TMP_Text newMsg = Instantiate(textObject, popUpWindow.transform);
                                        newMsg.text = requst.username + " Wants to be a member in room " +
                                                      requst.roomId.ToString();
                    
                                        // Set the position of the new text object within the popup window
                                        RectTransform newMsgRectTransform = newMsg.GetComponent<RectTransform>();
                                        newMsgRectTransform.anchoredPosition = new Vector2(1458f, 521f + y);
                    
                                        // Create local variables for the button listeners
                                        int index = i;
                                        Button approveButton = newMsg.GetComponentInChildren<Button>();
                                        approveButton.onClick.AddListener(() => ApproveRequest(index));
                    
                                        Button declineButton = newMsg.GetComponentInChildren<Button>();
                                        declineButton.onClick.AddListener(() => DeclineRequest(index));
                    
                                        y += 100;
                                    }
                }

                
            }

    }
    private void ApproveRequest(int index)
    {
        JoinRoomReq requestToApprove = roomRequests.GetJoinRoonmReq()[index];
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("managerId", playerData.GetUserId())
        };
        List<KeyValuePair<string, object>> body = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("userId", roomRequests.GetJoinRoonmReq()[index].GetUserId()),
            new KeyValuePair<string, object>("roomId", roomRequests.GetJoinRoonmReq()[index].GetRoomID()),
            new KeyValuePair<string, object>("requestStatus", "APPROVED")

        };
        //Dictionary<string, object> jsonBody = body.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        string bodyString = JsonConvert.SerializeObject(body, Formatting.Indented);

        var res = httpRequest.SendDataToServer(queryParams, bodyString, "/handlePendingJoinRequests", "POST");
        if (res.Item1 == 200)
        {
            Debug.Log("success");
            RefreshRequests();
        }
    }

    private void DeclineRequest(int index)
    {
        var requestToDecline = roomRequests.GetJoinRoonmReq()[index];
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("managerId", playerData.GetUserId())
        };
        List<KeyValuePair<string, object>> body = new List<KeyValuePair<string, object>>
        {
            new KeyValuePair<string, object>("userId", roomRequests.GetJoinRoonmReq()[index].GetUserId()),
            new KeyValuePair<string, object>("roomId", roomRequests.GetJoinRoonmReq()[index].GetRoomID()),
            new KeyValuePair<string, object>("requestStatus", "DECLINED")

        };
        Dictionary<string, object> jsonBody = body.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        string bodyString = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);

        var res = httpRequest.SendDataToServer(queryParams, bodyString, "/handlePendingJoinRequests", "POST");
        if (res.Item1 == 200)
        {
            Debug.Log("success");
            RefreshRequests();

        }
        
    }
    
    private void RefreshRequests()
    {
        // ...

        // Clear the existing UI elements
        foreach (Transform child in popUpWindow.transform)
        {
            Destroy(child.gameObject);
        }
        

        // Create new UI elements for the updated requests
       getPendingReqs();
    }


    }




