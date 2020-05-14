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

    public int numSpawned = 0;
    public int maxNumSpawned;

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
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].SetActive(false);
        }
    }


    void Update()
    {
        if (numSpawned < maxNumSpawned)
        {
            if (Time.time > nextSpawn) 
            {
                whatToSpawn = Random.Range(1, 3); 

                whereToSpawn = Random.Range(0, spawnPoints.Length + 1); 

                whenToSpawn = Random.Range(spawnMinRate, spawnMaxRate); 

                while (whatToSpawn == lastSpawned)
                {
                    whatToSpawn = Random.Range(1, 3);
                }
                lastSpawned = whatToSpawn;

                numSpawned++;

                //SPAWN
                GameObject pick = Instantiate(PickUp, spawnPoints[whereToSpawn].transform.position, Quaternion.identity);
                if (whatToSpawn == 1)
                {
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Sword;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 2)
                {
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Axe;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 3)
                {
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Spear;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 4)
                {
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Bow;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 5)
                {
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.CrossBow;
                    PickObject.RadomSpawner = this.gameObject;
                }
                else if (whatToSpawn == 6)
                {
                    PickObject = pick.GetComponent<PickUpScript>();
                    PickObject.picktype = PickTypes.Boomerang;
                    PickObject.RadomSpawner = this.gameObject;
                }

                /// set next spawn time
                nextSpawn = Time.time + whenToSpawn;
            }
        }
    }
}
