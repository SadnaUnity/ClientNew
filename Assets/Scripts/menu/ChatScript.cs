using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatScript : MonoBehaviour
{
    private string ipAddress = "127.0.0.1";
    private int port = 8080;
    private ClientWebSocket clientWebSocket;

    [SerializeField] private TMP_InputField msgIf;
    private RectTransform chatContent;
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private ScrollRect scrollView;
    private int msgCount;
    private List<TMP_Text> msgList;
    private bool isClosing = false;
    private float msgHeight;
    public async void Start()
    {
        scrollView.content.anchoredPosition = new Vector2(0, 325f);
        chatContent = scrollView.content;
        msgHeight = textObject.GetComponent<RectTransform>().rect.height;
        msgCount = 0;
        msgList = new List<TMP_Text>();

        clientWebSocket = new ClientWebSocket();
        Uri serverUri = new Uri($"ws://{ipAddress}:{port}/chat");

        TMP_Text newMsg = Instantiate(textObject, chatContent.transform);
        newMsg.text = $"Chat Room {PlayerDataManager.PlayerData.GetRoomId()}!";

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

        ReceiveMessages();
    }

    private async void ReceiveMessages()
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
    }

}
