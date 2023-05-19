using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorScript : MonoBehaviour
{
    public Image avatar;
    public Sprite yellow, blue,pink,green;
    public AvatarColor color;

    public void Start()
    {
        color = (AvatarColor)PlayerPrefs.GetInt("avatarColor");
        switch (color)
        {
            case AvatarColor.BLUE:
                avatar.sprite = blue;
                break;
            
            case AvatarColor.PINK:
                avatar.sprite = pink;
                break;
            
            case AvatarColor.GREEN:
                avatar.sprite = green;
                break;
            
            case AvatarColor.YELLOW:
                avatar.sprite = yellow;
                break;
            default:
                avatar.sprite = blue; //if first time
                break;
            }
    }

    public void ClickedYellow(){
        color = AvatarColor.YELLOW;
        avatar.sprite = yellow;
    }

    public void ClickedPink(){
        color = AvatarColor.PINK;
        avatar.sprite = pink;
    }
    public void ClickedBlue(){
        color = AvatarColor.BLUE;
        avatar.sprite = blue;
    }
    public void ClickedGreen(){
        color = AvatarColor.GREEN;
        avatar.sprite = green;
    }
    public void ClickedNext(){ 
        PlayerPrefs.SetInt("avatarColor",(int)color);
        SceneManager.LoadScene("ChooseAccessory");
    }
    public void ClickedBack(){ 
        SceneManager.LoadScene("First");
    }
}
