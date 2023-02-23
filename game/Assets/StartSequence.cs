using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSequence : MonoBehaviour{
    
    public GameManager gameManager;
    public GameObject StartCanvas;

    void Start(){
        StartCanvas.SetActive(true);
        gameManager.Pause();
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
