using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public Maps maps;

    [Header("Prefabs")]
    public GameObject spawnSkin1;
    public GameObject spawnSkin2;
    public GameObject spawnSkin3;

    [Header("Scripts")]
    public ScoreHUD scoreScript;
    private PlayerController2D playerScript;

    [Header("Delay")]
    public float scoreDelay;

    Vector3[] positionsMapTut = new[] { new Vector3(-7.4f, -4.2f, 1f), new Vector3(-7.4f, -4.2f, 1f), new Vector3(-7.4f, -4.2f, 1f), new Vector3(-7.4f, -4.2f, 1f), new Vector3(-7.4f, -4.2f, 1f) };
    Vector3[] positionsMap1 = new[] { new Vector3(-5.5f, 1.8f, 1f), new Vector3(5.5f, 1.8f, 0f), new Vector3(3.4f, -2.25f, 0f), new Vector3(-3.4f, -2.25f, 0f), new Vector3(0f, -4.2f, 0f) };
    Vector3[] positionsMap2 = new[] { new Vector3(0f, 0f, 0f), new Vector3(5f, 5f, 5f), new Vector3(-2f, -2f, -2f), new Vector3(-3f, -3f, -3f), new Vector3(-4f, -4f, -4f) };
    Vector3[] positionsMap3 = new[] { new Vector3(0f, 0f, 0f), new Vector3(10f, 10f, 10f), new Vector3(-2f, -2f, -2f), new Vector3(3f, 3f, 3f), new Vector3(-4f, -4f, -4f) };

    [Header("Spawn Points")]
    public Vector3[] actualMap;
    int randPos;

    public bool once = false;

    void Start()
    {
        if (!PlayerManager.Instance) { Debug.LogError("Not initialized. Do you have an PlayerManager in your scene?"); }
        Spawn();
        PlayerManager.Instance.StartGame();

        //TODO
        //switch (maps)
        //{
        //    case Maps.MapTut:
        //        actualMap = positionsMapTut;
        //        break;
        //    case Maps.Map1:
        //        actualMap = positionsMap1;
        //        break;
        //    case Maps.Map2:
        //        actualMap = positionsMap2;
        //        break;
        //    case Maps.Map3:
        //        actualMap = positionsMap3;
        //        break;
        //}
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
        switch (maps)
        {
            case Maps.MapTut:
                actualMap = positionsMapTut;
                break;
            case Maps.Map1:
                actualMap = positionsMap1;
                break;
            case Maps.Map2:
                actualMap = positionsMap2;
                break;
            case Maps.Map3:
                actualMap = positionsMap3;
                break;
        }
        for (int i = 0; i < PlayerManager.Instance.numPlayers; i++)
        {
            randPos = Random.Range(1, actualMap.Length);

            switch (PlayerManager.Instance.SkinPlayer1)
            {
                case 1:
                    GameObject Skin1 = Instantiate(spawnSkin1, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin1.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER0;
                    break;
                case 2:
                    GameObject Skin2 = Instantiate(spawnSkin2, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin2.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER0;
                    break;
                case 3:
                    GameObject Skin3 = Instantiate(spawnSkin3, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin3.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER0;
                    break;
                defualt:
                    Debug.LogError("Error, Random Player Generator received skin number that is not inside of the posibilities");
                    break;
            }
            PlayerManager.Instance.isAlivePlayer1 = true;

            if (PlayerManager.Instance.Player2ON == false) { Debug.Log("PGenerator: 1 Players Spawned"); return; }
            switch (PlayerManager.Instance.SkinPlayer2)
            {
                case 1:
                    GameObject Skin0 = Instantiate(spawnSkin1, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin0.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER1;
                    break;
                case 2:
                    GameObject Skin1 = Instantiate(spawnSkin2, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin1.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER1;
                    break;
                case 3:
                    GameObject Skin2 = Instantiate(spawnSkin3, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin2.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER1;
                    break;
                defualt:
                    Debug.LogError("Error, Random Player Generator received skin number that is not inside of the posibilities");
                    break;
            }
            PlayerManager.Instance.isAlivePlayer2 = true;

            if (PlayerManager.Instance.Player3ON == false) { Debug.Log("PGenerator: 2 Players Spawned"); return; }
            switch (PlayerManager.Instance.SkinPlayer1)
            {
                case 1:
                    GameObject Skin0 = Instantiate(spawnSkin1, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin0.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER2;
                    break;
                case 2:
                    GameObject Skin1 = Instantiate(spawnSkin2, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin1.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER2;
                    break;
                case 3:
                    GameObject Skin2 = Instantiate(spawnSkin3, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin2.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER2;
                    break;
                defualt:
                    Debug.LogError("Error, Random Player Generator received skin number that is not inside of the posibilities");
                    break;
            }
            PlayerManager.Instance.isAlivePlayer3 = true;

            if (PlayerManager.Instance.Player4ON == false) { Debug.Log("PGenerator: 3 Players Spawned"); return; }
            switch (PlayerManager.Instance.SkinPlayer4)
            {
                case 1:
                    GameObject Skin0 = Instantiate(spawnSkin1, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin0.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER3;
                    break;
                case 2:
                    GameObject Skin1 = Instantiate(spawnSkin2, actualMap[randPos], Quaternion.identity);
                    playerScript = Skin1.GetComponent<PlayerController2D>();
                    playerScript.controller = Controller.PLAYER3;
                    break;
                case 3:
                    GameObject Skin2 = Instantiate(spawnSkin3, actualMap[randPos], Quaternion.identity);
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
