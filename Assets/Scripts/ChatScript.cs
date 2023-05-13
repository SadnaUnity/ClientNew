using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Net.WebSockets;
using System.Linq;

public class ChatScript : MonoBehaviour
{
    private string ipAddress = "127.0.0.1";
    private int port = 8080;
    private ClientWebSocket clientWebSocket;

    [SerializeField] private TMP_InputField msgIf;
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private GameObject scrollView;
    private int msgCount;
    private List<TMP_Text> msgList;

    public async void Start()
    {
        msgCount = 0;
        msgList = new List<TMP_Text>();

        clientWebSocket = new ClientWebSocket();
        Uri serverUri = new Uri($"ws://{ipAddress}:{port}/chat");

        try
        {
            await clientWebSocket.ConnectAsync(serverUri, CancellationToken.None);
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
            try
            {
                WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    // Display the message in the chat panel
                    TMP_Text msgObject = Instantiate(textObject, chatPanel.transform);
                    msgList.Add(msgObject);
                    msgObject.text = message;
                    msgCount++;

                    ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();
                    scrollRect.verticalNormalizedPosition = 0f;
                }
            }
            catch (WebSocketException e)
            {
                Debug.LogError($"WebSocket error: {e.Message}");
                break;
            }
        }
    }

    public async void SendMsg()
    {
        string msg = msgIf.text;
        msgIf.text = "";

        if (clientWebSocket.State == WebSocketState.Open)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            // Display the message in the chat panel
            TMP_Text msgObject = Instantiate(textObject, chatPanel.transform);
            msgList.Add(msgObject);
            msgObject.text = msg;
            msgCount++;

            ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
