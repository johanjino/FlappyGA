using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;
using System;

public class ButtonUI : MonoBehaviour
{   
    
    public GameManager gameManager;

    public void Bot_button(){
        gameManager.gameType = 2;
    }
    public void User_button(){
        gameManager.gameType = 1;
    }
}
