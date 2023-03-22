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

 
}

}