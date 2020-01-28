using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Controller { NONE, PLAYER1, PLAYER2, PLAYER3, PLAYER4 };
public enum Skin { NONE, SKIN1, SKIN2 };
public enum Weaponed { NONE, SWORD, AXE, LANCE, BOW, CROSSBOW, BOOMERANG };

public class PlayerController2D : MonoBehaviour{

    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;
    public Weaponed weaponed = global::Weaponed.NONE;

    public GameObject Head;
    public GameObject Body;
    public GameObject HeadThrow;
    private HeadThrow headThrow;
    private HeadReturn headReturn;
    private HeadReceive bodyReceive;

    public float runSpeed = 2f; 
    public float jumpStrengh = 6.5f; 

    float charge = 0f;

    //KEYS
    KeyCode leftButton = KeyCode.None;
    KeyCode rightButton = KeyCode.None;
    KeyCode jumpButton = KeyCode.None;
    KeyCode pickupButton = KeyCode.None;
    KeyCode attackButton = KeyCode.None;
    KeyCode chargeButton = KeyCode.None;
    KeyCode headButton = KeyCode.None;
    private int IdleID;
    private int MoveID;
    private int JumpID;
    private int AttackID;
    private int ChargeID;
    private int HurtID;
    private int HeadID;

    //CONDITIONS
    private int GroundID;
    private int MovingID;
    private int WeaponedID;
    private int whichWeaponID;
    private int AttackingID;
    private int ChargingID;
    private int DamagedID;

    Animator animator;
    Rigidbody2D body2D;
    SpriteRenderer spriteRenderer; //Debug.Log("" + Time.time);

    void Start(){

        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //SKIN
        //if (skin == Skin.SKIN1)
        //{
        //    animator.runtimeAnimatorController = Resources.Load("Assets/Animations/Skin1.controller") as RuntimeAnimatorController; 
        //}
        //else if (skin == Skin.SKIN2)
        //{
        //    animator.runtimeAnimatorController = Resources.Load("Assets/Animations/Skin2.controller") as RuntimeAnimatorController;
        //}

        //KEYS

        //https://docs.unity3d.com/ScriptReference/KeyCode.html
        if (controller == Controller.PLAYER1)
        {
            leftButton = KeyCode.A;
            rightButton = KeyCode.D;
            jumpButton = KeyCode.Space;
            pickupButton = KeyCode.E;
            attackButton = KeyCode.R;
            chargeButton = KeyCode.F;
            headButton = KeyCode.Z;
        }
        else if (controller == Controller.PLAYER2)
        {
            leftButton = KeyCode.LeftArrow;
            rightButton = KeyCode.RightArrow;
            jumpButton = KeyCode.UpArrow;
            attackButton = KeyCode.None;
            chargeButton = KeyCode.None;
            
        }
        else if (controller == Controller.PLAYER3)
        {
            leftButton = KeyCode.None;
            rightButton = KeyCode.None;
            jumpButton = KeyCode.None;
            attackButton = KeyCode.None;
            chargeButton = KeyCode.None;
        }
        else if (controller == Controller.PLAYER4)
        {
            leftButton = KeyCode.None;
            rightButton = KeyCode.None;
            jumpButton = KeyCode.None;
            attackButton = KeyCode.None;
            chargeButton = KeyCode.None;
        }

        //CONDITIONS
        GroundID = Animator.StringToHash("Grounded");
        MovingID = Animator.StringToHash("Moving");
        WeaponedID = Animator.StringToHash("Weaponed");
        whichWeaponID = Animator.StringToHash("whichWeapon");
        AttackingID = Animator.StringToHash("Attacking");
        ChargingID = Animator.StringToHash("Charging");
        DamagedID = Animator.StringToHash("Damaged");
    }

    private void FixedUpdate(){
        //bool isCondition = animator.GetBool(ConditionID); //animator.SetBool(ConditionID, true);
        bool isGrounded = animator.GetBool(GroundID);
        bool isMoving = animator.GetBool(MovingID);
        bool isWeaponed = animator.GetBool(WeaponedID);
        int whichWeapon = animator.GetInteger(whichWeaponID);
        bool isCharging = animator.GetBool(ChargingID);

        //GetKey: repite cada segundo q se presiona | GetKeyDown: solo one vez al presionar | GetKeyUp: solo one vez al soltar

        //IDLE IS AUTOMATIC
        body2D.velocity = new Vector2(0, body2D.velocity.y);

        //MOVEMENT
        if (Input.GetKey(rightButton) && isCharging == false)
        {
            body2D.velocity = new Vector2(runSpeed, body2D.velocity.y);
            spriteRenderer.flipX = true;
            animator.SetBool(MovingID, true);
        }
        else if(Input.GetKey(leftButton) && isCharging == false)
        {
            body2D.velocity = new Vector2(-runSpeed, body2D.velocity.y);
            spriteRenderer.flipX = false;
            animator.SetBool(MovingID, true);
        }
        else
        {
            animator.SetBool(MovingID, false);
        }

        //JUMP
        if (Input.GetKey(jumpButton) && isGrounded == true && isCharging == false)
        {
            body2D.velocity = new Vector2(body2D.velocity.x, jumpStrengh);
        }

        //PICKUP
        if (Input.GetKey(pickupButton))
        {
            
        }

        //ATTACK
        if (Input.GetKeyDown(attackButton))
        {
            animator.SetBool(AttackingID, true);
        }

        //CHARGED
        if (Input.GetKey(chargeButton))
        {
            animator.SetBool(ChargingID, true);
            if (charge < 3f){
                charge += Time.deltaTime*1f;
                Debug.Log(charge);
            }
            else if (charge == 3f)
            {
                Debug.Log("MaxCharge");
            }
        }
        else if (charge > 0)
        {
            {
            animator.SetBool(ChargingID, false);
            charge = 0;
            }
        }

        if(Input.GetKey(rightButton) && isCharging == true)
        {
            body2D.velocity = new Vector2(runSpeed * 50 / 100, body2D.velocity.y); //(50% de max speed, velocidad actual y)
            spriteRenderer.flipX = true;
        }
        else if(Input.GetKey(leftButton) && isCharging == true)
        {
            body2D.velocity = new Vector2(-runSpeed * 50 / 100, body2D.velocity.y);
            spriteRenderer.flipX = false;
        }

        //HEAD THROW
        if (Input.GetKey(headButton) && isCharging == false)
        {
            GameObject head = Instantiate(HeadThrow, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            GameObject body = Instantiate(Body, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);

            headThrow = head.GetComponent<HeadThrow>();
            headThrow.controller = this.controller;
            headThrow.skin = this.skin;

            bodyReceive = body.GetComponent<HeadReceive>();
            bodyReceive.controller = this.controller;
            bodyReceive.skin = this.skin;

            Destroy(gameObject); //AUTODESTRUCCION
        }
    }

    private void OnCollisionStay2D(Collision2D collision){ //ON STAY SOLO CON EL SUELO, Q ES MUY PESADO EN CPU

        Vector3 hit = collision.contacts[0].normal;
        float angle = Vector3.Angle(hit, Vector3.up);

        if (collision.gameObject.tag == "Floor")
        {
            if (Mathf.Approximately(angle, 0))
            {
                animator.SetBool(GroundID, true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
   
        if (collision.gameObject.tag == "Pickup")
        {
            animator.SetBool(WeaponedID, true);
            //animator.SetBool(whichWeapon, 0);
        }

        if (collision.gameObject.tag == "Damage")
        {
            animator.SetBool(DamagedID, true);
            HeadOFF();
        }
    }

    private void OnCollisionExit2D(Collision2D collision){

        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool(GroundID, false);
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