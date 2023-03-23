using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class Leaderboard_EC2 : MonoBehaviour
{
    private const string SERVER_IP = "34.232.67.167";
    private const int SERVER_PORT = 1234; // change to a different port number
    //public LauncherFlappyBird launcher;
    private TcpClient client;
    public GameObject gameoverScreen;
    public GameObject leaderBoard;
    public bool isConnect;

    void Start()
    {
        gameoverScreen.SetActive(true);
        Debug.Log(isConnect);
        if (!isConnect){
            Connect();
        }
        string filePath = Application.dataPath + "/example.txt";
        string name = File.ReadAllText(filePath);
        string filePath1 = Application.dataPath + "/score.txt";
        string score = File.ReadAllText(filePath1);
        SendData(name,score);
        ReceiveLeaderboard();
        Disconnect();
        //gameoverScreen.SetActive(false);
        leaderBoard.SetActive(true);

    }

    void Update()
    {
        
    }


    public void onClick(){
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void Connect()
    {
        try
        {
            client = new TcpClient(SERVER_IP, SERVER_PORT);
            Debug.Log("Connected to server.");
            isConnect = true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    public void getClient()
    {
        LauncherFlappyBird launcher = new LauncherFlappyBird();
    }


    public void SendData(string playername, string score)
    {
        try
        {
            var data = new {playername = playername, score = score, type = "submit_score"}; // add type field
            var json = JsonConvert.SerializeObject(data);
            var bytes = Encoding.UTF8.GetBytes(json);
            var stream = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);

            Debug.Log("Sent data to server: " + json);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data to server: " + e.Message);
        }
    }

    public void ReceiveLeaderboard()
    {
        try
        {
            // Send a request to the server for the leaderboard data
            var request = Encoding.UTF8.GetBytes("leaderboard");
            var stream = client.GetStream();
            stream.Write(request, 0, request.Length);

            // Receive the response from the server
            var buffer = new byte[1024];
            var response = new StringBuilder();
            int bytes;

            do
            {
                bytes = stream.Read(buffer, 0, buffer.Length);
                response.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
            }
            while (stream.DataAvailable);

            // Parse the JSON response and display the top 5 players with highest scores
            var leaderboard = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.ToString());
            Debug.Log("Received leaderboard data from server:");
            if (leaderboard.Count<5){
                for (int i=0; i<leaderboard.Count;i++){
                    var playername = leaderboard[i]["playername"];
                    var score = leaderboard[i]["score"];
                    if (i==0){
                    // Find the TextMeshPro object by name
                    GameObject nameObject = GameObject.Find("Name1");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score1");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                    

                }
                else if (i==1){
                    GameObject nameObject = GameObject.Find("Name2");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score2");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                }
                else if (i==2){
                    GameObject nameObject = GameObject.Find("Name3");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score3");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                }
                else if (i==3){
                    GameObject nameObject = GameObject.Find("Name4");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score4");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                }
                else if (i==4){
                    GameObject nameObject = GameObject.Find("Name5");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score5");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                }
                }
            }
            else{
                for (int i = 0; i < 5; i++)
            {
                var playername = leaderboard[i]["playername"];
                var score = leaderboard[i]["score"].ToString();
                if (i==0){
                    Debug.Log("TRYING THIS? IDK MAN");
                    // Find the TextMeshPro object by name
                    GameObject nameObject = GameObject.Find("Name1");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score1");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                    
                }
                else if (i==1){
                    GameObject nameObject = GameObject.Find("Name2");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score2");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                }
                else if (i==2){
                    GameObject nameObject = GameObject.Find("Name3");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score3");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                }
                else if (i==3){
                    GameObject nameObject = GameObject.Find("Name4");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score4");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                }
                else if (i==4){
                    GameObject nameObject = GameObject.Find("Name5");
                    TextMeshProUGUI name = nameObject.GetComponent<TextMeshProUGUI>();
                    name.text = playername.ToString();
                    GameObject scoreObject = GameObject.Find("Score5");
                    TextMeshProUGUI scores = scoreObject.GetComponent<TextMeshProUGUI>();
                    scores.text = score.ToString();
                }
            }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving leaderboard data from server: " + e.Message);
        }
    }

    public void Disconnect()
{
    try
    {
        client.Close();
        Debug.Log("Disconnected from server.");
    }
    catch (Exception e)
    {
        Debug.LogError("Error disconnecting from server: " + e.Message);
    }
}
}
