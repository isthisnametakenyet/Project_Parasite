using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Maps { Map1, Map2, Map3 };
public enum PickTypes { Sword, Axe, Spear, Bow, CrossBow, Boomerang };

public class RandomSpawnScript : MonoBehaviour
{
    public Maps maps;

    // prefabs to instantiate
    public GameObject PickUp;
    private PickUpScript PickObject;

    // spawn prefabs once per 2 secs
    public float spawnMaxRate = 15f;
    public float spawnMinRate = 7f;

    // positions to spawn the prefabs
    Vector3[] positionsMap1 = new[] { new Vector3(-5.5f, 1.8f, 1f), new Vector3(5.5f, 1.8f, 0f), new Vector3(3.4f, -2.25f, 0f), new Vector3(-3.4f, -2.25f, 0f), new Vector3(0f, -4.2f, 0f) };
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

    //nuemro de objetos espawneados
    public int numSpawned = 0;
    public int maxNumSpawned;

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
        if (numSpawned < maxNumSpawned) {
            if (Time.time > nextSpawn) // if time has come
            {
                whatToSpawn = Random.Range(1, 7); //define random value between 1 and 6 (7 is exclusive)

                whereToSpawn = Random.Range(0, 5); // define random value between 0 and 4 (5 is exclusive) TODO:ArrayList.Length

                whenToSpawn = Random.Range(spawnMinRate, spawnMaxRate); // define random time value to spawn

                while (whatToSpawn == lastSpawned)
                {
                    whatToSpawn = Random.Range(1, 7);
                }
                lastSpawned = whatToSpawn;

                numSpawned++;

                if (whatToSpawn == 1)
                {
                    GameObject pick = Instantiate(PickUp, actualMap[whereToSpawn], Quaternion.identity);
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Sword;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 2)
                {
                    GameObject pick = Instantiate(PickUp, actualMap[whereToSpawn], Quaternion.identity);
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Axe;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 3)
                {
                    GameObject pick = Instantiate(PickUp, actualMap[whereToSpawn], Quaternion.identity);
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Spear;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 4)
                {
                    GameObject pick = Instantiate(PickUp, actualMap[whereToSpawn], Quaternion.identity);
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Bow;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whereToSpawn == 5)
                {
                    GameObject pick = Instantiate(PickUp, actualMap[whereToSpawn], Quaternion.identity);
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.CrossBow;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 6)
                {
                    GameObject pick = Instantiate(PickUp, actualMap[whereToSpawn], Quaternion.identity);
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Boomerang;
                    PickObject.RadomSpawner = this.gameObject;
                }

                // set next spawn time
                nextSpawn = Time.time + whenToSpawn;
            }
        }
    }
}
