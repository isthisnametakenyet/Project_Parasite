using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour
{
    public GameObject Picker;
    public Sprite crossbowSprite;

    public bool Idle;
    public bool Attack = false;
    public bool Charging = false;
    public bool Thrown = false;
    public bool Landed = false;
    public int Uses = 2;
    private bool inUse = false;

    //VARIABLES
    public float AttackTime = 2f;
    private float actualTime = 0f;

    BoxCollider2D collider2D;
    Rigidbody2D body2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        Idle = true;
    }

    void FixedUpdate()
    {
        //Debug.Log("tis but an axe");
        if (Uses == 0 && inUse == false) { Destroy(gameObject); } //AUTODESTRUCCION
        else if (Idle == true && collider2D.enabled == true && inUse == false && Thrown == false && Landed == false)
        {
            Debug.Log("Wp: Idle");
            actualTime = 0f;
            collider2D.enabled = false;
            transform.gameObject.tag = "Weapon";
            //START ANIMATION IDLE
        }
        else if (Attack == true && inUse == false && Thrown == false)
        {
            Debug.Log("Wp: Attack");
            collider2D.enabled = true;
            transform.gameObject.tag = "Attacking";
            //START ANIMATION ATTACKING
            actualTime = 0f;
            inUse = true;
            Idle = false;
            Uses--;
        }
        else if (Charging == true && inUse == false && Thrown == false)
        {
            Debug.Log("Wp: Charging");
            //BEFORE THIS, IDLE, ALLWAYS
            collider2D.enabled = false;
            //START ANIMATION CHARGING
            inUse = true;
        }
        else if (Thrown == true && Landed == false)
        {
            Debug.Log("Wp: Thrown");
            Charging = false;
            Idle = false;
            inUse = false;
            collider2D.enabled = true;
            transform.gameObject.tag = "Throwing";
            //START ANIMATION THROW
        }

        if (actualTime <= AttackTime && inUse == true && Charging == false) { actualTime += Time.deltaTime; } //TIEMPO Q DURA LA ANIMACION D ATAQUE
        else if (Thrown == false) { inUse = false; Idle = true; Attack = false; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("Wp: Landed");
            Uses--;
            Landed = true;
            Thrown = false;
        }

        if (collision.gameObject.tag == "EmptyBody")
        {
            Debug.Log("Wp: Landed");
            Uses--;
            Landed = true;
            Thrown = false;
        }
    }
}