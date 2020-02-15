using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public GameObject Picker;
    public Sprite axeSprite;

    public bool Idle;
    public bool Attack = false;
    public bool Charging = false;
    public bool Thrown = false;
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
       
        if (Idle == true && collider2D.enabled == true && inUse == false)
        {
            Debug.Log("Wp: Idle");
            actualTime = 0f;
            collider2D.enabled = false;
            transform.gameObject.tag = "Weapon";
            //START ANIMATION IDLE
        }
        else if (Attack == true && inUse == false)
        {
            Debug.Log("Wp: Attack");
            collider2D.enabled = true;
            transform.gameObject.tag = "Attacking";
            //START ANIMATION ATTACKING
            actualTime = 0f;
            inUse = true;
        }
        else if (Charging == true && inUse == false)
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
            collider2D.enabled = true;
            transform.gameObject.tag = "Throwing";
            //START ANIMATION THROW
        }

        if (actualTime <= AttackTime && inUse == true && Charging == false) { actualTime += Time.deltaTime; } //TIEMPO Q DURA LA ANIMACION D ATAQUE
        else { inUse = false; Idle = true; Attack = false; }
    }
}
