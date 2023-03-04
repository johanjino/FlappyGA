using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.IO;
using System;

public class StartSequence : MonoBehaviour{
    
    public GameManager gameManager;
    public GameObject StartCanvas;

    void Start(){
        StartCanvas.SetActive(true);
        gameManager.Pause();

        string relative_Path = "score_data.txt"; 
        string full_Path = Path.Combine(Application.dataPath, relative_Path);

        using (FileStream file = new FileStream(full_Path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)) {
            using (StreamWriter writer = new StreamWriter(file)) {
         
                    file.Seek(0, SeekOrigin.Begin);
                    writer.Write(Score.score);
                    writer.Flush();
                    System.Threading.Thread.Sleep(100);

            }
        }
    }

    public void AI_data_button(){
        gameManager.gameType = 3;
        StartCanvas.SetActive(false);
        gameManager.Resume();

    }
    public void Bot_button(){
        gameManager.gameType = 2;
        StartCanvas.SetActive(false);
        gameManager.Resume();
    }
    public void User_button(){
        gameManager.gameType = 1;
        StartCanvas.SetActive(false);
        gameManager.Resume();
    }
}
