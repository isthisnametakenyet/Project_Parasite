using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public enum Controller { NONE, PLAYER0, PLAYER1, PLAYER2, PLAYER3 };
public enum Skin { NONE, SKIN1, SKIN2 };
public enum Arms { NONE, ONE, TWO };

public class PlayerController2D : MonoBehaviour
{
    //private Rewired.Player player { get { return PlayerAssignment.GetRewiredPlayer(((int)controller)-1); } }
    private Player player;

    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;
    public Arms arms = Arms.NONE;

    public bool playerReady = false;

    public GameObject HeadFall;
    public GameObject BodyEmpty;
    public GameObject HeadThrow;
    public GameObject RightArm;
    public GameObject LeftArm;
    private HeadThrow headThrow;
    private HeadReturn headReturn;
    private EmptyBody emptyBody;
    private Arm rightScript;
    private Arm leftScript;
    private Transform ArmParent;

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
    public float throwWeaponSpeed = 10f;
    public float headThrowCharge = 2f;
    public float forgetWeaponChargeRange = 0.3f;
    public float forgetHeadThrowRange = 0.4f;
    public float pickDelay = 0.2f;

    //TEMPORALES
    bool facingright = true;
    float headCharge = 0f;
    float weaponCharge = 0f;
    bool picking = false;
    float pickTemp = 0;

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
                animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/Skin1");
                Debug.Log("Skin: 2");
                break;
        }

        switch (controller)
        {
            case Controller.PLAYER0:
                if (PlayerManager.Instance.Player1ON == true) { playerReady = true; player = ReInput.players.GetPlayer(0); }
                break;

            case Controller.PLAYER1:
                if (PlayerManager.Instance.Player2ON == true) { playerReady = true; player = ReInput.players.GetPlayer(1); }
                break;

            case Controller.PLAYER2:
                if (PlayerManager.Instance.Player2ON == true) { playerReady = true; player = ReInput.players.GetPlayer(2); }
                break;

            case Controller.PLAYER3:
                if (PlayerManager.Instance.Player2ON == true) { playerReady = true; player = ReInput.players.GetPlayer(3); }
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

    public bool isWeaponed = false;

    private void FixedUpdate(){
        if(!ReInput.isReady || player == null) {
            Debug.Log("not set or Disconnected"); //TODO: MESSAGE IN SCREEN
            return;
        }
        else if (playerReady == false)
        {
            Debug.Log("player not ready");
            return;
        };

        if (pickDelay <= pickTemp && picking == true) { picking = false; pickTemp = 0; }
        else if (picking == true) { pickTemp += Time.deltaTime; }
        
        //bool isCondition = animator.GetBool(ConditionID); //animator.SetBool(ConditionID, true);
        bool isGrounded = animator.GetBool(GroundingID);
        bool isMoving = animator.GetBool(MovingID);
        //bool isWeaponed = animator.GetBool(WeaponingID);
        int whichWeapon = animator.GetInteger(whichWeaponID);
        bool isCharging = animator.GetBool(ChargingID);
        bool isHeading = animator.GetBool(HeadingID);
        bool isDucking = false;

        //IDLE IS AUTOMATIC
        body2D.velocity = new Vector2(0, body2D.velocity.y);

        //MOVEMENT
        if (player.GetAxis("Move") > 0 )
        {
            if (isCharging == false && isHeading == false && isDucking == false)
            {
                body2D.velocity = new Vector2(runSpeed, body2D.velocity.y);
                spriteRenderer.flipX = true;
                animator.SetBool(MovingID, true);
                facingright = true;
            }
        }
        else if (player.GetAxis("Move") < 0)
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
            //animator.SetBool(WeaponingID, false);
            isWeaponed = false;
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
            emptyBody.arms = this.arms;
            emptyBody.LeftArm = LeftArm;
            LeftArm.transform.parent = emptyBody.transform;
            Debug.Log("Left-Brazo");
            emptyBody.RightArm = RightArm;
            RightArm.transform.parent = emptyBody.transform;
            Debug.Log("Right-Brazo");

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
        if (player.GetAxis("Move") > 0)
        {
            if (isCharging == true || isHeading == true)
            {
                body2D.velocity = new Vector2(runSpeed * 50 / 100, body2D.velocity.y); //(50% de max speed, velocidad actual y)
                spriteRenderer.flipX = true;
                facingright = true;
            }
        }
        else if (player.GetAxis("Move") < 0)
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
        //bool isWeaponed = animator.GetBool(WeaponingID); 

        if (collision.gameObject.tag == "PickUp" && player.GetButtonDown("PickUp") && isWeaponed == false && picking == false)
        {
            pickUpScript = collision.GetComponent<PickUpScript>();
            pickUpScript.Picker = this.gameObject;
            pickUpScript.picked = true;
            //animator.SetBool(WeaponingID, true);
            isWeaponed = true;

            switch (pickUpScript.picktype)
            {
                case PickTypes.Sword:
                    if (arms == Arms.NONE) { Debug.Log("NONE, cant pick"); break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 1);
                    break;
                case PickTypes.Axe:
                    if (arms == Arms.NONE) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 2);
                    break;
                case PickTypes.Spear:
                    if (arms == Arms.NONE) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 3);
                    break;
                case PickTypes.Bow:
                    if (arms == Arms.NONE || arms == Arms.ONE) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 4);
                    break;
                case PickTypes.CrossBow:
                    if (arms == Arms.NONE || arms == Arms.ONE) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 5);
                    break;
                case PickTypes.Boomerang:
                    if (arms == Arms.NONE || arms == Arms.ONE) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 6);
                    break;
            }
        }
        else if (collision.gameObject.tag == "Stuck" && player.GetButtonDown("PickUp") && isWeaponed == false && picking == false)  //RE-PICKUP
        {
            isWeaponed = true;
            switch (collision.gameObject.name)
            {
                case "place_sword(Clone)":
                    if (arms == Arms.NONE) { break; }
                    picking = true;
                    swordScript = collision.GetComponent<Sword>();
                    swordScript.Picker = this.gameObject;
                    animator.SetInteger(whichWeaponID, 1);
                    break;
                case "place_axe(Clone)":
                    if (arms == Arms.NONE) { break; }
                    picking = true;
                    axeScript = collision.GetComponent<Axe>();
                    axeScript.Picker = this.gameObject;
                    animator.SetInteger(whichWeaponID, 2);
                    break;
                case "place_spear(Clone)":
                    if (arms == Arms.NONE) { break; }
                    picking = true;
                    spearScript = collision.GetComponent<Spear>();
                    spearScript.Picker = this.gameObject;
                    animator.SetInteger(whichWeaponID, 3);
                    break;
                case "place_boomerang(Clone)":
                    if (arms == Arms.NONE || arms == Arms.ONE) { break; }
                    picking = true;
                    boomerangScript = collision.GetComponent<Boomerang>();
                    boomerangScript.Picker = this.gameObject;
                    animator.SetInteger(whichWeaponID, 6);
                    break;
            }
        }
        if (collision.gameObject.tag == "FreeArm" && player.GetButtonDown("PickUp") && picking == false)
        {
            switch (arms)
            {
                case Arms.NONE: //PICKUP ARM NOT HAVING ANY
                    picking = true;
                    arms = Arms.ONE;
                    Debug.Log(controller + "Armed:" + arms);

                    collision.gameObject.transform.parent.transform.parent = this.transform;
                    RightArm = collision.gameObject.transform.parent.gameObject;
                    collision.gameObject.transform.parent.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
                    collision.gameObject.transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    collision.gameObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    break;

                case Arms.ONE: //PICKUP ARM HAVING ONE
                    picking = true;
                    arms = Arms.TWO;
                    Debug.Log(controller + "Armed:" + arms);

                    collision.gameObject.transform.parent.transform.parent = this.transform;
                    LeftArm = collision.gameObject.transform.parent.gameObject;
                    collision.gameObject.transform.parent.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
                    collision.gameObject.transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    collision.gameObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    break; 

                case Arms.TWO: //CANT PICKUP ARM
                    Debug.Log("Nope");
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
            Rigidbody2D armRigid;
            BoxCollider2D armCollider;
            int whichWeapon = animator.GetInteger(whichWeaponID);
            //LOSE ARM
            switch (arms)
            {

                case Arms.NONE:
                    Debug.Log("boi u stupid"); //WHAT? why?
                    break;

                case Arms.ONE:
                    Debug.Log("armles"); //OUT RightArm
                    arms = Arms.NONE;

                    rightScript = RightArm.GetComponent<Arm>();
                    //rightScript.armType = ArmType.NONE;
                    RightArm.transform.parent = null;

                    armCollider = RightArm.GetComponent<BoxCollider2D>();
                    armCollider.enabled = true;
                    armCollider.isTrigger = true;

                    armRigid = RightArm.GetComponent<Rigidbody2D>();
                    armRigid.bodyType = RigidbodyType2D.Dynamic; //CHANGE TO DYNAMIC
                    RightArm = null;
                    break;

                case Arms.TWO:
                    Debug.Log("1 arm left"); //OUT LeftArm
                    arms = Arms.ONE;

                    leftScript = LeftArm.GetComponent<Arm>();
                    //leftScript.armType = ArmType.NONE;
                    LeftArm.transform.parent = null;

                    armCollider = LeftArm.GetComponent<BoxCollider2D>();
                    armCollider.enabled = true;
                    armCollider.isTrigger = true;

                    armRigid = LeftArm.GetComponent<Rigidbody2D>();
                    armRigid.bodyType = RigidbodyType2D.Dynamic; //CHANGE TO DYNAMIC
                    LeftArm = null;
                    break;
            }
            if (isWeaponed == true)
            {
                switch (whichWeapon)
                {
                    case 1:
                        if (arms == Arms.NONE) { PickedWeapon.transform.parent = null;
                            PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            swordScript = PickedWeapon.GetComponent<Sword>();
                            swordScript.Drop = true;
                            PickedWeapon = null;
                            isWeaponed = false; }
                        break;
                    case 2:
                        if (arms == Arms.NONE) { PickedWeapon.transform.parent = null;
                            PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            axeScript = PickedWeapon.GetComponent<Axe>();
                            axeScript.Drop = true;
                            PickedWeapon = null;
                            isWeaponed = false;
                        }
                        break;
                    case 3:
                        if (arms == Arms.NONE) { PickedWeapon.transform.parent = null;
                            PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            spearScript = PickedWeapon.GetComponent<Spear>();
                            spearScript.Drop = true;
                            PickedWeapon = null;
                            isWeaponed = false;
                        }
                        break;
                    case 4:
                        if (arms == Arms.NONE || arms == Arms.ONE) { PickedWeapon.transform.parent = null;
                            PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            bowScript = PickedWeapon.GetComponent<Bow>();
                            bowScript.Drop = true;
                            PickedWeapon = null;
                            isWeaponed = false;
                        }
                        break;
                    case 5:
                        if (arms == Arms.NONE || arms == Arms.ONE) { PickedWeapon.transform.parent = null;
                            PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            crossbowScript = PickedWeapon.GetComponent<CrossBow>();
                            spearScript.Drop = true;
                            PickedWeapon = null;
                            isWeaponed = false;
                        }
                        break;
                    case 6:
                        if (arms == Arms.NONE || arms == Arms.ONE) { PickedWeapon.transform.parent = null;
                            PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                            PickedWeapon.transform.GetComponent<BoxCollider2D>().enabled = true;
                            boomerangScript = PickedWeapon.GetComponent<Boomerang>();
                            boomerangScript.Drop = true;
                            PickedWeapon = null;
                            isWeaponed = false;
                        }
                        break;
                }
            }
        }

        if (collision.gameObject.tag == "Damage") //DEATH
        {
            Debug.Log("Touch Sierra [DEATH " + controller + "]");
            GameObject head = Instantiate(HeadFall, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);

            Rigidbody2D headRigid;
            headRigid = head.GetComponent<Rigidbody2D>(); //ASIGN HEAD RIGID
            headCharge = 2f;

            Rigidbody2D armRigid;
            BoxCollider2D armCollider;
            //LOSE ARM
            switch (arms)
            {
                case Arms.ONE:
                    rightScript = RightArm.GetComponent<Arm>();
                    //rightScript.armType = ArmType.NONE;
                    RightArm.transform.parent = null;

                    armCollider = RightArm.GetComponent<BoxCollider2D>();
                    armCollider.enabled = true;
                    armCollider.isTrigger = true;

                    armRigid = RightArm.GetComponent<Rigidbody2D>();
                    armRigid.bodyType = RigidbodyType2D.Dynamic; //CHANGE TO DYNAMIC
                    break;

                case Arms.TWO:
                    //LEFT ARM DOWN
                    leftScript = LeftArm.GetComponent<Arm>();
                    //leftScript.armType = ArmType.NONE;
                    LeftArm.transform.parent = null;

                    armCollider = LeftArm.GetComponent<BoxCollider2D>();
                    armCollider.enabled = true;
                    armCollider.isTrigger = true;

                    armRigid = LeftArm.GetComponent<Rigidbody2D>();
                    armRigid.bodyType = RigidbodyType2D.Dynamic;
                    armRigid.velocity = new Vector2(headCharge * 1.8f, 2f);

                    //RIGHT ARM DOWN
                    rightScript = RightArm.GetComponent<Arm>();
                    //rightScript.armType = ArmType.NONE;
                    RightArm.transform.parent = null;

                    armCollider = RightArm.GetComponent<BoxCollider2D>();
                    armCollider.enabled = true;
                    armCollider.isTrigger = true;

                    armRigid = RightArm.GetComponent<Rigidbody2D>();
                    armRigid.bodyType = RigidbodyType2D.Dynamic; 
                    armRigid.velocity = new Vector2(headCharge * 1.8f, 2f);
                    break;
            }

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