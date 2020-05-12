using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    public GameObject Picker;
    private PlayerController2D playerScript;

    //STATES
    public bool Idle = false;
    public bool Thrown = false;
    public bool Landed = false;

    //VARIABLES
    public int Uses = 2;
    public float stuckTime = 0.3f;
    public float actualStuck = 0f;

    //GETTERS
    BoxCollider2D collider2D;
    Rigidbody2D body2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        collider2D.isTrigger = true;
        body2D.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (Uses == 0) { Destroy(gameObject); } //AUTODESTRUCCION
        else if (Idle == true && Thrown == false && Landed == false)
        {
            Debug.Log("Wp: Idle");
            collider2D.enabled = false;
            transform.gameObject.tag = "Stuck";
            //START ANIMATION IDLE
        }
        else if (Thrown == true)
        {
            Debug.Log("Wp: Thrown");
            collider2D.enabled = true;
            transform.gameObject.tag = "Throwing";
            //START ANIMATION THROW
        }

        //TODO: SI GOLPEA ALGO SE PARA Y SE QUEDA PEGADO
        if (actualStuck >= stuckTime)
        {
            Debug.Log("Wp: Stuck");
            Thrown = false;
            body2D.velocity = new Vector2(0, 0);
            Uses--;
            actualStuck = 0f;

            Destroy(gameObject); //AUTODESTRUCCION
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" && Thrown == true)
        {
            Debug.Log("Wp: Landad");
            actualStuck += Time.deltaTime * 10;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        }

        if (collision.gameObject.tag == "Player" && collision.gameObject != Picker && Thrown == true )
        {
            Debug.Log("Wp: HitTri");
            playerScript = collision.GetComponent<PlayerController2D>();
            actualStuck += Time.deltaTime * 10;

            playerScript.ThrowCollision(this.gameObject);
        }
    }
}