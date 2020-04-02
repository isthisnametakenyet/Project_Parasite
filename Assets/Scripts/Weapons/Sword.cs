using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject Picker;
    private PlayerController2D pickerPlayerScript;
    private EmptyBody pickerEmptyScript;
    private PlayerController2D playerScript;
    private EmptyBody emptyScript;

    //STATE
    public bool Idle;
    public bool Attack = false;
    public bool Charging = false;
    public bool Thrown = false;
    public bool Landed = false;
    public bool Stuck = false;
    public bool Drop = false;
    private bool inUse = false;

    //VARIABLES
    public int Uses = 2;
    public float AttackTime = 2f;
    private float actualAttack = 0f;
    public float stuckTime = 0.5f;
    public float actualStuck = 0f;
    BoxCollider2D collider2D;
    Rigidbody2D body2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        if (Picker.gameObject.tag == "Player") { pickerPlayerScript = Picker.GetComponent<PlayerController2D>(); }
        else if (Picker.gameObject.tag == "EmptyBody") { pickerEmptyScript = Picker.GetComponent<EmptyBody>(); }
        Idle = true;
        Thrown = false;
        Attack = false;
        collider2D.isTrigger = true;
        body2D.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (Drop == true)
        {
            Idle = false;
        }
        else if (Uses == 0 && inUse == false)
        {
            if (Picker.gameObject.tag == "Player") { pickerPlayerScript.isWeaponed = false; }

            else if (Picker.gameObject.tag == "EmptyBody") { pickerEmptyScript.isWeaponed = false; }

            Destroy(gameObject); //AUTODESTRUCCION
        }
        else if (Idle == true && inUse == false && Thrown == false && Attack == false && Landed == false)
        {
            Debug.Log("Wp: Idle");
            actualAttack = 0f;
            collider2D.enabled = false;
            transform.gameObject.tag = "Weapon";
            //START ANIMATION IDLE
        }

        if (Attack == true && inUse == false)
        {
            Debug.Log("Wp: Attack");
            collider2D.enabled = true;
            transform.gameObject.tag = "Attacking";
            //START ANIMATION ATTACKING
            actualAttack = 0f;
            inUse = true;
            Idle = false;
            Uses--;
        }

        if (Charging == true && inUse == false && Thrown == false)
        {
            Debug.Log("Wp: Charging");
            //BEFORE THIS, IDLE, ALLWAYS
            collider2D.enabled = false;
            //START ANIMATION CHARGING
            inUse = true;
        }
        else if (Thrown == true)
        {
            Debug.Log("Wp: Thrown");
            Charging = false;
            Idle = false;
            inUse = false;
            collider2D.enabled = true;
            transform.gameObject.tag = "Throwing";
            //START ANIMATION THROW
        }

        if (actualAttack <= AttackTime && inUse == true && Charging == false) { actualAttack += Time.deltaTime; }
        else if (Thrown == false && Stuck == false) { inUse = false; Idle = true; Attack = false; } //TIEMPO Q DURA LA ANIMACION D ATAQUE

        //SI GOLPEA ALGO SE PARA Y SE QUEDA PEGADO
        if (actualStuck >= stuckTime)
        {
            Debug.Log("Wp: Stuck");
            //Stuck = true;
            Thrown = false;
            body2D.velocity = new Vector2(0, 0);
            Uses--;
            actualStuck = 0f;
         }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" && Thrown == true)
        {
            Debug.Log("Wp: Landed");
            actualStuck += Time.deltaTime * 10;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        }
        else if (collision.gameObject.tag == "Floor" && Drop == true)
        {
            Debug.Log("Dropped");
            body2D.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            body2D.velocity = new Vector2(0, 0);
            transform.gameObject.tag = "Stuck";
        }

        if (collision.gameObject.tag == "Player" && Thrown == true)
        {
            playerScript = collision.GetComponent<PlayerController2D>();
            if (Picker.gameObject.tag == "Player" && playerScript.controller != pickerPlayerScript.controller)
            {
                Debug.Log("Wp: Hit");
                actualStuck += Time.deltaTime * 10;
                transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                this.transform.parent = collision.transform;
            }
        }

        if (collision.gameObject.tag == "EmptyBody" && Thrown == true)
        {
            emptyScript = collision.GetComponent<EmptyBody>();
            if (Picker.gameObject.tag == "EmptyBody" && emptyScript.controller != pickerEmptyScript.controller)
            {
                Debug.Log("Wp: Hit");
                actualStuck += Time.deltaTime * 10;
                transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                this.transform.parent = collision.transform;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && Thrown == true)
        {
            Debug.Log("Wp: Landed");
            actualStuck += Time.deltaTime * 10;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        }
    }
}