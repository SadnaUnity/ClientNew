using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatScript:MonoBehaviour
{
    public string ipAddress = "127.0.0.1";
    public int port = 9000;

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text chatLogText;
    [SerializeField] private ScrollRect chatLogScrollRect;

    private TcpClient client;
    private NetworkStream stream;

    private async void Start()
    {
        // Connect to the server
        try
        {
            client = new TcpClient();
            await client.ConnectAsync(ipAddress, port);
            stream = client.GetStream();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error connecting to server: {ex.Message}");
            return;
        }

        // Start receiving messages
        _ = ReceiveMessages();
    }

    private async Task ReceiveMessages()
    {
        while (true)
        {
            // Read incoming messages
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Update the chat log UI
            chatLogText.text += message;
            chatLogText.text += '\n';
            chatLogScrollRect.verticalNormalizedPosition = 0f;
        }
    }

    public async void OnSendMsg()
    {
        // Get the message text from the input field
        string message = inputField.text;
        if (string.IsNullOrEmpty(message))
        {
            return;
        }

        // Send the message to the server
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(buffer, 0, buffer.Length);
        
        // Update the chat log UI
        chatLogText.text += message;
        chatLogText.text += '\n';
        chatLogScrollRect.verticalNormalizedPosition = 0f;
        
        // Clear the input field
        inputField.text = "";
    }
}