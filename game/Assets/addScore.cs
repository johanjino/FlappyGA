using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addScore : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision){
        Score.score++;
    }
}
