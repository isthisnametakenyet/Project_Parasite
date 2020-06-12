using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public class HeadReturn : MonoBehaviour
{
    [Header("Origin")]
    public Controller controller = Controller.NONE;

    private Player player;

    #region InstanceVariables
    //GAME OBJECTS
    public GameObject OriginalBody;
    private PlayerController2D playerAll;

    [Header("Blink Colors")]
    public Color redBlink;
    public Color purpleBlink;
    public Color yellowBlink;
    public Color greenBlink;

    //VARAIBLES
    [Header("Variables")]
    public float MaxStun = 2;
    float Wait = 0;
    [HideInInspector] public bool Stunned = false;
    [HideInInspector] public bool isDead = false;
    #endregion

    //CONDITIONS
    //private int BackID;

    //COMPONENTS
    //Animator animator;
    SpriteRenderer renderer;

    void Start()
    {
        //animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        switch (controller)
        {
            case Controller.PLAYER0:
                player = ReInput.players.GetPlayer(0);
                renderer.material.SetColor("_FlashColor", redBlink); ///sets the color of the shader to the pre-set color in the inspector
                break;

            case Controller.PLAYER1:
                player = ReInput.players.GetPlayer(1);
                renderer.material.SetColor("_FlashColor", purpleBlink);
                break;

            case Controller.PLAYER2:
                player = ReInput.players.GetPlayer(2);
                renderer.material.SetColor("_FlashColor", yellowBlink);
                break;

            case Controller.PLAYER3:
                player = ReInput.players.GetPlayer(3);
                renderer.material.SetColor("_FlashColor", greenBlink);
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
