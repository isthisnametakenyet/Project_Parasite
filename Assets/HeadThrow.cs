using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class HeadThrow : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    private Player player;

    public GameObject BodyEmpty;
    public GameObject PlayerAll;
    private PlayerController2D playerAll;

    bool BadThrow = false;

    void Start()
    {
        switch (controller)
        {
            case Controller.PLAYER1:
                player = ReInput.players.GetPlayer(0);
            break;

            case Controller.PLAYER2:
                player = ReInput.players.GetPlayer(1);
            break;

            case Controller.PLAYER3:
                player = ReInput.players.GetPlayer(2);
            break;

            case Controller.PLAYER4:
                player = ReInput.players.GetPlayer(3);
            break;
        }
    }

    private void FixedUpdate()
    {
        if (player.GetButtonDown("Head Return") && BadThrow == true)
        {
            Debug.Log("fk u");
            this.transform.position = new Vector3(BodyEmpty.transform.position.x, BodyEmpty.transform.position.y, 0);
            GameObject player = Instantiate(PlayerAll, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            playerAll = player.GetComponent<PlayerController2D>();
            playerAll.controller = this.controller;
            playerAll.skin = this.skin;

            Destroy(BodyEmpty); 
            Destroy(gameObject); //AUTODESTRUCCION
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EmptyBody")
        {

        }

        if (collision.gameObject.tag == "Floor")
        {
            BadThrow = true;
            Debug.Log("Head Thrown Hits Ground");
        }
    }
}
