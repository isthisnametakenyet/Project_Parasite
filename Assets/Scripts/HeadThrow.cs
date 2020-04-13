using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class HeadThrow : MonoBehaviour
{
    public Controller controller = Controller.NONE;

    SpriteRenderer spriteRenderer;

    private Player player;
    Rigidbody2D body2D;
    BoxCollider2D collider2D;

    public GameObject OriginalBody;
    public GameObject PlayerArmless;
    public GameObject HeadDead;
    private PlayerController2D playerScript;
    private EmptyBody collisionScript;
    private EmptyBody returnScript;

    private bool Parasiting = false;
    private bool BadThrow = false;
    public float floorStunMax = 2f;
    public float expulsedStunMax = 3f;
    private float actualStun = 0;
    public bool Expulsed = false;
    public bool GoBack = false;
    public bool canReturn = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();

        switch (controller)
        {
            case Controller.PLAYER0:
                player = ReInput.players.GetPlayer(0);
                break;

            case Controller.PLAYER1:
                player = ReInput.players.GetPlayer(1);
                break;

            case Controller.PLAYER2:
                player = ReInput.players.GetPlayer(2);
                break;

            case Controller.PLAYER3:
                player = ReInput.players.GetPlayer(3);
                break;
        }

        returnScript = OriginalBody.gameObject.GetComponent<EmptyBody>();
    }

    private void FixedUpdate()
    {
        if (OriginalBody == null)
        {
            GameObject headDead = Instantiate(HeadDead, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            Destroy(gameObject); //AUTODESTRUCCION
        }

        //RETURN TO EMPTY
        if (BadThrow == true && actualStun < floorStunMax) { actualStun += Time.deltaTime; canReturn = false; }
        else if (actualStun >= floorStunMax) { canReturn = true; }

        if (Expulsed == true && actualStun < expulsedStunMax) { actualStun += Time.deltaTime; canReturn = false; }
        else if (actualStun >= expulsedStunMax) { canReturn = true; }

        if (player.GetAxis("HeadThrow&Return") > 0 && canReturn == true && returnScript.parasited == false || GoBack == true && returnScript.parasited == false) 
        {
            this.transform.position = new Vector3(OriginalBody.transform.position.x, OriginalBody.transform.position.y, 0);
            GameObject playerBody = Instantiate(PlayerArmless, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            playerScript = playerBody.GetComponent<PlayerController2D>();
            playerScript.controller = this.controller;
            returnScript = OriginalBody.GetComponent<EmptyBody>();
            playerScript.arms = returnScript.arms;

            playerScript.LeftArm = returnScript.LeftArm;
            returnScript.LeftArm.transform.parent = playerScript.transform;
            playerScript.RightArm = returnScript.RightArm;
            returnScript.RightArm.transform.parent = playerScript.transform;

            if (OriginalBody.GetComponent<SpriteRenderer>().flipX == true)
            {
                playerScript.LeftArm.transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y - 0.1f, 0);
                playerScript.RightArm.transform.position = new Vector3(transform.position.x + 0.75f, transform.position.y - 0.1f, 0);
                playerScript.RightArm.GetComponent<SpriteRenderer>().flipX = true;
                playerScript.LeftArm.GetComponent<SpriteRenderer>().flipX = true;
            }

            Destroy(OriginalBody);
            Destroy(gameObject); //AUTODESTRUCCION
        }
        else if (player.GetAxis("HeadThrow&Return") > 0 && canReturn == true && returnScript.parasited == true || GoBack == true && returnScript.parasited == true)
        {//RETURN TO PARASITED
            Debug.Log("Return to parasited body----");
            returnScript.expulseParasite = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionScript = collision.gameObject.GetComponent<EmptyBody>();

        if (collision.gameObject.tag == "EmptyBody" && collisionScript.parasited == false && Expulsed == false && Parasiting == false)
        {
            if (collisionScript.controller != this.controller)
            {
                collisionScript.controller = this.controller;
                collisionScript.parasited = true;
                collisionScript.Parasite = this.gameObject;

                Parasiting = true;
                body2D.isKinematic = true;
                body2D.Sleep();
                collider2D.enabled = false;

                this.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y+0.6f, 0);
                this.transform.parent = collisionScript.transform;
                body2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (collision.gameObject.tag == "Floor" && Parasiting == false)
        {
            BadThrow = true;
            Debug.Log("Head Thrown Hits Ground");
        }
    }
}
