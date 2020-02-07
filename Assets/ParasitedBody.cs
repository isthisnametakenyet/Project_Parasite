using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public class ParasitedBody : MonoBehaviour
{

    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    private Player player;

    public int originalController;

    public GameObject HeadFall;
    private HeadReturn headReturn;

    //CAMBIABLE
    public float runSpeed = 2f;
    public float jumpStrengh = 6.5f;
    bool facingright = true;

    //CONDITIONS
    private int GroundingID;
    private int MovingID;
    private int JumpedID;
    private int WeaponingID;
    private int whichWeaponID;
    private int AttackedID;
    private int ChargingID;
    private int HeadingID;
    private int DamagedID;

    Animator animator;
    Rigidbody2D body2D;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //SKIN
        switch (skin)
        {
            case Skin.NONE:
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/SkinPlaceholder");
                break;
            case Skin.SKIN1:
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Skin1");
                break;
            case Skin.SKIN2:
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Skin2");
                break;
        }

        //KEYS
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

        //CONDITIONS
        GroundingID = Animator.StringToHash("Grounding");
        MovingID = Animator.StringToHash("Moving");
        JumpedID = Animator.StringToHash("Jumped");
        WeaponingID = Animator.StringToHash("Weaponing");
        whichWeaponID = Animator.StringToHash("whichWeapon");
        AttackedID = Animator.StringToHash("Attacked");
        ChargingID = Animator.StringToHash("Charging");
        HeadingID = Animator.StringToHash("Heading");
        DamagedID = Animator.StringToHash("Damaged");
    }

    void FixedUpdate()
    {
        bool isGrounded = animator.GetBool(GroundingID);
        bool isMoving = animator.GetBool(MovingID);
        bool isWeaponed = animator.GetBool(WeaponingID);
        int whichWeapon = animator.GetInteger(whichWeaponID);
        bool isCharging = animator.GetBool(ChargingID);
        bool isHeading = animator.GetBool(HeadingID);
        bool isDucking = false;

        //IDLE IS AUTOMATIC
        body2D.velocity = new Vector2(0, body2D.velocity.y);

        //MOVEMENT
        if (player.GetAxis("Move Joystick") > 0 || player.GetButton("Move Right Keys"))
        {
            if (isCharging == false && isHeading == false && isDucking == false)
            {
                body2D.velocity = new Vector2(runSpeed, body2D.velocity.y);
                spriteRenderer.flipX = true;
                animator.SetBool(MovingID, true);
                facingright = true;
            }
        }
        else if (player.GetAxis("Move Joystick") < 0 || player.GetButton("Move Left Keys"))
        {
            if (isCharging == false && isHeading == false && isDucking == false)
            {
                body2D.velocity = new Vector2(-runSpeed, body2D.velocity.y);
                spriteRenderer.flipX = false;
                animator.SetBool(MovingID, true);
                facingright = false;
            }
        }
        else
        {
            animator.SetBool(MovingID, false);
        }

        //JUMP
        if (player.GetButtonDown("Jump") && isGrounded == true && isCharging == false && isDucking == false)
        {
            animator.SetTrigger(JumpedID);
            body2D.velocity = new Vector2(body2D.velocity.x, jumpStrengh);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool(GroundingID, true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool(GroundingID, false);
        }
    }
}
