using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class HeadThrow : MonoBehaviour
{
    public Controller ParasiterController = Controller.NONE;

    SpriteRenderer spriteRenderer;

    private Player player;
    Rigidbody2D body2D;
    BoxCollider2D collider2D;

    public GameObject OriginalBody;
    private PlayerController2D playerScript;
    public GameObject HeadDead;
    private HeadReturn headFall;

    //VARIABLES
    public float floorStunMax = 2f;
    public float expulsedStunMax = 3f;
    private float actualStun = 0;

    private bool Parasiting = false;
    private bool BadThrow = false;

    public bool Expulsed = false;
    public bool GoBack = false;
    public bool canReturn = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();

        switch (ParasiterController)
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

        playerScript = OriginalBody.GetComponent<PlayerController2D>();
    }

    private void FixedUpdate()
    {
        if (OriginalBody == null) //PLAYER IS DEAD
        {
            GameObject headDead = Instantiate(HeadDead, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            headFall = headDead.GetComponent<HeadReturn>();
            headFall.isDead = true;
            Destroy(gameObject); //AUTODESTRUCCION
        }

        //RETURN
        if (BadThrow == true && actualStun < floorStunMax) { actualStun += Time.deltaTime; canReturn = false; }
        else if (actualStun >= floorStunMax) { canReturn = true; }

        if (Expulsed == true && actualStun < expulsedStunMax) { actualStun += Time.deltaTime; canReturn = false; }
        else if (actualStun >= expulsedStunMax) { canReturn = true; }

        if (player.GetAxis("HeadThrow&Return") > 0 && canReturn == true || GoBack == true && canReturn == true) 
        {
            Debug.Log("Head Throw: Return");
            //this.transform.position = new Vector3(OriginalBody.transform.position.x, OriginalBody.transform.position.y, 0);
            playerScript.ReturnHead();

            Destroy(gameObject); //AUTODESTRUCCION
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" && Parasiting == false)
        {
            BadThrow = true;
            Debug.Log("Head Thrown Hits Ground");
        }
    }
}
