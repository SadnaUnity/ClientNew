using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popUpWindow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowPopup()
    {
        gameObject.SetActive(true);
    }

    // This method is called when you want to hide the pop-up window
    public void HidePopup()
    {
        gameObject.SetActive(false);
    }

}


