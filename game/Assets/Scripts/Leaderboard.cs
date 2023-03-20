using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour{

    [SerializeField]
    private List<TextMeshProUGUI> names;

    [SerializeField]
    private List<TextMeshProUGUI> scores;


    public void getLeaderboard(){

        /*
        This is how to update leaderboard:

        for (int i = 0; i<names.Count; i++){
            names[i].text = //name;
            scores[i].text = //score.ToString (if int)
        }


        */
    }

    public void setleaderboardentry(string username, int score){
        //upload to server new username and score
    }

}
