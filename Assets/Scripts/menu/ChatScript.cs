using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Classes.DTO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ChatScript : MonoBehaviour
{
    /*private string ipAddress = "127.0.0.1";
    private int port = 8080;
    private ClientWebSocket clientWebSocket;*/

    [SerializeField] private TMP_InputField msgIf;
    private RectTransform chatContent;
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private ScrollRect scrollView;
    private int msgCount;
    private List<TMP_Text> msgList;
    //private bool isClosing = false;
    private float msgHeight;
    private HttpRequest httpRequest;
    private Player playerData;
    private long lastUpdateTimeStamp;
    public void Start()
    {
        lastUpdateTimeStamp = 0;
        playerData = PlayerDataManager.PlayerData;
        httpRequest = new HttpRequest();
        scrollView.content.anchoredPosition = new Vector2(0, 325f);
        chatContent = scrollView.content;
        msgHeight = textObject.GetComponent<RectTransform>().rect.height;
        msgCount = 0;
        msgList = new List<TMP_Text>();

        TMP_Text newMsg = Instantiate(textObject, chatContent.transform);
        newMsg.text = $"Chat Room {PlayerDataManager.PlayerData.GetRoomId()}!";

        StartCoroutine(GetMsgs());
        /*clientWebSocket = new ClientWebSocket();
        Uri serverUri = new Uri($"ws://{ipAddress}:{port}/chat");
        
        try
        {
            var token = new CancellationToken();
            clientWebSocket.Options.SetRequestHeader("userID", PlayerDataManager.PlayerData.GetUserId().ToString());
            await clientWebSocket.ConnectAsync(serverUri, token);
            Debug.Log("Connected to server");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error connecting to server: {e.Message}");
        }

        ReceiveMessages();*/
    }

    public void SendMsg()
    {
        string message = msgIf.text;
        msgIf.text = "";
        
        List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
        {
            new("userId", playerData.GetUserId()),
            new("roomId", playerData.GetRoomId())
        };
        List<KeyValuePair<string, object>> body = new List<KeyValuePair<string, object>>
        {
            new ("content", message),
            new ("sender", playerData.GetUserName()),
            new ("timestamp", DateTimeOffset.Now.ToUnixTimeMilliseconds())
        };
        Dictionary<string, object> jsonBody = body.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        string bodyString = JsonConvert.SerializeObject(jsonBody, Formatting.Indented);

        var res = httpRequest.SendDataToServer(queryParams, bodyString, "/echo", "PUT");
    }
    private void AddMessageToChat(string message)
    {
        TMP_Text newMsg = Instantiate(textObject, chatContent.transform);
        newMsg.text = message;

        msgList.Add(newMsg);
        msgCount++;

        if (msgCount > 100)
        {
            Destroy(msgList[0].gameObject);
            msgList.RemoveAt(0);
            msgCount--;
        }

        
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }

    private IEnumerator GetMsgs()
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            List<KeyValuePair<string, object>> queryParams = new List<KeyValuePair<string, object>>
            {
                new("userId", playerData.GetUserId()),
                new("roomId", playerData.GetRoomId()),
                new("timestamp", lastUpdateTimeStamp)
            };

            lastUpdateTimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var res = httpRequest.SendDataToServer(queryParams, "", "/chat", "GET");
            if (res.Item1 == 200)
            {
                ChatResDto chatResDto = JsonConvert.DeserializeObject<ChatResDto>(res.Item2);
                foreach (MsgDto msgDto in chatResDto.messageList)
                {
                    AddMessageToChat(msgDto.sender + ": " + msgDto.content);
                }
                
            }
            else
            {
                Debug.LogError("chat get error!");
            }
            
            yield return new WaitForSeconds(3f);
        }
        
    }
    
    /*private async void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];

        while (clientWebSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Debug.Log($"Received message: {message}");

                AddMessageToChat(message);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                Debug.Log("WebSocket connection closed by server");
                break;
            }
        }

        Debug.Log("WebSocket connection closed");
    }

    

    public async void SendMsg()
    {
        string message = msgIf.text;
        if (!string.IsNullOrEmpty(message))
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

            msgIf.text = "";
        }
    }
    
    private async void OnApplicationQuit()
    {
        if (isClosing) return;
        isClosing = true;

        Debug.Log("Closing connection..");
        await CloseWebSocket();
        Debug.Log("Connection is closed");
    }

    private async Task CloseWebSocket()
    {
        if (clientWebSocket == null) return;

        try
        {
            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
        catch (WebSocketException e)
        {
            Debug.LogError($"Error closing WebSocket: {e.Message}");
        }
        finally
        {
            clientWebSocket.Dispose();
            clientWebSocket = null;
        }
    }*/

}
