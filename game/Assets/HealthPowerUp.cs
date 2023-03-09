using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HealthPowerUp : MonoBehaviour
{
    public GameObject pickupEffect;

    void OnTriggerEnter2D (Collider2D collision) {
        if (collision.tag == "Player") {
            Pickup(collision);
            // GameObject player = collision.GameObject;
            // Player playerScript = player.GetComponent<Player>();

            // if(playerScript){
            //     Debug.Log("Power up picked up");
            // }
        }
    }

    void Pickup(Collider2D player) {
        // Instantiate(pickupEffect, transform.position, transform.rotation);
        Fly stats = player.GetComponent<Fly>();
        stats.lives += 1;

        Debug.Log("Number of lives " + stats.lives);
        // Debug.Log("Power up picked up");

        Destroy(gameObject);
    }
    // Start is called before the first frame update
  
}
