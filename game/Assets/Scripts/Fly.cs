using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.IO;
using Amazon.DynamoDBv2;
using Amazon;
using Photon.Pun;


public class Client {


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


public class Fly : MonoBehaviourPunCallbacks{

    public GameManager gameManager;
    public Client gameMetadata = new Client();
    public string data = "";
    public float velocity = 1;
    public int lives = 3;
    public int sizeduration = 0;
    public Rigidbody2D rb;
    public InferenceSession session;
    public GameObject gameOverCanvas;

    public int gameType;
    public int count = 0;
    public int max_time = 100;

    // Start is called before the first frame update
    void Start(){
        UnityInitializer.AttachToGameObject(this.gameObject);
        rb = GetComponent<Rigidbody2D>();
        session = gameMetadata.ML_load();
        rb.velocity = Vector2.up * velocity;
        using (StreamReader reader = new StreamReader("Assets/GameType.txt")) {
            // Read the integer from the file
            gameType = int.Parse(reader.ReadToEnd());
        }

        Physics2D.IgnoreLayerCollision(6, 6, true);
    }

    // Update is called once per frame
    void Update(){
        Debug.Log(gameType);
        if (gameType == 2){
            Vector2 pos = findNearestObstacle(GameObject.FindGameObjectsWithTag("obstacle"), rb.position.x);
            //data = String.Format("1,{0},{1},{2},{3}",rb.position.y,pos.x,pos.y,count);
            if (gameMetadata.ML_predict(rb.position.y,pos.x,pos.y,count, session)){
                rb.velocity = Vector2.up * velocity;
                count = 0;
            }
        }
        else{
            
            if (photonView.IsMine) {
            //string relative_Path = "jump_data.txt"; 
            //string full_Path = Path.Combine(Application.dataPath, relative_Path);
           
           // FileStream file = new FileStream(full_Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite); 

           // byte[] buffer = new byte[1024];
           // int bytesRead = file.Read(buffer, 0, buffer.Length);

           // string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            //if (file != null)
            //{  
                if (sizeduration == 0 && (/*data=="1" ||*/ Input.GetKey("up"))){  //FPGA Jumps
                
                rb.velocity = Vector2.up * velocity;
                
                }
                else if(/*data=="1" || */ Input.GetKeyDown("up")){
                    rb.velocity = Vector2.down * velocity;
                }
               // print(data);
                //file.Close();
            //}
            }
        }
        count += 1;
    
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag != "Player"){
            //GameObject gameOver = Instantiate(gameOverCanvas);
            //gameOver.SetActive(true);
            lives -=1;
            if(lives == 0){
                Time.timeScale = 0;
                SceneManager.LoadScene("Leaderboard", LoadSceneMode.Additive);
            }
        }
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
