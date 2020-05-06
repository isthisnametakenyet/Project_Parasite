using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { MELEE, RANGED, X };

public class WeaponScript : MonoBehaviour
{
    public WeaponType type = WeaponType.X;

    public GameObject Picker;
    private PlayerController2D pickerPlayerScript;
    private EmptyBody pickerEmptyScript;
    private PlayerController2D playerScript;
    private EmptyBody emptyScript;

    public GameObject prefabWeaponArrow;

    private Sword swordScript;
    private Axe axeScript;
    private Spear spearScript;
    private Arrow arrowScript;

    //STATE
    public bool Attack = false;
    public bool Charging = false;
    public bool Thrown = false;
    private int pastState = 0;

    //VARIABLES
    public int Uses = 2;
    BoxCollider2D collider2D;
    Rigidbody2D body2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        if (Picker.gameObject.tag == "Player") { pickerPlayerScript = Picker.GetComponent<PlayerController2D>(); }
        else if (Picker.gameObject.tag == "EmptyBody") { pickerEmptyScript = Picker.GetComponent<EmptyBody>(); }
        collider2D.isTrigger = true;
    }

    void FixedUpdate()
    {
        if (Uses == 0)
        {
            if (Picker.gameObject.tag == "Player") { pickerPlayerScript.isWeaponed = false; }
            else if (Picker.gameObject.tag == "EmptyBody") { pickerEmptyScript.isWeaponed = false; }

            //Destroy(gameObject); //AUTODESTRUCCION
        }
        else if (Attack == true && pastState != 1)
        {
            pastState = 1;
            collider2D.enabled = true;
            switch (type)
            {
                case WeaponType.MELEE:
                    Uses--;
                    transform.gameObject.tag = "Attacking";
                    break;
                case WeaponType.RANGED:
                    break;
                case WeaponType.X:
                    break;
                default:
                    break;
            }

        }
        else if (Charging == true && pastState != 2)
        {
            collider2D.enabled = false;
            transform.gameObject.tag = "Weapon";
        }
        else if (Thrown == true && pastState != 3)
        {
            collider2D.enabled = true;
            GameObject throwed = Instantiate(prefabWeaponArrow, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            switch (this.gameObject.name)
            {
                case "Axe":
                    Debug.Log("Axe");
                    axeScript = throwed.GetComponent<Axe>();
                    axeScript.Picker = Picker;
                    break;
                default:
                    Debug.Log("wut");
                    break;
            }
        }
        else { pastState = 0; }
    }
}