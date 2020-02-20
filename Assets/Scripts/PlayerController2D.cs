using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public enum Controller { NONE, PLAYER0, PLAYER1, PLAYER2, PLAYER3 };
public enum Skin { NONE, SKIN1, SKIN2 };

public class PlayerController2D : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    private Player player;

    public GameObject HeadFall;
    public GameObject BodyEmpty;
    public GameObject HeadThrow;
    private HeadThrow headThrow;
    private HeadReturn headReturn;
    private EmptyBody emptyBody;

    //WEAPONS PICKUP
    public GameObject PickedWeapon;
    private PickUpScript pickUpScript;
    private Sword swordScript;
    private Axe axeScript;
    private Spear spearScript;
    private Bow bowScript;
    private CrossBow crossbowScript;
    private Boomerang boomerangScript;

    //VARIABLES
    public float runSpeed = 2f; 
    public float jumpStrengh = 6.5f;
    public float headReturnDelay = 2f;
    public float maxWeaponCharge = 1.5f;
    public float throwWeaponSpeed = 8f;
    public float headThrowCharge = 2f;
    public float forgetWeaponChargeRange = 0.3f;
    public float forgetHeadThrowRange = 0.4f;

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
    BoxCollider2D box2D;

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
                Debug.Log("PlayerB Skin: PlaceHolder");
                break;
            case Skin.SKIN1:
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Skin1");
                Debug.Log("Skin: 1");
                break;
            case Skin.SKIN2:
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Skin2");
                Debug.Log("Skin: 2");
                break;
        }

        //KEYS
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
        if (player.GetButton("Duck") && isGrounded == true && isCharging == false)
        {
            isDucking = true;
            Debug.Log("Quack");
        }
        else
        {
            isDucking = false;
        }

        //ATTACK
        if (player.GetButtonDown("Attack"))
        {
            //animator.SetBool(AttackedID, true);
            if (isWeaponed == true)
            {
                switch (whichWeapon)
                {
                    case 1:
                        swordScript = PickedWeapon.GetComponent<Sword>();
                        swordScript.Attack = true;
                        Debug.Log("PWeapon: 1");
                        break;
                    case 2:
                        axeScript = PickedWeapon.GetComponent<Axe>();
                        axeScript.Attack = true;
                        Debug.Log("PWeapon: 2");
                        break;
                    case 3:
                        spearScript = PickedWeapon.GetComponent<Spear>();
                        spearScript.Attack = true;
                        Debug.Log("PWeapon: 3");
                        break;
                    case 4:
                        bowScript = PickedWeapon.GetComponent<Bow>();
                        bowScript.Attack = true;
                        Debug.Log("PWeapon: 4");
                        break;
                    case 5:
                        crossbowScript = PickedWeapon.GetComponent<CrossBow>();
                        crossbowScript.Attack = true;
                        Debug.Log("PWeapon: 5");
                        break;
                    case 6:
                        boomerangScript = PickedWeapon.GetComponent<Boomerang>();
                        boomerangScript.Attack = true;
                        Debug.Log("PWeapon: 6");
                        break;
                }
            }
        }

        //CHARGED
        if (player.GetButton("Charge") && isHeading == false && isDucking == false && isWeaponed == true)
        {
            animator.SetBool(ChargingID, true);
            if (weaponCharge < maxWeaponCharge)
            {
                weaponCharge += Time.deltaTime; 
                Debug.Log(weaponCharge);
                if(weaponCharge > forgetWeaponChargeRange)
                {
                    switch (whichWeapon)
                    {
                        case 1:
                            swordScript = PickedWeapon.GetComponent<Sword>();
                            swordScript.Charging = true;
                            break;
                        case 2:
                            axeScript = PickedWeapon.GetComponent<Axe>();
                            axeScript.Charging = true;
                            break;
                        case 3:
                            spearScript = PickedWeapon.GetComponent<Spear>();
                            spearScript.Charging = true;
                            break;
                        case 4:
                            bowScript = PickedWeapon.GetComponent<Bow>();
                            bowScript.Charging = true;
                            break;
                        case 5:
                            crossbowScript = PickedWeapon.GetComponent<CrossBow>();
                            crossbowScript.Charging = true;
                            break;
                        case 6:
                            boomerangScript = PickedWeapon.GetComponent<Boomerang>();
                            boomerangScript.Charging = true;
                            break;
                    }
                }
            }
            else if (weaponCharge >= maxWeaponCharge)
            {
                Debug.Log("MaxCharge");
            }
        }
        else if (weaponCharge < forgetWeaponChargeRange) { weaponCharge = 0; animator.SetBool(ChargingID, false); }
        else if (weaponCharge > forgetWeaponChargeRange)
        {
            Debug.Log("PWeapon: THROW");
            switch (whichWeapon)
            {
                case 1:
                    swordScript.Thrown = true;
                    break;
                case 2:
                    axeScript.Thrown = true;
                    break;
                case 3:
                    spearScript.Thrown = true;
                    break;
                case 4:
                    bowScript.Thrown = true;
                    break;
                case 5:
                    crossbowScript.Thrown = true;
                    break;
                case 6:
                    boomerangScript.Thrown = true;
                    break;
            }
            animator.SetBool(ChargingID, false); //END ANIAMTION, BACK TO IDLE

            PickedWeapon.transform.parent = null;
            Rigidbody2D weaponRigid;
            weaponRigid = PickedWeapon.GetComponent<Rigidbody2D>(); //ASIGN HEAD RIGIDBODY

            if (facingright == true) //THROW WEAPON with headCharge as force
            {
                weaponRigid.velocity = new Vector2(throwWeaponSpeed, 0);
            }
            else if (facingright == false)
            {
                weaponRigid.velocity = new Vector2(-throwWeaponSpeed, 0);
            }
            weaponCharge = 0;
            animator.SetBool(WeaponingID, false);
        }

        //HEAD THROW
        if (player.GetButton("Head Throw") && isCharging == false && isDucking == false)
        { 
            {
                animator.SetBool(HeadingID, true);
                if (headCharge <= headThrowCharge)
                {
                    headCharge += Time.deltaTime;
                    Debug.Log(headCharge);
                }
                else if (headCharge >= headThrowCharge)
                {
                    Debug.Log("MaxCharge");
                }
            }
        }
        else if (headCharge < forgetHeadThrowRange) { headCharge = 0; animator.SetBool(HeadingID, false); }
        else if (headCharge > forgetHeadThrowRange) //THROW
        {
            animator.SetBool(HeadingID, false);

            GameObject head = Instantiate(HeadThrow, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            GameObject body = Instantiate(BodyEmpty, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);

            emptyBody = body.GetComponent<EmptyBody>();
            emptyBody.controller = this.controller;
            emptyBody.skin = this.skin;

            headThrow = head.GetComponent<HeadThrow>();
            headThrow.controller = this.controller;
            headThrow.skin = this.skin;
            headThrow.OriginalBody = body; //REFERENCE EMPTYBODY IN HEAD THROW TO KNOW ORIGIN

            Rigidbody2D headRigid;
            headRigid = head.GetComponent<Rigidbody2D>(); //GET HEAD RIGIDBODY

            if (facingright == true) //THROW HEAD with headCharge as force
            {
             headRigid.velocity = new Vector2(headCharge * 1.8f, 2f);
            }
            else if (facingright == false)
            {
            headRigid.velocity = new Vector2(-headCharge * 1.8f, 2f);
            }

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

    private void OnTriggerStay2D(Collider2D collision) //PICKUP
    {
        bool isWeaponed = animator.GetBool(WeaponingID); 

        if (collision.gameObject.tag == "PickUp" && player.GetButtonDown("PickUp") && isWeaponed == false)
        {
            pickUpScript = collision.GetComponent<PickUpScript>();
            pickUpScript.Picker = this.gameObject;
            pickUpScript.picked = true;
            animator.SetBool(WeaponingID, true);

            switch (pickUpScript.picktype)
            {
                case PickTypes.Sword:
                    animator.SetInteger(whichWeaponID, 1);
                    break;
                case PickTypes.Axe:
                    animator.SetInteger(whichWeaponID, 2);
                    break;
                case PickTypes.Spear:
                    animator.SetInteger(whichWeaponID, 3);
                    break;
                case PickTypes.Bow:
                    animator.SetInteger(whichWeaponID, 4);
                    break;
                case PickTypes.CrossBow:
                    animator.SetInteger(whichWeaponID, 5);
                    break;
                case PickTypes.Boomerang:
                    animator.SetInteger(whichWeaponID, 6);
                    break;
            }
        }
        else if (collision.gameObject.tag == "Stuck" && player.GetButtonDown("PickUp") && isWeaponed == false)  //RE-PICKUP
        {
            switch (collision.gameObject.name)
            {
                case "place_sword(Clone)":
                    swordScript = collision.GetComponent<Sword>();
                    swordScript.Picker = this.gameObject;
                    animator.SetInteger(whichWeaponID, 1);
                    break;
                case "place_axe(Clone)":
                    axeScript = collision.GetComponent<Axe>();
                    axeScript.Picker = this.gameObject;
                    animator.SetInteger(whichWeaponID, 2);
                    break;
                case "place_spear(Clone)":
                    spearScript = collision.GetComponent<Spear>();
                    spearScript.Picker = this.gameObject;
                    animator.SetInteger(whichWeaponID, 3);
                    break;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //DAMAGE
    {
        if (collision.gameObject.tag == "Throwing" && collision.gameObject != PickedWeapon)
        {
            //animator.SetTrigger(DamagedID);
            GameObject head = Instantiate(HeadFall, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            GameObject body = Instantiate(BodyEmpty, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);

            headReturn = head.GetComponent<HeadReturn>();
            headReturn.controller = this.controller;
            headReturn.skin = this.skin;
            headReturn.OriginalBody = body;
            headReturn.Stunned = true;

            Rigidbody2D headRigid;
            headRigid = head.GetComponent<Rigidbody2D>(); //ASIGN ITS RIGID
            headCharge = 2f;
            
            if (transform.position.x > collision.transform.position.x) //RIGHT
            {
                headRigid.velocity = new Vector2(headCharge * 1.8f, 2f);
            }
            else if (transform.position.x < collision.transform.position.x) //LEFT
            {
                headRigid.velocity = new Vector2(-headCharge * 1.8f, 2f);
            }
            else { Debug.LogError("Error Detectando Direccion de Collision"); }

            emptyBody = body.GetComponent<EmptyBody>();
            emptyBody.controller = this.controller;
            emptyBody.skin = this.skin;

            Destroy(gameObject); //AUTODESTRUCCION
        }

        if (collision.gameObject.tag == "Attacking" && collision.gameObject != PickedWeapon)
        {
            //LOSE ARM
        }

        if (collision.gameObject.tag == "Damage") //DEATH
        {
            Debug.Log("Touch Sierra");
            GameObject head = Instantiate(HeadFall, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);

            Rigidbody2D headRigid;
            headRigid = head.GetComponent<Rigidbody2D>(); //ASIGN ITS RIGID
            headCharge = 2f;

            if (transform.position.x > collision.transform.position.x) //RIGHT
            {
                headRigid.velocity = new Vector2(headCharge * 1.8f, 2f);
            }
            else if (transform.position.x < collision.transform.position.x) //LEFT
            {
                headRigid.velocity = new Vector2(-headCharge * 1.8f, 2f);
            }
            else { Debug.LogError("Error Detectando Direccion de Collision"); }

            Destroy(gameObject); //AUTODESTRUCCION
        }
    }
}