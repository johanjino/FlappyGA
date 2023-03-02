using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.IO;


public class Client {

    public TcpClient client;
    public NetworkStream stream;
    public bool isConnected;

    
    public void send(string data){
        string host = "127.0.0.1";
        int port = 8888;
        // Connect to the server
        client = new TcpClient(host, port);

        // Get a network stream for sending data
        stream = client.GetStream();

        isConnected = true;

        // Convert the data to a byte array and send it
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        stream.Write(buffer, 0, buffer.Length);
    }
    

    public bool bot(string data){
        string host = "127.0.0.1";
        int port = 8888;
        // Connect to the server
        client = new TcpClient(host, port);

        // Get a network stream for sending data
        stream = client.GetStream();

        isConnected = true;

        // Convert the data to a byte array and send it
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        stream.Write(buffer, 0, buffer.Length);
        byte[] receiveData = new byte[100];
        int bytesReceived = stream.Read(receiveData, 0, receiveData.Length);
        string receivedString = Encoding.ASCII.GetString(receiveData, 0, bytesReceived);
        if (receivedString=="1".ToString()){
            return true;
        }
        return false;
    }
    public void close(){
        // Close the stream and the connection
        stream.Close();
        client.Close();
    }
    
    public InferenceSession ML_load(){
        InferenceSession session = new InferenceSession("Assets/FlappyGA.onnx");
        return session;
    }
    public bool ML_predict(float input1, float input2, float input3, int input4, InferenceSession session){
        var input = new DenseTensor<float>(new[] { 1, 4 });
        input[0, 0] = input1;
        input[0, 1] = input2;
        input[0, 2] = input3;
        input[0, 3] = input4;

        var inputs = new List<NamedOnnxValue>();
        inputs.Add(NamedOnnxValue.CreateFromTensor(session.InputMetadata.Keys.First(), input));

        // Run the prediction
        var outputName = session.OutputMetadata.Keys.First();
        var results = session.Run(inputs);

        // Get the output tensor and print it
        var outputTensor = results.First().AsTensor<float>();
        if (outputTensor.ToArray()[0] > 0.98f){
            return true;
        }
        else{
            return false;
        }

// code to use the result in output
    }
}


public class Fly : MonoBehaviour{

    public GameManager gameManager;
    public Client gameMetadata = new Client();
    public string data = "";
    public float velocity = 1;
    private Rigidbody2D rb;
    public InferenceSession session;

    public int count = 0;
    public int max_time = 100;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        session = gameMetadata.ML_load();
    }

    // Update is called once per frame
    void Update(){
        if (gameManager.gameType == 2){
            Vector2 pos = findNearestObstacle(GameObject.FindGameObjectsWithTag("obstacle"), rb.position.x);
            //data = String.Format("1,{0},{1},{2},{3}",rb.position.y,pos.x,pos.y,count);
            if (gameMetadata.ML_predict(rb.position.y,pos.x,pos.y,count, session)){
                rb.velocity = Vector2.up * velocity;
                count = 0;
            }
        }
        else if (gameManager.gameType == 3){
            if (Input.GetKey("up")){
                //Jump
                rb.velocity = Vector2.up * velocity;
                Vector2 pos = findNearestObstacle(GameObject.FindGameObjectsWithTag("obstacle"), rb.position.x);
                data = String.Format("1,{0},{1},{2},{3}",rb.position.y,pos.x,pos.y,count);
                if (count>10){
                    gameMetadata.send(data);
                }
                count = 0;
            }
            else if (Input.GetKey("f")){
                Vector2 pos = findNearestObstacle(GameObject.FindGameObjectsWithTag("obstacle"), rb.position.x);
                data = String.Format("0,{0},{1},{2},{3}",rb.position.y,pos.x,pos.y,count);
                gameMetadata.send(data);
            }
        }
        else{
            // opens the file without any filelock
            FileStream file = new FileStream(@"H:\EIE2_Projects\IP_Labs\jump_data_temp.txt", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

            byte[] buffer = new byte[1024];

            // Read data from the file stream into the buffer
            int bytesRead = file.Read(buffer, 0, buffer.Length);

            // Convert the byte array to a string
            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            if (file != null)
            {  
                if (data=="1"){
                //FPGA Jumps
                rb.velocity = Vector2.up * velocity;
                }

                 file.Close();
            }
        
        }
        count += 1;
    
    }

    private void OnCollisionEnter2D(Collision2D collision){
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
