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
    CircleCollider2D collider2D;

    //GAMEOBJECTS
    public GameObject OriginalBody;
    public GameObject ParasitedBody;
    public GameObject HeadFallPrefab;

    //SCRIPTS
    private PlayerController2D playerScript;
    private HeadReturn headFall;

    //VARIABLES
    public float floorStunMax = 2f;
    public float expulsedStunMax = 3f;
    private float actualStun = 0;

    private bool Parasiting = false;

    private bool BadThrow = false;
    public bool Expulsed = false;
    public bool canReturn = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CircleCollider2D>();

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
            Debug.Log("Player" + ParasiterController + "is Dead");
            GameObject headDead = Instantiate(HeadFallPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            headFall = headDead.GetComponent<HeadReturn>();
            headFall.isDead = true;
            Destroy(gameObject); //AUTODESTRUCCION
        }

        //TEMPS
        if (BadThrow == true && actualStun < floorStunMax) { actualStun += Time.deltaTime; canReturn = false; }
        else if (actualStun >= floorStunMax) { canReturn = true; }

        if (Expulsed == true && actualStun < expulsedStunMax) { actualStun += Time.deltaTime; canReturn = false; }
        else if (actualStun >= expulsedStunMax) { canReturn = true; }

        if (player.GetAxis("HeadThrow&Return") > 0 && canReturn == true)
        {
                GoBack();
        }
    }



    //EXPULSE
    /// Function executed if this HeadThrow is parasiting an enemy and the enemy gets back to their body
    public void Expulse()
    {
        Debug.Log("Head Expulsed");
        actualStun = 0;

    }

    //GOBACK
    /// Function executed if this HeadThrow gets back to their original body
    public void GoBack()
    {
        Debug.Log("Head Throw: Return");
        playerScript = ParasitedBody.GetComponent<PlayerController2D>();
        playerScript.GoBack();

        playerScript = OriginalBody.GetComponent<PlayerController2D>();
        playerScript.ReturnHead();

        Destroy(gameObject); //AUTODESTRUCCION
    }



    //COLLISIONS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" && Parasiting == false)
        {
            BadThrow = true;
            collider2D.isTrigger = false;
            Debug.Log("Head Thrown Hits Ground");
        }

        if (collision.gameObject.tag == "Player" && Parasiting == false)
        {
            Debug.Log("Head Thrown Hits Player");
            playerScript = collision.gameObject.GetComponent<PlayerController2D>();
            if (playerScript.Parasitable == true)
            {
                Debug.Log("Head Thrown Parasites Player");
                playerScript.Parasite(this.gameObject);
                ParasitedBody = collision.gameObject;
            }
        }
    }
}
