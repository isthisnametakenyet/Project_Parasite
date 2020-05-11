using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { MELEE, RANGED, X };

public class WeaponScript : MonoBehaviour
{
    public WeaponType type = WeaponType.X;

    public GameObject Picker;
    public PlayerController2D pickerPlayerScript;
    private PlayerController2D playerScript;

    //PREFABS
    public GameObject prefabDrop;
    public GameObject prefabThrow;

    //WEAPON SCRIPTS
    private MeleeScript meleeScript;
    private Arrow arrowScript;

    //STATE
    public bool Attack = false;
    public bool Charging = false;
    public bool Thrown = false;
    private int pastState = 0;

    //VARIABLES
    public int originalUses;
    public int Uses;
    public float throwWeaponSpeed = 10f;

    //GETTERS
    BoxCollider2D collider2D;
    Rigidbody2D body2D;
    SpriteRenderer renderer;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        pickerPlayerScript = Picker.GetComponent<PlayerController2D>();
        //collider2D.isTrigger = true;
        originalUses = Uses;

        Uses = originalUses;
    }

    void FixedUpdate()
    {
        if (Uses == 0)
        {
            pickerPlayerScript.isWeaponed = false;
            transform.gameObject.tag = "Weapon";

            this.gameObject.SetActive(false); //HIDE SELF
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
                    transform.gameObject.tag = "Slap";
                    break;
                case WeaponType.X:
                    break;
                default:
                    break;
            }

        }
        else if (Charging == true && pastState != 2)
        {
            pastState = 2;
            transform.gameObject.tag = "Weapon";
        }
        else if (Thrown == true && Uses != -1)
        {
            pastState = 3;
            GameObject throwed = Instantiate(prefabThrow, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            body2D = throwed.GetComponent<Rigidbody2D>();
            renderer = throwed.GetComponent<SpriteRenderer>();

            if (pickerPlayerScript.facingright == true) //THROW WEAPON with headCharge as force
            {
                body2D.velocity = new Vector2(throwWeaponSpeed, 0);
            }
            else if (pickerPlayerScript.facingright == false)
            {
                body2D.velocity = new Vector2(-throwWeaponSpeed, 0);
                renderer.flipX = true;
            }

            switch (type)
            {
                case WeaponType.MELEE:
                    Debug.Log("Melee");
                    meleeScript = throwed.GetComponent<MeleeScript>();
                    meleeScript.Picker = Picker;
                    meleeScript.Uses = Uses;
                    meleeScript.Thrown = true;
                    break;
                case WeaponType.RANGED:
                    Debug.Log("Arrow");
                    arrowScript = throwed.GetComponent<Arrow>();
                    arrowScript.Picker = Picker;
                    break;
                case WeaponType.X:
                    break;
                default:
                    Debug.Log("wut");
                    break;
            }
            Uses = -1;
            this.gameObject.SetActive(false); //DESACTIVATE
        }
        else if (pastState != 0) { pastState = 0; }
        else if (Attack== false && Charging == false && Thrown == false)
        {
            collider2D.enabled = false;
            transform.gameObject.tag = "Weapon";
        }
    }

    public void Pickup()
    {
        Uses = originalUses;
    }

    public void Drop()
    {
        GameObject dropped = Instantiate(prefabDrop, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

        switch (type)
        {
            case WeaponType.MELEE:
                meleeScript = dropped.GetComponent<MeleeScript>();
                meleeScript.Uses = Uses;
                meleeScript.Idle = true;
                break;
            case WeaponType.RANGED:
                break;
            case WeaponType.X:
                break;
            default:
                Debug.Log("wut");
                break;
        }
    }
}