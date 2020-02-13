using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class HeadThrow : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;
    public Sprite NONE;
    SpriteRenderer spriteRenderer;

    private Player player;
    Rigidbody2D body2D;

    public GameObject OriginalBody;
    public GameObject PlayerAll;
    private PlayerController2D playerAll;
    private EmptyBody collisionScript;

    private bool Parasiting = false;
    private bool BadThrow = false;
    public bool Expulsed = false;
    public bool GoBack = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body2D = GetComponent<Rigidbody2D>();

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

        switch (skin)
        {
            case Skin.NONE:
                spriteRenderer.sprite = NONE;
                break;
                //case Skin.SKIN1:
                //    spriteRenderer.sprite = Skin1;
                //    break;
                //case Skin.SKIN2:
                //    spriteRenderer.sprite = Skin2;
                //    break;
        }
    }

    private void FixedUpdate()
    {
        if (player.GetButtonDown("Head Return") && BadThrow == true && Parasiting == false || GoBack == true)
        {
            Debug.Log("d vuelta");
            this.transform.position = new Vector3(OriginalBody.transform.position.x, OriginalBody.transform.position.y, 0);
            GameObject player = Instantiate(PlayerAll, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            playerAll = player.GetComponent<PlayerController2D>();
            playerAll.controller = this.controller;
            playerAll.skin = this.skin;

            Destroy(OriginalBody);
            Destroy(gameObject); //AUTODESTRUCCION
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "EmptyBody" && Parasiting == false && Expulsed == false)
        {
            collisionScript = collision.gameObject.GetComponent<EmptyBody>();

            if (collisionScript.controller != this.controller)
            {
                collisionScript.controller = this.controller;
                collisionScript.parasited = true;

                body2D.bodyType = RigidbodyType2D.Kinematic;
                this.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y+0.6f, 0);
                this.transform.parent = collisionScript.transform;
                body2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Parasiting = true;
            }
        }

        if (collision.gameObject.tag == "Floor" && Parasiting == false)
        {
            BadThrow = true;
            Debug.Log("Head Thrown Hits Ground");
        }
    }
}
