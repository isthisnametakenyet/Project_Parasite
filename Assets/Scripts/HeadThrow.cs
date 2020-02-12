using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class HeadThrow : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    private Player player;

    public GameObject OriginalBody;
    public GameObject PlayerAll;
    public GameObject Head;
    public GameObject Body;
    private PlayerController2D playerAll;
    private ParasiteHead parasiteHead;
    private EmptyBody collisionScript;

    bool BadThrow = false;

    void Start()
    {
        switch (controller)
        {
            case Controller.PLAYER0:
                player = ReInput.players.GetPlayer(3);
                break;

            case Controller.PLAYER1:
                player = ReInput.players.GetPlayer(0);
            break;

            case Controller.PLAYER2:
                player = ReInput.players.GetPlayer(1);
            break;

            case Controller.PLAYER3:
                player = ReInput.players.GetPlayer(2);
            break;
        }
    }

    private void FixedUpdate()
    {
        if (player.GetButtonDown("Head Return") && BadThrow == true)
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

        if (collision.gameObject.tag == "EmptyBody")
        {
            collisionScript = collision.gameObject.GetComponent<EmptyBody>();

            if (collisionScript.controller != this.controller)
            {
                this.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y, 0);
                GameObject parasite = Instantiate(Head, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);

                collisionScript.controller = this.controller;
                collisionScript.ParasiteBody = OriginalBody;

                parasiteHead = parasite.GetComponent<ParasiteHead>();
                parasiteHead.skin = this.skin;
                parasite.transform.parent = collisionScript.transform;

                Destroy(gameObject); //AUTODESTRUCCION
            }
        }

        if (collision.gameObject.tag == "Floor")
        {
            BadThrow = true;
            Debug.Log("Head Thrown Hits Ground");
        }
    }
}
