using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

public class Leaderboard_EC2 : MonoBehaviour
{
    private const string SERVER_IP = "34.232.67.167";
    private const int SERVER_PORT = 1234; // change to a different port number
    private TcpClient client;

    void Start()
    {
        Connect();
        SendData("TestingName1",30);
        Thread.Sleep(1000);
        SendData("TestingName2",23);
        Thread.Sleep(1000);
        SendData("TestingName3",20);
        Thread.Sleep(1000);
        SendData("TestingName4",50);
        Thread.Sleep(1000);
        SendData("TestingName5",12);
        ReceiveLeaderboard();
    }

    void Update()
    {
        
    }

    public void Connect()
    {
        try
        {
            client = new TcpClient(SERVER_IP, SERVER_PORT);
            Debug.Log("Connected to server.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    public void SendData(string playername, int score)
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
            for (int i = 0; i < 5; i++)
            {
                var playername = leaderboard[i]["playername"].ToString();
                var score = (int) leaderboard[i]["score"];
                Debug.Log("Player " + (i+1) + ": " + playername + " - Score: " + score);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving leaderboard data from server: " + e.Message);
        }
    }
}
