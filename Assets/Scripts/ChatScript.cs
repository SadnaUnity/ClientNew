using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChatScript:MonoBehaviour
{
    private string ipAddress = "127.0.0.1";
    private int port = 9000;

    [SerializeField] private TMP_InputField msgIf;
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private GameObject scrollView;
    private int msgCount;
    private List<TMP_Text> msgList;

    public void Start()
    {
        msgCount = 0;
        msgList = new List<TMP_Text>();
    }

    public void SendMsg()
    {
        if (msgCount > 15)
        {
            Destroy(msgList[msgCount - 16]);
        }
        string msg = msgIf.text;
        msgIf.text = "";

        TMP_Text msgObject = Instantiate(textObject, chatPanel.transform);
        msgList.Add(msgObject);
        msgObject.text = msg;
        msgCount++;
        
        ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();
        scrollRect.verticalNormalizedPosition = 0f;
    }


}
