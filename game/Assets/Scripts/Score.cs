using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Score : MonoBehaviour{
    
    public static int score  = 0;

    private void Update(){
        GetComponent<UnityEngine.UI.Text>().text = score.ToString();
        string filePath = Application.dataPath + "/score.txt";
        File.WriteAllText(filePath, score.ToString());
    }
}
