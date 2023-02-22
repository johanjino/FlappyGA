using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Client {

    public TcpClient client;
    public NetworkStream stream;
    public bool isConnected;

    /*
    void Main(){
        isConnected = false;
    }

    public void connect() {
        string host = "127.0.0.1";
        int port = 8888;
        // Connect to the server
        client = new TcpClient(host, port);

        // Get a network stream for sending data
        stream = client.GetStream();

        isConnected = true;
    }
    */

    public void send(string data){
        string host = "127.0.0.1";
        int port = 8888;
        // Connect to the server
        client = new TcpClient(host, port);

        // Get a network stream for sending data
        stream = client.GetStream();

        isConnected = true;
        stream = client.GetStream();
        // Convert the data to a byte array and send it
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        stream.Write(buffer, 0, buffer.Length);
    }
    /*
    public void close(){
        // Close the stream and the connection
        stream.Close();
        client.Close();
    }
    */
}



public class Fly : MonoBehaviour{

    public GameManager gameManager;
    public Client gameMetadata = new Client();
    public string data = "";
    public float velocity = 1;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKey("up")){
            //Jump
            rb.velocity = Vector2.up * velocity;
            Vector2 pos = findNearestObstacle(GameObject.FindGameObjectsWithTag("obstacle"), rb.position.x);
            data = String.Format("1,{0},{1},{2}",rb.position.y,pos.x,pos.y);
        }
        else{
            Vector2 pos = findNearestObstacle(GameObject.FindGameObjectsWithTag("obstacle"), rb.position.x);
            data = String.Format("0,{0},{1},{2}",rb.position.y,pos.x,pos.y);
        }

        if (Input.GetKey("c")){
            //create client
            //gameMetadata.connect();
        }
        gameMetadata.send(data);
        /*
        if (gameMetadata.isConnected){
            //send data
            gameMetadata.send(data);
        }
        */
    }

    private void OnCollisionEnter2D(Collision2D collision){
        //if (gameMetadata.isConnected){
          //  gameMetadata.close();
        //}
        gameManager.GameOver();
    }

    private Vector2 findNearestObstacle(GameObject[] Obstacles, float x){
        Vector2 nearestPosition = Vector2.zero;
        float minDistance = Mathf.Infinity;

        foreach (GameObject obj in Obstacles) {
            float distance = (x - obj.transform.position.x);
            Console.Write(obj.transform.position.x);
            if ((Mathf.Abs(distance) < minDistance) && (distance<=0)) {
                minDistance = distance;
                nearestPosition = obj.transform.position;
            }
        }

        return nearestPosition;
    }
}
