using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button menuBtn;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject notification;
    private RectTransform btnRectTransform;
    private RectTransform notificationBtnRectTransform;

    private bool showMenu;
    private HttpRequest httpRequest;
    private Player playerData;
    public void Start()
    {
        httpRequest = new HttpRequest();
        playerData = PlayerDataManager.PlayerData;
        btnRectTransform = menuBtn.GetComponent<RectTransform>();
        notificationBtnRectTransform = notification.GetComponent<RectTransform>();
        showMenu = true;
        menuPanel.SetActive(showMenu);
        notification.SetActive(false);
        btnRectTransform.anchoredPosition = new Vector2(-890, 500);
    }
    public void ShowHideMenu()
    {
        showMenu = !showMenu;
        if (showMenu)
        {
            menuPanel.SetActive(true);
            btnRectTransform.anchoredPosition = new Vector2(-890, 500);
            notificationBtnRectTransform.anchoredPosition = new Vector2(-875, 440);

        }
        else
        {
            menuPanel.SetActive(false);
            btnRectTransform.anchoredPosition = new Vector2(-1190, 500);
            notificationBtnRectTransform.anchoredPosition = new Vector2(-1175, 440);

        }
        
        
    }

    
}
