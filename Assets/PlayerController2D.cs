using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Controller { NONE, PLAYER1, PLAYER2, PLAYER3, PLAYER4 };
public enum Skin { NONE, SKIN1, SKIN2 };

public class PlayerController2D : MonoBehaviour{

    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    public GameObject Head;
    public GameObject Body;
    public GameObject HeadThrow;
    private HeadThrow headThrow;
    private HeadReturn headReturn;
    private HeadReceive bodyReceive;

    public float runSpeed = 2f; 
    public float jumpStrengh = 6.5f;

    bool facingright = true;
    float headCharge = 0f;
    float weaponCharge = 0f;

    //KEYS
    KeyCode leftButton = KeyCode.None;
    KeyCode rightButton = KeyCode.None;
    KeyCode jumpButton = KeyCode.None;
    KeyCode pickupButton = KeyCode.None;
    KeyCode attackButton = KeyCode.None;
    KeyCode chargeButton = KeyCode.None;
    KeyCode headButton = KeyCode.None;
    private int MoveID;
    private int JumpID;
    private int AttackID;
    private int ChargeID;
    private int HurtID;
    private int HeadID;

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
                leftButton = KeyCode.A;
                rightButton = KeyCode.D;
                jumpButton = KeyCode.Space;
                pickupButton = KeyCode.E;
                attackButton = KeyCode.R;
                chargeButton = KeyCode.F;
                headButton = KeyCode.Z;
                break;
            case Controller.PLAYER2:
                leftButton = KeyCode.LeftArrow;
                rightButton = KeyCode.RightArrow;
                jumpButton = KeyCode.Space;
                pickupButton = KeyCode.E;
                attackButton = KeyCode.R;
                chargeButton = KeyCode.F;
                headButton = KeyCode.Z;
                break;
            case Controller.PLAYER3:
                leftButton = KeyCode.A;
                rightButton = KeyCode.D;
                jumpButton = KeyCode.Space;
                pickupButton = KeyCode.E;
                attackButton = KeyCode.R;
                chargeButton = KeyCode.F;
                headButton = KeyCode.Z;
                break;
            case Controller.PLAYER4:
                leftButton = KeyCode.A;
                rightButton = KeyCode.D;
                jumpButton = KeyCode.Space;
                pickupButton = KeyCode.E;
                attackButton = KeyCode.R;
                chargeButton = KeyCode.F;
                headButton = KeyCode.Z;
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

        //GetKey: repite cada segundo q se presiona | GetKeyDown: solo one vez al presionar | GetKeyUp: solo one vez al soltar

        //IDLE IS AUTOMATIC
        body2D.velocity = new Vector2(0, body2D.velocity.y);

        //MOVEMENT
        if (Input.GetKey(rightButton) && isCharging == false && isHeading == false)
        {
            body2D.velocity = new Vector2(runSpeed, body2D.velocity.y);
            spriteRenderer.flipX = true;
            animator.SetBool(MovingID, true);
            facingright = true;
        }
        else if(Input.GetKey(leftButton) && isCharging == false && isHeading == false)
        {
            body2D.velocity = new Vector2(-runSpeed, body2D.velocity.y);
            spriteRenderer.flipX = false;
            animator.SetBool(MovingID, true);
            facingright = false;
        }
        else
        {
            animator.SetBool(MovingID, false);
        }

        //JUMP
        if (Input.GetKey(jumpButton) && isGrounded == true && isCharging == false)
        {
            animator.SetTrigger(JumpedID);
            body2D.velocity = new Vector2(body2D.velocity.x, jumpStrengh);
        }

        //ATTACK
        if (Input.GetKeyDown(attackButton))
        {
            animator.SetBool(AttackedID, true);
        }

        //CHARGED
        if (Input.GetKey(chargeButton) && isHeading == false)
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
        if (Input.GetKey(headButton) && isCharging == false)
        {
            {
                animator.SetBool(HeadingID, true);
                if (headCharge <= 3f)
                {
                    headCharge += Time.deltaTime * 1f;
                    Debug.Log(headCharge);
                }
                else if (headCharge >= 3f)
                {
                    Debug.Log("MaxCharge");
                }
            }
        }
        else if (headCharge > 0)
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
             headRigid.velocity = new Vector2(headCharge * 2, 0);
            }
            else if (facingright == false)
            {
            headRigid.velocity = new Vector2(-headCharge * 2, 0);
            }


            headCharge = 0;

            bodyReceive = body.GetComponent<HeadReceive>();
            bodyReceive.controller = this.controller;
            bodyReceive.skin = this.skin;
            
            Destroy(gameObject); //AUTODESTRUCCION
        }

        //50% MOVEMENT
        if (Input.GetKey(rightButton) && isCharging == true || Input.GetKey(rightButton) && isHeading == true)
        {
            body2D.velocity = new Vector2(runSpeed * 50 / 100, body2D.velocity.y); //(50% de max speed, velocidad actual y)
            spriteRenderer.flipX = true;
            facingright = true;
        }
        else if (Input.GetKey(leftButton) && isCharging == true || Input.GetKey(leftButton) && isHeading == true)
        {
            body2D.velocity = new Vector2(-runSpeed * 50 / 100, body2D.velocity.y);
            spriteRenderer.flipX = false;
            facingright = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision){ //ON STAY SOLO CON EL SUELO, Q ES MUY PESADO EN CPU

        Vector3 hit = collision.contacts[0].normal;
        float angle = Vector3.Angle(hit, Vector3.up);

        if (collision.gameObject.tag == "Floor")
        {
            if (Mathf.Approximately(angle, 0))
            {
                animator.SetBool(GroundingID, true);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        bool isWeaponed = animator.GetBool(WeaponingID);

        if (collision.gameObject.tag == "PickUp")
        {
            Debug.Log("Collided w/ pickup");
        }

        if (collision.gameObject.tag == "PickUp" && Input.GetKeyDown(pickupButton) && isWeaponed == false)
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

        if (collision.gameObject.tag == "Damage")
        {
            animator.SetTrigger(DamagedID);
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