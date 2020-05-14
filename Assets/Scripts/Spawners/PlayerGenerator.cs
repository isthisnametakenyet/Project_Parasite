using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject spawnSkin1;
    public GameObject spawnSkin2;
    public GameObject spawnSkin3;

    [Header("Scripts")]
    public ScoreHUD scoreScript;
    private PlayerController2D playerScript;

    [Header("Delay")]
    public float scoreDelay;

    [Space(10)]
    public GameObject[] spawnPoints;
    int randPos;
    private int whereToSpawn;

    private bool once = false;

    void Start()
    {
        if (!PlayerManager.Instance) { Debug.LogError("Not initialized. Do you have an PlayerManager in your scene?"); }
        Spawn();
        PlayerManager.Instance.StartGame();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].SetActive(false);
        }
    }

    void FixedUpdate() 
    {
        //END ROUND
        if (PlayerManager.Instance.WinRound == true && once == false)
        {
            once = true;
            scoreScript.Activate();
            scoreScript.Round();
            if (PlayerManager.Instance.WinGame == false) { StartCoroutine("DelayHUD"); /*Debug.Log("PGenerator: Start DelayHUD()");*/ }
        }

        //END GAME
        if (PlayerManager.Instance.WinGame == true)
        {
            scoreScript.End();
        }
    }

    IEnumerator DelayHUD()
    {
        yield return new WaitForSeconds(scoreDelay);
        //Debug.Log("PGenerator: End DelayHUD()");
        once = false;
        scoreScript.Desactivate();
        PlayerManager.Instance.DeleteProps = false;
        PlayerManager.Instance.WinRound = false;
        Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < PlayerManager.Instance.numPlayers; i++)
        {
            whereToSpawn = Random.Range(0, spawnPoints.Length + 1);
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
            whereToSpawn = Random.Range(0, spawnPoints.Length + 1);
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
            whereToSpawn = Random.Range(0, spawnPoints.Length + 1);
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
            whereToSpawn = Random.Range(0, spawnPoints.Length + 1);
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
