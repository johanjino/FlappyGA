using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePowerUp : MonoBehaviour
{
    public float timeRemaining;
    public float duration = 4;
    public GameObject pickupEffect;
   
   //maybe return to coroutine?? 

    void OnTriggerEnter2D (Collider2D collision) {
        if (collision.tag == "Player") {
            Fly powernumber = collision.GetComponent<Fly>();
            // powernumber.sizepowers += 1;
            // Destroy(gameObject);
            StartCoroutine(Pickup(collision));
            // PickUp(collision);
        }
    }

    // private void PickUp(Collider2D player){
    //     player.transform.localScale *= 0.4f;

    //     GetComponent<SpriteRenderer>().enabled = false;
    //     GetComponent<Collider2D>().enabled = false;


    //     Invoke("ResetEffects", 3.0f);
    // }

    IEnumerator Pickup(Collider2D player) {
        Fly powernumber = player.GetComponent<Fly>();

        player.transform.localScale *= 0.4f;
        var scale = player.transform.localScale;
        scale.y *= -1;
        player.transform.localScale = scale;
        powernumber.rb.gravityScale *= -1;
        powernumber.sizeduration += 1;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(duration); 

        powernumber.sizeduration -= 1;
        scale.y *= -1;
        player.transform.localScale = scale;
        powernumber.rb.gravityScale *= -1;
        player.transform.localScale /= 0.4f;
        


        Destroy(gameObject);
    }


    // Start is called before the first frame update
    // void Update(){
    //         if(timeRemaining > 0){
    //             timeRemaining -= Time.deltaTime;
    //         }
    //         else {
                
    //         }
    //     }
}
