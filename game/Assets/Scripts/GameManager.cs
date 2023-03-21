using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    
    public GameObject gameOverCanvas;
    public static int gameType;

    private void Start(){
        gameOverCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    public void Use_Bot(){
        gameType = 2;
    }
    public void User_play(){
        gameType = 1;
    }
    public int check_game_type(){
        return gameType;
    }

    public void GameOver(){
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
        Score.score = 0;
    }

    public void Replay(){
        SceneManager.LoadScene(0);
    }

    public void Pause(){
        Time.timeScale = 0;
    }

    public void Resume(){
        Time.timeScale = 1;
    }

}
