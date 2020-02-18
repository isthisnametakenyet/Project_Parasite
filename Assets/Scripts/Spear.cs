using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public GameObject Picker;
    public Sprite spearSprite;

    //STATE
    public bool Idle;
    public bool Attack = false;
    public bool Charging = false;
    public bool Thrown = false;
    public bool Landed = false;
    public bool Stuck = false;
    private bool inUse = false;

    //VARIABLES
    public int Uses = 2;
    public float AttackTime = 2f;
    private float actualAttack = 0f;
    public float stuckTime = 0.5f;
    private float actualStuck = 0f;

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
            actualAttack = 0f;
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
            actualAttack = 0f;
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
        else if (Thrown == true && Stuck == false)
        {
            Debug.Log("Wp: Thrown");
            Charging = false;
            Idle = false;
            inUse = false;
            collider2D.enabled = true;
            collider2D.isTrigger = true;
            transform.gameObject.tag = "Throwing";
            //START ANIMATION THROW
        }
        if (actualAttack <= AttackTime && inUse == true && Charging == false) { actualAttack += Time.deltaTime; }
        else if (Thrown == false) { inUse = false; Idle = true; Attack = false; } //TIEMPO Q DURA LA ANIMACION D ATAQUE

        //SI GOLPEA ALGO SE PARA Y SE QUEDA PEGADO
        if (actualStuck >= stuckTime)
        {
            Stuck = true;
            Thrown = false;
            body2D.velocity = new Vector2(0, 0);
            Uses--;
            body2D.isKinematic = true;
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

        if (collision.gameObject.tag == "Player" && collision.gameObject != Picker && Thrown == true)
        {
            Debug.Log("Wp: Hit");
            actualStuck += Time.deltaTime * 10;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
            this.transform.parent = collision.transform;
        }

        if (collision.gameObject.tag == "EmptyBody" && collision.gameObject != Picker && Thrown == true)
        {
            Debug.Log("Wp: Hit");
            actualStuck += Time.deltaTime * 10;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
            this.transform.parent = collision.transform;
        }
    }
}