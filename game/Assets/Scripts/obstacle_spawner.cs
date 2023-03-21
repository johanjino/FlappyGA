using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle_spawner : MonoBehaviour{
    
    public float maxTime = 1;
    private float timer = 0;
    public GameObject obstacle;
    public GameObject sizeup;
    public GameObject health;
    public int randomNumber;
    public float height;

    // Start is called before the first frame update
    void Start(){
        GameObject new_obstacle = Instantiate(obstacle);
        new_obstacle.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
    }


    // Update is called once per frame
    void Update(){
        if (timer > maxTime){
            GameObject new_obstacle = Instantiate(obstacle);
            new_obstacle.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
            randomNumber = Random.Range(0, 2);
            if(randomNumber == 1){
                randomNumber = Random.Range(0, 2);
                if(randomNumber == 1){
                    GameObject new_sizeup = Instantiate(sizeup);
                    new_sizeup.transform.position = transform.position + new Vector3(2.3f, Random.Range(-3, 3), 0);
                    new_sizeup.transform.parent = new_obstacle.transform;
                    Destroy(new_sizeup, 12);
                }
                else{
                    GameObject new_health = Instantiate(health);
                    new_health.transform.position = transform.position + new Vector3(2.3f, Random.Range(-3, 3), 0);
                    new_health.transform.parent = new_obstacle.transform;
                    Destroy(new_health, 12);
                }
        
            }
            Destroy(new_obstacle, 12);
            timer = 0;
        }
        timer += Time.deltaTime;
    }
}