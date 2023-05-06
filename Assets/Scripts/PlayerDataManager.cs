using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private static Player playerData;
    public static Player PlayerData
    {
        get { return playerData; }
        set { playerData = value; }
    }
    
    private void Awake()
    {
        // If playerData is already set, destroy this instance
        if (playerData != null)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, set playerData and prevent this instance from being destroyed
        playerData = new Player();
        DontDestroyOnLoad(gameObject);
    }
}

