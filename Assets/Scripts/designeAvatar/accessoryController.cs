
using UnityEngine;
using Image = UnityEngine.UI.Image;
using UnityEngine.SceneManagement;
public class accessoryController : MonoBehaviour
{
    public Image avatar;
    public Sprite blue, pink, yellow, green;
    public Sprite blueHartGlass,pinkHartGlass,greenHartGlass,yellowHartGlass,
        blueRoundGlass, pinkRoundGlass,  greenRoundGlass, yellowRoundGlass,
        blueChefHat,pinkChefHat,greenChefHat,yellowChefHat,
        blueChristmesHat,pinkChristmesHat,greenChristmesHat,yellowChristmesHat;
    public AvatarColor chosenAvatarColor;
    private AvatarAccessory chosenAvatarAccessory;
    public void Start()
    {
        chosenAvatarColor = (AvatarColor)PlayerPrefs.GetInt("avatarColor");
        chosenAvatarAccessory = AvatarAccessory.EMPTY;
        switch (chosenAvatarColor)
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
            
        }
    }

    public void clickedHartGlass(){
        switch (chosenAvatarColor)
        {
            case AvatarColor.BLUE:
                avatar.sprite = blueHartGlass;
                break;
            
            case AvatarColor.PINK:
                avatar.sprite = pinkHartGlass;
                break;
            
            case AvatarColor.GREEN:
                avatar.sprite = greenHartGlass;
                break;
            
            case AvatarColor.YELLOW:
                avatar.sprite = yellowHartGlass;
                break;
            
        }
        chosenAvatarAccessory = AvatarAccessory.HEART_GLASSES;
    }
    public void clickedRoundGlass(){
        switch (chosenAvatarColor)
        {
            case AvatarColor.BLUE:
                avatar.sprite = blueRoundGlass;
                break;
            
            case AvatarColor.PINK:
                avatar.sprite = pinkRoundGlass;
                break;
            
            case AvatarColor.GREEN:
                avatar.sprite = greenRoundGlass;
                break;
            
            case AvatarColor.YELLOW:
                avatar.sprite = yellowRoundGlass;
                break;
            
        }        chosenAvatarAccessory = AvatarAccessory.NORMAL_GLASSES;
    }
    public void clickedChefHat(){
        switch (chosenAvatarColor)
        {
            case AvatarColor.BLUE:
                avatar.sprite = blueChefHat;
                break;
            
            case AvatarColor.PINK:
                avatar.sprite = pinkChefHat;
                break;
            
            case AvatarColor.GREEN:
                avatar.sprite = greenChefHat;
                break;
            
            case AvatarColor.YELLOW:
                avatar.sprite = yellowChefHat;
                break;
            
        }        chosenAvatarAccessory = AvatarAccessory.COOK_HAT;
    }
    public void clickedChristmesHat(){
        switch (chosenAvatarColor)
        {
            case AvatarColor.BLUE:
                avatar.sprite = blueChristmesHat;
                break;
            
            case AvatarColor.PINK:
                avatar.sprite = pinkChristmesHat;
                break;
            
            case AvatarColor.GREEN:
                avatar.sprite = greenChristmesHat;
                break;
            
            case AvatarColor.YELLOW:
                avatar.sprite = yellowChristmesHat;
                break;
            
        }
        chosenAvatarAccessory = AvatarAccessory.SANTA_HAT;
    }
    public void clickedOnEmpty(){
        switch (chosenAvatarColor)
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
            
        }    }

    public void ClickedNext()
    {
        PlayerPrefs.SetInt("avatarAccessory", (int)chosenAvatarAccessory);
        SceneManager.LoadScene("ChooseNameAndPassword");
    }
    public void ClickedBack(){ 
        PlayerPrefs.SetInt("avatarColor",(int)chosenAvatarColor);
        SceneManager.LoadScene("ChooseAvatar");
    }
}
   

