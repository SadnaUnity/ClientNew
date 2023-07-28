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
    private bool popUpIsOn = false;

    // Start is called before the first frame update
    void Start()
    {
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;

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

            List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
            {
                new("managerId", playerData.GetUserId())
            };
            var res = httpRequest.SendDataToServer(queryParams, "", "/waitingJoinRoomRequests", "GET");
            if (res.Item1 == 200)
            {
                int y = 0;
                RequstesDTO requstesDto = JsonConvert.DeserializeObject<RequstesDTO>(res.Item2);
                roomRequests = new RoomRequests(requstesDto);
                for (int i = 0; i < requstesDto.joinRoomRequests.Count; i++)
                {
                    var requst = requstesDto.joinRoomRequests[i];

                    TMP_Text newMsg = Instantiate(textObject, popUpWindow.transform);
                    newMsg.text = requst.username.ToString() + " Wants to be a member in room " +
                                  requst.roomId.ToString();
                    newMsg.fontSize = 18;

                    // Set the position of the new text object within the popup window
                    RectTransform newMsgRectTransform = newMsg.GetComponent<RectTransform>();
                    newMsgRectTransform.anchoredPosition = new Vector2(1458f, 521f + y);

                    // Create local variables for the button listeners
                    int index = i;
                    Button approveButton = newMsg.GetComponentInChildren<Button>();
                    approveButton.onClick.AddListener(() => ApproveRequest());

                    Button declineButton = newMsg.GetComponentInChildren<Button>();
                    declineButton.onClick.AddListener(() => DeclineRequest());

                    y += 100;
                }
            }
        }

    }

    private void ApproveOrDeclien(int index)
    {
        JoinRoomReq requestToApprove = roomRequests.GetJoinRoonmReq()[index];
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("managerId", playerData.GetUserId())
        };
        if (index == 1)
        {
            requestToApprove.SetStatus("APPROVED");
        }
        else
        {
            requestToApprove.SetStatus("DECLINED");

        }
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

    private void ApproveRequest()
    {
        ApproveOrDeclien(1);
    }

    private void DeclineRequest()
    {
        ApproveOrDeclien(2);
    }
}
