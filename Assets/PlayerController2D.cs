using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public enum Controller { NONE, PLAYER1, PLAYER2, PLAYER3, PLAYER4 };
public enum Skin { NONE, SKIN1, SKIN2 };

public class PlayerController2D : MonoBehaviour{

    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    private Player player;

    public GameObject Head;
    public GameObject Body;
    public GameObject HeadThrow;
    private HeadThrow headThrow;
    private HeadReturn headReturn;
    private HeadReceive bodyReceive;

    //CAMBIABLE
    public float runSpeed = 2f; 
    public float jumpStrengh = 6.5f;

    //TEMPORALES
    bool facingright = true;
    float headCharge = 0f;
    float weaponCharge = 0f;

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

    void Start(){

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
        //https://docs.unity3d.com/ScriptReference/KeyCode.html
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

    private void FixedUpdate(){
        //bool isCondition = animator.GetBool(ConditionID); //animator.SetBool(ConditionID, true);
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

        //DUCK
        if (player.GetButtonDown("Duck") && isGrounded == true && isCharging == false)
        {
            isDucking = true;
        }
        else
        {
            isDucking = false;
        }

        //ATTACK
        if (player.GetButtonDown("Attack"))
        {
            animator.SetBool(AttackedID, true);
        }

        //CHARGED
        if (player.GetButton("Charge") && isHeading == false)
        {
            animator.SetBool(ChargingID, true);
            if (weaponCharge < 3f){
                weaponCharge += Time.deltaTime*1f; 
                Debug.Log(weaponCharge);
            }
            else if (weaponCharge >= 3f)
            {
                Debug.Log("MaxCharge");
            }
        }
        else if (weaponCharge > 0)
        {
            {
            animator.SetBool(ChargingID, false);
            weaponCharge = 0;
                //THROW
            }
        }

        //HEAD THROW
        if (player.GetButton("Head Throw") && isCharging == false)
        {
            {
                animator.SetBool(HeadingID, true);
                if (headCharge <= 2f)
                {
                    headCharge += Time.deltaTime * 1f;
                    Debug.Log(headCharge);
                }
                else if (headCharge >= 2f)
                {
                    Debug.Log("MaxCharge");
                }
            }
        }
        else if (headCharge > 0) //THROW
        {
            animator.SetBool(HeadingID, false);

            GameObject head = Instantiate(HeadThrow, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            GameObject body = Instantiate(Body, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);

            headThrow = head.GetComponent<HeadThrow>();
            headThrow.controller = this.controller;
            headThrow.skin = this.skin;
            Rigidbody2D headRigid;
            headRigid = head.GetComponent<Rigidbody2D>(); //ASIGN ITS RIGID

            if (facingright == true) //THROW HEAD with headCharge as force
            {
             headRigid.velocity = new Vector2(headCharge * 1.8f, 2f);
            }
            else if (facingright == false)
            {
            headRigid.velocity = new Vector2(-headCharge * 1.8f, 2f);
            }


            headCharge = 0;

            bodyReceive = body.GetComponent<HeadReceive>();
            bodyReceive.controller = this.controller;
            bodyReceive.skin = this.skin;
            
            Destroy(gameObject); //AUTODESTRUCCION
        }

        //50% MOVEMENT
        if (player.GetAxis("Move Joystick") > 0 || player.GetButton("Move Right Keys"))
        {
            if (isCharging == true || isHeading == true)
            {
                body2D.velocity = new Vector2(runSpeed * 50 / 100, body2D.velocity.y); //(50% de max speed, velocidad actual y)
                spriteRenderer.flipX = true;
                facingright = true;
            }
        }
        else if (player.GetAxis("Move Joystick") < 0 || player.GetButton("Move Left Keys"))
        {
            if (isCharging == true || isHeading == true)
            {
                body2D.velocity = new Vector2(-runSpeed * 50 / 100, body2D.velocity.y);
                spriteRenderer.flipX = false;
                facingright = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision){ //ES MUY PESADO EN CPU

        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool(GroundingID, true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        bool isWeaponed = animator.GetBool(WeaponingID);

        if (collision.gameObject.tag == "PickUp" && player.GetButtonDown("PickUp") && isWeaponed == false)
        {
            
            animator.SetBool(WeaponingID, true);

            if (collision.gameObject.name == "Sword")
            {
                animator.SetInteger(whichWeaponID, 1);
            }
            else if (collision.gameObject.name == "Axe")
            {
                animator.SetInteger(whichWeaponID, 2);
            }
            else if (collision.gameObject.name == "Lance")
            {
                animator.SetInteger(whichWeaponID, 3);
            }
            else if (collision.gameObject.name == "Bow")
            {
                animator.SetInteger(whichWeaponID, 4);
            }
            else if (collision.gameObject.name == "CrossBow")
            {
                animator.SetInteger(whichWeaponID, 5);
            }
            else if (collision.gameObject.name == "Boomerang")
            {
                animator.SetInteger(whichWeaponID, 6);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        Vector3 hit = collision.contacts[0].normal;
        float angle = Vector3.Angle(hit, Vector3.up);

        if (collision.gameObject.tag == "Damage")
        {
            animator.SetTrigger(DamagedID);

            if (Mathf.Approximately(angle, 90))
            {
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                if (cross.y == 1f) //RIGHT
                {
                    Debug.Log("Right"); 
                }
                else if (cross.y > 90) //LEFT
                {
                    Debug.Log("Left"); 
                }
                else
                {
                    Debug.Log("WTF GENTE");
                }
            }

            HeadOFF();
        }
    }

    private void OnCollisionExit2D(Collision2D collision){

        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool(GroundingID, false);
        }
    }

    private void HeadOFF(){
        GameObject head = Instantiate(Head, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
        GameObject body = Instantiate(Body, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);

        headReturn = head.GetComponent <HeadReturn>();
        headReturn.controller = this.controller;
        headReturn.skin = this.skin;
        headReturn.Body = body;


        bodyReceive = body.GetComponent <HeadReceive>();
        bodyReceive.controller = this.controller;
        bodyReceive.skin = this.skin;

        Destroy(gameObject); //AUTODESTRUCCION
    }
}