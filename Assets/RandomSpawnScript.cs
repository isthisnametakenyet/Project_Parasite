using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// maps enum
public enum Maps {NONE, Map1, Map2, Map3 };

public class RandomSpawnScript : MonoBehaviour
{
    public Maps maps = Maps.NONE;

    // prefabs to instantiate
    public GameObject prefab1, prefab2, prefab3, prefab4, prefab5;

    // spawn prefabs once per 2 secs
    public float spawnMaxRate = 20f;
    public float spawnMinRate = 10f;

    // positions to spawn the prefabs
    Vector3[] positionsMap1 = new[] { new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f), new Vector3(2f, 2f, 2f), new Vector3(3f, 3f, 3f), new Vector3(4f, 4f, 4f) };
    Vector3[] positionsMap2 = new[] { new Vector3(0f, 0f, 0f), new Vector3(5f, 5f, 5f), new Vector3(-2f, -2f, -2f), new Vector3(-3f, -3f, -3f), new Vector3(-4f, -4f, -4f) };
    Vector3[] positionsMap3 = new[] { new Vector3(0f, 0f, 0f), new Vector3(10f, 10f, 10f), new Vector3(-2f, -2f, -2f), new Vector3(3f, 3f, 3f), new Vector3(-4f, -4f, -4f) };

    Vector3[] actualMap;

    // variable to set next spawn time
    public float nextSpawn = 0f;

    // variable to contain random value
    public int whatToSpawn;

    // variable to contain random value
    public int whereToSpawn;

    // temporal variable to save what we spawned
    int lastSpawned;

    // variable to declare when to spawn
    public float whenToSpawn;

    private void Start()
    {
        switch(maps)
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


        ArrayList MapsSpawns = new ArrayList();
        MapsSpawns.Add(positionsMap1);
        MapsSpawns.Add(positionsMap2);
        MapsSpawns.Add(positionsMap3);
    }


    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn) // if time has come
        {
            whatToSpawn = Random.Range(1, 6); //define random value between 1 and 5 (6 is exclusive)
            Debug.Log(whatToSpawn);

            whereToSpawn = Random.Range(0, 5); // define random value between 0 and 4 (5 is exclusive)
            Debug.Log(whereToSpawn);

            whenToSpawn = Random.Range(spawnMinRate, spawnMaxRate); // define random time value to spawn
            
            while(whatToSpawn == lastSpawned)
            {
                whatToSpawn = Random.Range(1, 6);
            }
            lastSpawned = whatToSpawn;
            Debug.Log(whatToSpawn);

            if (whatToSpawn == 1)
            {
                Instantiate(prefab1, actualMap[whereToSpawn], Quaternion.identity);
            }
           else if(whatToSpawn == 2)
            {
                Instantiate(prefab2, actualMap[whereToSpawn], Quaternion.identity);
            }
           else if(whatToSpawn == 3)
            {
                Instantiate(prefab3, actualMap[whereToSpawn], Quaternion.identity);
            }
           else if(whatToSpawn == 4)
            {
                Instantiate(prefab4, actualMap[whereToSpawn], Quaternion.identity);
            }
           else if( whereToSpawn == 5)
            {
                Instantiate(prefab5, actualMap[whereToSpawn], Quaternion.identity);
            }

            // set next spawn time
            nextSpawn = Time.time + whenToSpawn;
        }
    }
}
