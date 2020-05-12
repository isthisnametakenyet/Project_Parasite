using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public class HeadReturn : MonoBehaviour
{
    public Controller controller = Controller.NONE;

    private Player player;

    //GAME OBJECTS
    public GameObject OriginalBody;
    private PlayerController2D playerAll;

    //VARAIBLES
    public float MaxStun = 2;
    float Wait = 0;
    public bool Stunned = false;
    public bool isDead = false;

    //private int BackID;

    //Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();

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

        if (Stunned == true && Wait < MaxStun) { Wait += Time.deltaTime * 1f; }
        else { Stunned = false; }

        if (player.GetAxis("HeadThrow&Return") > 0 && Stunned == false)
        {
            //animator.Play(BackID);

            playerAll = OriginalBody.GetComponent<PlayerController2D>();
            playerAll.ReturnHead();

            Destroy(gameObject); //AUTODESTRUCCION
        }
    }
}
