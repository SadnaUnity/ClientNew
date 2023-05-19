using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class firstSceneController : MonoBehaviour
{

   public void clickedSignIn(){
        SceneManager.LoadScene("Login");
   }
   public void clickedSignUp(){
       SceneManager.LoadScene("ChooseAvatar");
   }
}
