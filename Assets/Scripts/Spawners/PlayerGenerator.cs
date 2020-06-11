using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject spawnSkin1;
    public GameObject spawnSkin2;
    public GameObject spawnSkin3;

    private PlayerController2D playerScript;

    [Header("SpawnPoints")]
    public GameObject spawnPointsFolder;
    public GameObject[] spawnPoints;

    int randPos;
    private int whereToSpawn;

    void Start()
    {
        if (!PlayerManager.Instance) { Debug.LogError("Not initialized. Do you have an PlayerManager in your scene?"); }
        Spawn();
        PlayerManager.Instance.StartGame();
        spawnPointsFolder.SetActive(false);
        //for (int i = 0; i < spawnPoints.Length; i++)
        //{
        //    spawnPoints[i].SetActive(false);
        //}
    }

    public void Spawn()
    {
        for (int i = 0; i < PlayerManager.Instance.numPlayers; i++)
        {
            whereToSpawn = Random.Range(0, spawnPoints.Length);
            switch (PlayerManager.Instance.SkinPlayer1)
            {
                case 1:
                    GameObject Skin1 = Instantiate(spawnSkin1, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin1.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER0;
                    break;
                case 2:
                    GameObject Skin2 = Instantiate(spawnSkin2, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin2.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER0;
                    break;
                case 3:
                    GameObject Skin3 = Instantiate(spawnSkin3, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin3.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER0;
                    break;
                defualt:
                    Debug.LogError("Error, Random Player Generator received skin number that is not inside of the posibilities");
                    break;
            }
            PlayerManager.Instance.isAlivePlayer1 = true;

            if (PlayerManager.Instance.Player2ON == false) { Debug.Log("PGenerator: 1 Players Spawned"); return; }

            whereToSpawn = Random.Range(0, spawnPoints.Length);
            switch (PlayerManager.Instance.SkinPlayer2)
            {
                case 1:
                    GameObject Skin0 = Instantiate(spawnSkin1, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin0.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER1;
                    break;
                case 2:
                    GameObject Skin1 = Instantiate(spawnSkin2, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin1.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER1;
                    break;
                case 3:
                    GameObject Skin2 = Instantiate(spawnSkin3, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin2.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER1;
                    break;
                defualt:
                    Debug.LogError("Error, Random Player Generator received skin number that is not inside of the posibilities");
                    break;
            }
            PlayerManager.Instance.isAlivePlayer2 = true;

            if (PlayerManager.Instance.Player3ON == false) { Debug.Log("PGenerator: 2 Players Spawned"); return; }
            whereToSpawn = Random.Range(0, spawnPoints.Length);
            switch (PlayerManager.Instance.SkinPlayer1)
            {
                case 1:
                    GameObject Skin0 = Instantiate(spawnSkin1, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin0.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER2;
                    break;
                case 2:
                    GameObject Skin1 = Instantiate(spawnSkin2, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin1.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER2;
                    break;
                case 3:
                    GameObject Skin2 = Instantiate(spawnSkin3, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin2.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER2;
                    break;
                defualt:
                    Debug.LogError("Error, Random Player Generator received skin number that is not inside of the posibilities");
                    break;
            }
            PlayerManager.Instance.isAlivePlayer3 = true;

            if (PlayerManager.Instance.Player4ON == false) { Debug.Log("PGenerator: 3 Players Spawned"); return; }
            whereToSpawn = Random.Range(0, spawnPoints.Length);
            switch (PlayerManager.Instance.SkinPlayer4)
            {
                case 1:
                    GameObject Skin0 = Instantiate(spawnSkin1, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin0.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER3;
                    break;
                case 2:
                    GameObject Skin1 = Instantiate(spawnSkin2, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin1.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER3;
                    break;
                case 3:
                    GameObject Skin2 = Instantiate(spawnSkin3, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                    playerScript = Skin2.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER3;
                    break;
                defualt:
                    Debug.LogError("Error, Random Player Generator received skin number that is not inside of the posibilities");
                    break;
            }
            PlayerManager.Instance.isAlivePlayer4 = true;
        }
        Debug.Log("PGenerator: All Players Spawned");
    }
}
