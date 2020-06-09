using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class HeadThrow : MonoBehaviour
{
    public Controller ParasiterController = Controller.NONE;


    private Player player;

    #region InstanceVariables
    //GAMEOBJECTS
    public GameObject OriginalBody;
    public GameObject ParasitedBody;
    public GameObject HeadFallPrefab;

    //SCRIPTS
    private PlayerController2D playerScript;
    private PlayerController2D parasitedScript;
    private HeadReturn headFall;

    //VARIABLES
    public float floorStunMax = 2f;
    public float expulsedStunMax = 3f;
    private float actualStun = 0;

    public float expulsedStrengh = 3f;

    [HideInInspector] public bool Parasiting = false;

    private bool BadThrow = false;
    [HideInInspector] public bool Expulsed = false;
    [HideInInspector] public bool canReturn = false;
    #endregion

    //COMPONENTS
    SpriteRenderer thisspriteRenderer;
    Rigidbody2D thisbody2D;
    CircleCollider2D thiscollider2D;

    void Start()
    {
        canReturn = false; //important

        //SETTERS
        thisspriteRenderer = GetComponent<SpriteRenderer>();
        thisbody2D = GetComponent<Rigidbody2D>();
        thiscollider2D = GetComponent<CircleCollider2D>();

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
        Debug.Log("canReturn: " + canReturn);
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


        //GOBACK
        if (player.GetButtonUp("HeadThrow&Return") && canReturn == true)
        {
            GoBack();
        }
    }



    //EXPULSE
    /// Function executed if this HeadThrow is parasiting an enemy and the enemy gets back to their body
    public void Expulse()
    {
        SoundManager.instance.Play("Parasit");
        Debug.Log("Head Expulsed");
        actualStun = 0;

        thisspriteRenderer.enabled = true;
        thiscollider2D.enabled = true;

        if (playerScript.facingright == true) //THROW HEAD with headCharge as force
        {
            thisbody2D.velocity = new Vector2(expulsedStrengh * 4f, 3f);
        }
        else if (playerScript.facingright == false)
        {
            thisbody2D.velocity = new Vector2(-expulsedStrengh * 4f, 3f);
        }
    }

    //GOBACK
    /// Function executed if this HeadThrow gets back to their original body 
    public void GoBack()
    {
        Debug.Log("Head Throw: Return");
        SoundManager.instance.Play("Parasit");
        if (Parasiting == true)
        {
            parasitedScript = ParasitedBody.GetComponent<PlayerController2D>();
            parasitedScript.RunHead();
        }

        playerScript.ReturnHead();

        Destroy(gameObject); //AUTODESTRUCCION
    }



    //COLLISIONS
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" && Parasiting == false)
        {
            BadThrow = true;
            thiscollider2D.isTrigger = false;
            Debug.Log("Head Thrown Hits Ground");
        }

        if (collision.gameObject.tag == "Player" && Parasiting == false && collision.gameObject != OriginalBody)
        {
            Debug.Log("Head Thrown Hits Player");
            parasitedScript = collision.gameObject.GetComponent<PlayerController2D>();
            if (parasitedScript.Parasitable == true)
            {
                Debug.Log("Head Thrown Parasites Player");
                parasitedScript.Parasite(this.gameObject);
                ParasitedBody = collision.gameObject;
                Parasiting = true;

                ///Pause unnecesary components
                thisspriteRenderer.enabled = false;
                thiscollider2D.enabled = false;
            }
        }
    }
}
