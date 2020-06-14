using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public enum HeadFallEffect { NONE, ConstantBlinkTime, ColorIncrease, ProgresiveBlinkTime };

public class HeadReturn : MonoBehaviour
{
    [HideInInspector] public Controller controller = Controller.NONE;

    private Player player;

    #region InstanceVariables
    //GAME OBJECTS
    [HideInInspector] public GameObject OriginalBody;
    private PlayerController2D playerAll;

    [Header("Blink")]
    public Color redBlink;
    public Color purpleBlink;
    public Color yellowBlink;
    public Color greenBlink;

    [Space(6)]
    public HeadFallEffect headFallEffect;

   //VARAIBLES
    [Header("Variables")]
    public float MaxStun = 2;

    [HideInInspector] public bool Stunned = false;
    [HideInInspector] public bool isDead = false;

    //TEMPORALES
    float Wait = 0;
    float blinkDelay = 0;
    float blinkTmp = 0;
    bool Blinked = false;
    bool setted = false;
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

        if (Stunned == true && Wait < MaxStun)
        {

            Wait += Time.deltaTime * 1f;

            switch (headFallEffect)
            {
                case HeadFallEffect.NONE:
                    break;
                case HeadFallEffect.ConstantBlinkTime: ///slow constant blinks (regardles of stun time)
                    if (renderer.material.GetFloat("_FlashAmount") < 0.6f) 
                    {
                        renderer.material.SetFloat("_FlashAmount", renderer.material.GetFloat("_FlashAmount") + 0.05f);
                    }
                    else
                    {
                        renderer.material.SetFloat("_FlashAmount", 0f);
                    }
                    break;
                case HeadFallEffect.ColorIncrease: ///progresively increases color density depending on remaining stun time
                    renderer.material.SetFloat("_FlashAmount", Wait / MaxStun); 
                    break;
                case HeadFallEffect.ProgresiveBlinkTime: ///progresively decreses delay for next blink depending on remaining stun time
                    if (Blinked == false)
                    {
                        if (setted == false)
                        {
                            blinkDelay = 1 - (Wait / MaxStun);
                            blinkTmp = 0;
                            renderer.material.SetFloat("_FlashAmount", 0.7f);
                            setted = true;
                        }
                        else
                        {
                            if (blinkTmp < 1 - (Wait / MaxStun)) {
                                blinkTmp += Time.deltaTime;
                                renderer.material.SetFloat("_FlashAmount", renderer.material.GetFloat("_FlashAmount") - 0.05f);
                            }
                            else { blinkTmp = 0; Blinked = true; renderer.material.SetFloat("_FlashAmount", 0f); }
                        }
                    }
                    else if (blinkTmp < blinkDelay) { blinkTmp += Time.deltaTime; }
                    else { Blinked = false; setted = false; }
                    break;
                default:
                    break;
            }
        }

        else { Stunned = false; renderer.material.SetFloat("_FlashAmount", 0.6f); }

        if (player.GetAxis("HeadThrow&Return") > 0 && Stunned == false)
        {
            //animator.Play(BackID);

            playerAll = OriginalBody.GetComponent<PlayerController2D>();
            playerAll.ReturnHead();

            Destroy(gameObject); //AUTODESTRUCCION
        }
    }
}
