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
    public bool Attacking = false;
    public bool Thrown = false;
    public bool Off = false;
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

        originalUses = Uses;

        Uses = originalUses;
    }

    void FixedUpdate()
    {
        //BREAK
        if (Uses == 0)
        {
            pickerPlayerScript.isWeaponed = false;
            pickerPlayerScript.WeaponBreak();
            transform.gameObject.tag = "Weapon";

            this.gameObject.SetActive(false); //HIDE SELF
        }

        //THROW
        else if (Thrown == true && Uses > 0)
        {
            Thrown = false;
            Throw();
        }

        //ATTACK
        else if(Attacking == true && Uses > 0)
        {
            Attacking = false;
            Attack();
        }

        //DISABLE
        else if (Off == true)
        {
            Off = false;

            collider2D.enabled = false;
            transform.gameObject.tag = "Weapon";

            Debug.Log("Weapon Off");
            switch (type)
            {
                case WeaponType.MELEE:
                    Uses--;
                    break;
                case WeaponType.RANGED:
                    break;
                case WeaponType.X:
                    break;
                default:
                    break;
            }
        }
    }



    //ATTACK
    public void Attack()
    {
        collider2D.enabled = true;
        switch (type)
        {
            case WeaponType.MELEE:
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

    //THROW
    public void Throw()
    {
        GameObject throwed = Instantiate(prefabThrow, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        //throwed.transform.Rotate(0.0f, 0.0f, this.transform.rotation.z, Space.Self);

        int flipDir = 0;
        //Configure initial Position && Rotation
        switch (this.gameObject.name)
        {
            case "Sword":
                throwed.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
                flipDir = 2;
                break;
            case "Axe":
                if (pickerPlayerScript.facingright == true)
                {
                    throwed.transform.Rotate(0.0f, 0.0f, this.transform.rotation.z, Space.Self);
                }
                else if (pickerPlayerScript.facingright == false)
                {
                    throwed.transform.Rotate(0.0f, 0.0f, this.transform.rotation.z, Space.Self);
                }
                flipDir = 1;
                break;
            case "Spear":
                //throwed.transform.Rotate(0.0f, 0.0f, -90.0f, Space.Self);
                //flipDir = 1;
                break;
            case "Bow":
                break;
            case "CrossBow":
                break;
            case "Boomerang":
                break;
            default:
                Debug.Log("wut");
                break;
        }

        body2D = throwed.GetComponent<Rigidbody2D>();
        renderer = throwed.GetComponent<SpriteRenderer>();
        if (pickerPlayerScript.facingright == true)
        {
            body2D.velocity = new Vector2(throwWeaponSpeed, 0);
        }
        else if (pickerPlayerScript.facingright == false)
        {
            body2D.velocity = new Vector2(-throwWeaponSpeed, 0);
            if (flipDir == 1) { renderer.flipX = true; } //FLIP X
            else if (flipDir == 2) { renderer.flipY = true; } //FLIP Y
        }

        switch (type)
        {
            case WeaponType.MELEE:
                Debug.Log("WeaponScrp: Throw Melee");
                meleeScript = throwed.GetComponent<MeleeScript>();
                meleeScript.Picker = Picker;
                meleeScript.Uses = Uses;
                meleeScript.Thrown = true;
                break;
            case WeaponType.RANGED:
                Debug.Log("WeaponScrp: Throw Arrow");
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

    //PICKUP
    public void Pickup()
    {
        Uses = originalUses;
    }

    //DROP
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