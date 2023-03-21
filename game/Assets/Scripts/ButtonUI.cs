using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;
using System;

public class ButtonUI : MonoBehaviour
{   
    
    //public GameManager gameManager = new GameManager();

    public void Bot_button(){
        using (StreamWriter writer = new StreamWriter("Assets/GameType.txt")) {
            // Write the content to the file
            writer.Write(2);
        }
    }
    public void User_button(){
        using (StreamWriter writer = new StreamWriter("Assets/GameType.txt")) {
            // Write the content to the file
            writer.Write(1);
        }
    }
}
