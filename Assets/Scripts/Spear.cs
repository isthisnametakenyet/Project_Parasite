using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public GameObject Picker;
    public Sprite spearSprite;

    public bool Idle;
    public bool Attack = false;
    public bool Charging = false;
    public bool Thrown = false;

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
        if (Idle == true && collider2D.enabled == true)
        {
            collider2D.enabled = false;
            transform.gameObject.tag = "Weapon";
            //START ANIMATION IDLE
        }
        else if (Attack == true)
        {
            collider2D.enabled = true;
            transform.gameObject.tag = "Attacking";
            //START ANIMATION ATTACKING
        }
        else if (Charging == true)
        {
            //FIRST IDLE THEN THIS, ALLWAYS
            collider2D.enabled = false;
            //START ANIMATION CHARGING
        }
        else if (Thrown == true)
        {
            collider2D.enabled = true;
            transform.gameObject.tag = "Throwing";
            //START ANIMATION THROW
        }
    }
}
