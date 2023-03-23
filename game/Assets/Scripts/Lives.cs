using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Lives : MonoBehaviour{
    
    public static int lives  = 0;

    private void Update(){
        if (lives!=-1){
            GetComponent<UnityEngine.UI.Text>().text = lives.ToString();
        }
        
    }
}
