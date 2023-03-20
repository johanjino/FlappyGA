using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class PlayerSpawner : MonoBehaviour
{

    public static PlayerSpawner instance;
    public GameManager gameManager;

    private void Awake()
    {
        instance = this;
    }

    public GameObject playerPrefab;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            SpawnPlayer();
            
            string relative_Path = "score_data.txt"; 
            string full_Path = Path.Combine(Application.dataPath, relative_Path);

            using (FileStream file = new FileStream(full_Path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)) {
                using (StreamWriter writer = new StreamWriter(file)) {
            
                        file.Seek(0, SeekOrigin.Begin);
                        writer.Write(Score.score);
                        writer.Flush();
                        System.Threading.Thread.Sleep(100);

                }
            }
            gameManager.gameType = 1;
            gameManager.Resume();
        }

    }
    public void SpawnPlayer()
    {
        Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();
        player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
    }

    

    
}
