using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickTypes { Sword, Axe, Spear, Bow, CrossBow, Boomerang };

public class RandomSpawnScript : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject PickUp;
    private PickUpScript PickObject;

    [Header("Rate")]
    public float spawnMaxRate = 15f;
    public float spawnMinRate = 7f;

    [Space(5)]
    public bool OnePerSpawnPoint;
    public bool[] filledPoints;
    public bool allFilled = false;

    [Space(10)]
    public GameObject[] spawnPoints;

    /// variable to set next spawn time
    private float nextSpawn = 0f;

    /// variable to contain random value
    private int whatToSpawn;

    /// variable to contain random value
    private int whereToSpawn;

    /// temporal variable to save what we spawned
    int lastSpawned;

    /// variable to declare when to spawn
    private float whenToSpawn;
    

    private void Start()
    {
        filledPoints = new bool [spawnPoints.Length];

        for (int i = 0; i < filledPoints.Length; i++)
        {
            filledPoints[i] = false;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].SetActive(false);
        }
    }


    void FixedUpdate()
    {
        if (Time.time > nextSpawn) 
        {
            if (OnePerSpawnPoint == true && allFilled == true) { return; } ///if all spawnpoints filled, no need to continue

            else if (OnePerSpawnPoint == true && allFilled == false)
            {
                bool tmp = false;
                while (tmp == false)
                {
                    whereToSpawn = Random.Range(0, spawnPoints.Length);
                    if (filledPoints[whereToSpawn] == false)
                    {
                        tmp = true;
                        filledPoints[whereToSpawn] = true;

                        ///test if all spawnpoints are filled, so that it doesnt spawn anymore
                        bool tmp2 = true;
                        for (int i = 0; i < filledPoints.Length; i++)
                        {
                            if (filledPoints[i] == false) { tmp2 = false; }
                        }
                        if (tmp2 == true) { allFilled = true; }
                    }
                }
            }
            else if (OnePerSpawnPoint == false)
            {
                whereToSpawn = Random.Range(0, spawnPoints.Length);
            }


            whatToSpawn = Random.Range(1, 3);

            whenToSpawn = Random.Range(spawnMinRate, spawnMaxRate); 

            while (whatToSpawn == lastSpawned)
            {
                whatToSpawn = Random.Range(1, 3);
            }
            lastSpawned = whatToSpawn;

            //SPAWN
            GameObject pick = Instantiate(PickUp, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
            //Debug.Log(spawnPoints[whereToSpawn].transform.position);

            ///getter script from instantiated pickup
            PickObject = pick.GetComponentInChildren<PickUpScript>();

            if (whatToSpawn == 1)
            {
                PickObject.picktype = PickTypes.Sword;
            }
            else if (whatToSpawn == 2)
            {
                PickObject.picktype = PickTypes.Axe;
            }
            else if (whatToSpawn == 3)
            {
                PickObject.picktype = PickTypes.Spear;
            }
            else if (whatToSpawn == 4)
            {
                PickObject.picktype = PickTypes.Bow;
            }
            else if (whatToSpawn == 5)
            {
                PickObject.picktype = PickTypes.CrossBow;
            }
            else if (whatToSpawn == 6)
            {
                PickObject.picktype = PickTypes.Boomerang;
            }

            ///set this object in the RandomSpawner variable
            PickObject.RadomSpawner = this.gameObject;

            ///set array filledPoints actual pos, so that when it is picked it sets the pos as true
            PickObject.numFilled = whereToSpawn;

            /// set next spawn time
            nextSpawn = Time.time + whenToSpawn;
        }
    }

    public void RestartFilledArray()
    {
        for (int i = 0; i < filledPoints.Length; i++)
        {
            filledPoints[i] = false;
        }           
        allFilled = false;
    }

    public void RecalculateNext()
    {
        nextSpawn = Time.time + whenToSpawn;
    }
}
