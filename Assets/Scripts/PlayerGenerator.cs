using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGenerator : MonoBehaviour
{
    public Maps maps;

    public GameObject spawnSkin0;
    public GameObject spawnSkin1;
    public GameObject spawnSkin2;
    private PlayerController2D playerController;

    Vector3[] positionsMap1 = new[] { new Vector3(1f, 0f, 0f), new Vector3(0f, -1f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 0f) };
    Vector3[] positionsMap2 = new[] { new Vector3(0f, 0f, 0f), new Vector3(5f, 5f, 5f), new Vector3(-2f, -2f, -2f), new Vector3(-3f, -3f, -3f), new Vector3(-4f, -4f, -4f) };
    Vector3[] positionsMap3 = new[] { new Vector3(0f, 0f, 0f), new Vector3(10f, 10f, 10f), new Vector3(-2f, -2f, -2f), new Vector3(3f, 3f, 3f), new Vector3(-4f, -4f, -4f) };

    public Vector3[] actualMap;
    int randPos;
    

    void Start()
    {
        switch (maps)
        {
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

        Debug.Log("Spawning Players");

        for (int i = 0; i < PlayerManager.Instance.numPlayers; i++)
        {
            randPos = Random.Range(1, actualMap.Length);

            switch (PlayerManager.Instance.skinPlayers[i])
            {
                case 0:
                    GameObject Skin0 = Instantiate(spawnSkin0, actualMap[randPos], Quaternion.identity);
                    break;
                case 1:
                    GameObject Skin1 = Instantiate(spawnSkin1, actualMap[randPos], Quaternion.identity);
                    break;
                case 2:
                    GameObject Skin2 = Instantiate(spawnSkin2, actualMap[randPos], Quaternion.identity);
                    break;
                defualt:
                    Debug.LogError("Error, Random Player Generator received skin number that is not inside of the posibilities");
                    break;
            }
        }

        Debug.Log("Players Spawned, auto-destroying this");

        Destroy(this);
    }
}
