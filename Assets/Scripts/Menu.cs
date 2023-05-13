using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button menuBtn;
    [SerializeField] private GameObject menuPanel;
    private RectTransform btnRectTransform;
    private bool showMenu;
    public void Start()
    {
        btnRectTransform = menuBtn.GetComponent<RectTransform>();
        showMenu = false;
        menuPanel.SetActive(false);
    }
    public void ShowHideMenu()
    {
        showMenu = !showMenu;
        if (showMenu)
        {
            menuPanel.SetActive(true);
            btnRectTransform.anchoredPosition = new Vector2(-875, 500);
        }
        else
        {
            menuPanel.SetActive(false);
            btnRectTransform.anchoredPosition = new Vector2(-1175, 500);
        }
        
        
    }
}
