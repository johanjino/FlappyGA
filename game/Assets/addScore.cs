using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class addScore : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision){

        string relative_Path = "score_data.txt"; 
        string full_Path = Path.Combine(Application.dataPath, relative_Path);

        Score.score++;

        using (FileStream file = new FileStream(full_Path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)) {
            using (StreamWriter writer = new StreamWriter(file)) {
         
                    file.Seek(0, SeekOrigin.Begin);
                    writer.Write(Score.score);
                    writer.Flush();
                    System.Threading.Thread.Sleep(100);

            }
        }

    
    }
}
