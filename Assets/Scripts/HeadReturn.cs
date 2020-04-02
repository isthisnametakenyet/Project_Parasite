using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public class HeadReturn : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    //public Skin skin = Skin.NONE;

    private Player player;

    public Sprite Skin1;
    public Sprite Skin2;
    public Sprite Skin3;

    public GameObject OriginalBody;
    public GameObject PlayerAll;
    private PlayerController2D playerAll;

    public float MaxStun = 2;
    float Wait = 0;
    public bool Stunned = false;
    public bool ParasiteReturn = false;
    public bool isDead = false;
    public bool Returning = false;

    private int BackID;

    Animator animator;
    Rigidbody2D body2D;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

            default:
                isDead = true;
                break;
        }

        //switch (skin)
        //{
        //    case Skin.SKIN1:
        //        spriteRenderer.sprite = Skin1;
        //        break;
        //    case Skin.SKIN2:
        //        spriteRenderer.sprite = Skin2;
        //        break;
        //    case Skin.SKIN3:
        //        spriteRenderer.sprite = Skin3;
        //        break;
        //}
    }

    void FixedUpdate()
    {
        if (isDead == true)
        {
            switch (controller)
            {
                case Controller.PLAYER0:
                    PlayerManager.Instance.isAlivePlayer1 = false;
                    break;
                case Controller.PLAYER1:
                    PlayerManager.Instance.isAlivePlayer2 = false;
                    break;
                case Controller.PLAYER2:
                    PlayerManager.Instance.isAlivePlayer3 = false;
                    break;
                case Controller.PLAYER3:
                    PlayerManager.Instance.isAlivePlayer4 = false;
                    break;
            }
            this.enabled = false;
        }

        if (Stunned == true && Wait < MaxStun)
        {
            Wait += Time.deltaTime * 1f;
            Debug.Log("Stun:" + Wait);
        }
        else { Stunned = false; }

        if (player.GetButtonDown("Head Return") && Stunned == false || Returning == true)
        {
            animator.Play(BackID);

            this.transform.position = new Vector3(OriginalBody.transform.position.x, OriginalBody.transform.position.y, 0);
            GameObject player = Instantiate(PlayerAll, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            playerAll = player.GetComponent<PlayerController2D>();
            playerAll.controller = this.controller;
            //playerAll.skin = this.skin;

            Destroy(OriginalBody);
            Destroy(gameObject); //AUTODESTRUCCION
        }
    }
}
