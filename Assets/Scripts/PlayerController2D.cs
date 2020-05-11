using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public enum Controller { NONE, PLAYER0, PLAYER1, PLAYER2, PLAYER3 };

public class PlayerController2D : MonoBehaviour
{
    //private Rewired.Player player { get { return PlayerAssignment.GetRewiredPlayer(((int)controller)-1); } }
    private Player player;
    public bool playerReady = false;

    public Controller controller = Controller.NONE;

    //PREFABS
    public GameObject ArmFallPrefab;

    public GameObject HeadFallPrefab;
    private HeadReturn headReturn;

    public GameObject HeadThrowPrefab;
    private HeadThrow headThrow;

    //TEMPORAL GAMEOBJECT
    public GameObject HeadThrown;

    public GameObject Parasiter;
    public Controller Parasitcontroller = Controller.NONE;

    //WEAPONS ATTACHED
    ///melee
    public GameObject Sword;
    public GameObject Axe;
    public GameObject Spear;
    private MeleeScript meleeScript;
    ///ranged
    public GameObject Bow;
    public GameObject CrossBow;
    ///boomerang
    public GameObject Boomerang;

    //PICKUP
    private PickUpScript pickUpScript;
    private WeaponScript weaponScript;

    //VARIABLES
    public int Arms = 2;
    public float runSpeed = 2f; 
    public float jumpCooldown = 2f;
    public float jumpStrengh = 6.5f;
    public float headReturnDelay = 2f;
    public float maxWeaponCharge = 1.5f;
    public float throwWeaponSpeed = 10f;
    public float headThrowCharge = 2f;
    public float forgetWeaponChargeRange = 0.3f;
    public float forgetHeadThrowRange = 0.4f;
    public float pickDelay = 0.2f;

    public bool Parasitable = false;
    private bool Parasited = false;
    private bool ReturnedHead = false;

    //TEMPORALES
    float jumpTemp = 0;

    public bool facingright = true;
    float headCharge = 0f;
    float weaponCharge = 0f;

    bool picking = false;
    float pickTemp = 0;

    //CONDITIONS
    private int GroundingID;     ///Bool
    private int MovingID;        ///Bool
    private int JumpedID;        ///Trigger
    private int DuckingID;       ///Bool
    private int whichWeaponID;   ///Int
    private int AttackedID;      ///Trigger
    private int ChargingID;      ///Bool
    private int ThrowedID;       ///Trigger
    private int HeadingID;       ///Bool
    private int DamagedID;       ///Trigger
    private int BreakID;         ///Trigger
    private int GetWeaponID;     ///Trigger
    private int LoseArmID;       ///Trigger
    private int GetArmID;        ///Trigger
    private int LoseHeadID;      ///Trigger
    private int GetHeadID;       ///Trigger
    private int StaticID;        ///Bool

    Animator animator;
    Rigidbody2D body2D;
    SpriteRenderer spriteRenderer;
    BoxCollider2D box2D;

    void Start()
    {
        //SoundManager.instance.Play("Shot1");
        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        GroundingID =   Animator.StringToHash("Grounding");
        MovingID =      Animator.StringToHash("Moving");
        JumpedID =      Animator.StringToHash("Jumped");
        DuckingID =     Animator.StringToHash("Ducking");
        whichWeaponID = Animator.StringToHash("whichWeapon");
        AttackedID =    Animator.StringToHash("Attacked");
        ChargingID =    Animator.StringToHash("Charging");
        ThrowedID =     Animator.StringToHash("Throwed");
        HeadingID =     Animator.StringToHash("Heading");
        DamagedID =     Animator.StringToHash("Damaged");
        BreakID =       Animator.StringToHash("Break");
        GetWeaponID =   Animator.StringToHash("GetWeapon");
        LoseArmID =     Animator.StringToHash("LoseArm");
        GetArmID =      Animator.StringToHash("GetArm");
        LoseHeadID =    Animator.StringToHash("LoseHead");
        GetHeadID =     Animator.StringToHash("GetHead");
        StaticID =      Animator.StringToHash("Static");
    }

    public bool isWeaponed = false;

    private void Update()
    {
        //IDLE IS AUTOMATIC
        body2D.velocity = new Vector2(0, body2D.velocity.y);

        //MOVEMENT
        Movement();
    }

    private void FixedUpdate(){
        //PLAYER / ROUND MANAGER
        if (PlayerManager.Instance.DeleteProps == true) { Destroy(gameObject); } //END ROUND
        if(!ReInput.isReady || player == null) {
            Debug.Log("not set or Disconnected"); //TODO: MESSAGE IN SCREEN
            return;
        }
        else if (playerReady == false)
        {
            Debug.Log("player not ready");
            return;
        };

        //TEMPS
        if (pickDelay <= pickTemp && picking == true) { picking = false; pickTemp = 0; }
        else if (picking == true) { pickTemp += Time.deltaTime; }

        if (jumpTemp <= jumpCooldown) { jumpTemp += Time.deltaTime; }

        //BOOLS
        int whichWeapon = animator.GetInteger(whichWeaponID);
        bool isCharging = animator.GetBool(ChargingID);
        bool isHeading =  animator.GetBool(HeadingID);
        bool isDucking =  animator.GetBool(DuckingID);
        bool isStatic =   animator.GetBool(StaticID);

        if (isStatic == false)
        {
            //ATTACK
            if (player.GetAxis("Attack&Charge") > 0)
            {
                animator.SetBool(AttackedID, true);
                if (isWeaponed == true)
                {
                    switch (whichWeapon)
                    {
                        case 1:
                            weaponScript = Sword.GetComponent<WeaponScript>();
                            weaponScript.Attack = true;
                            Debug.Log("PWeapon: 1");
                            break;
                        case 2:
                            weaponScript = Axe.GetComponent<WeaponScript>();
                            weaponScript.Attack = true;
                            Debug.Log("PWeapon: 2");
                            break;
                        case 3:
                            weaponScript = Spear.GetComponent<WeaponScript>();
                            weaponScript.Attack = true;
                            Debug.Log("PWeapon: 3");
                            break;
                        case 4:
                            weaponScript = Bow.GetComponent<WeaponScript>();
                            weaponScript.Attack = true;
                            Debug.Log("PWeapon: 4");
                            break;
                        case 5:
                            weaponScript = CrossBow.GetComponent<WeaponScript>();
                            weaponScript.Attack = true;
                            Debug.Log("PWeapon: 5");
                            break;
                        case 6:
                            weaponScript = Boomerang.GetComponent<WeaponScript>();
                            weaponScript.Attack = true;
                            Debug.Log("PWeapon: 6");
                            break;
                    }
                }
            }

            //CHARGED
            if (player.GetAxis("Attack&Charge") < 0 && isHeading == false && isDucking == false && isWeaponed == true)
            {
                animator.SetBool(ChargingID, true);
                if (weaponCharge < maxWeaponCharge)
                {
                    weaponCharge += Time.deltaTime;
                    Debug.Log(weaponCharge);
                }
                else if (weaponCharge >= maxWeaponCharge)
                {
                    Debug.Log("MaxCharge");
                }
            }
            else if (weaponCharge < forgetWeaponChargeRange) { weaponCharge = 0; animator.SetBool(ChargingID, false); }
            else if (weaponCharge > forgetWeaponChargeRange)
            {
                animator.SetTrigger(ThrowedID);
                animator.SetInteger(whichWeaponID, 0);
                animator.SetBool(ChargingID, false);

                weaponCharge = 0;
                isWeaponed = false;
            }

            //HEAD THROW
            if (player.GetAxis("HeadThrow&Return") > 0 && isCharging == false && isDucking == false)
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
                animator.SetBool(StaticID, true);
                animator.SetTrigger(LoseHeadID);
                GameObject head = Instantiate(HeadThrowPrefab, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);

                headThrow = head.GetComponent<HeadThrow>();
                headThrow.ParasiterController = this.controller;
                //headThrow.skin = this.skin;
                headThrow.OriginalBody = this.gameObject; //REFERENCE THIS IN HEAD THROW TO KNOW ORIGIN

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
                headCharge = 0;
                Parasitable = true;
            }
        }
    }



    //MOVEMENT
    void Movement()
    {
        //bool isCondition = animator.GetBool(ConditionID); //animator.SetBool(ConditionID, true);
        bool isGrounded = animator.GetBool(GroundingID);
        bool isMoving = animator.GetBool(MovingID);
        int whichWeapon = animator.GetInteger(whichWeaponID);
        bool isCharging = animator.GetBool(ChargingID);
        bool isHeading = animator.GetBool(HeadingID);
        bool isDucking = animator.GetBool(DuckingID);
        bool isStatic = animator.GetBool(StaticID);

        float moveInput = Input.GetAxisRaw("Horizontal");

        if(isStatic == false)
        {
            if (player.GetAxis("Move") > 0)
            {
                Debug.Log("Moving > 0");
                if (isCharging == false && isHeading == false && isDucking == false)
                {
                    body2D.velocity = new Vector2(runSpeed, body2D.velocity.y);
                    //spriteRenderer.flipX = true;
                    animator.SetBool(MovingID, true);
                }
            }
            else if (player.GetAxis("Move") < 0)
            {
                Debug.Log("Moving < 0");
                if (isCharging == false && isHeading == false && isDucking == false)
                {
                    body2D.velocity = new Vector2(-runSpeed, body2D.velocity.y);
                    //spriteRenderer.flipX = false;
                    animator.SetBool(MovingID, true);
                }
            }
            else
            {
                animator.SetBool(MovingID, false);
            }

            if (player.GetAxis("Move") > 0 && !facingright)
            {
                Flip();
                facingright = true;
            }
            else if (player.GetAxis("Move") < 0 && facingright)
            {
                Flip();
                facingright = false;
            }

            //FLIP
            void Flip()
            {
                facingright = !facingright;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            //JUMP
            if (player.GetButtonDown("Jump&Duck") && jumpTemp > jumpCooldown && isGrounded == true && isCharging == false && isDucking == false)
            {
                jumpTemp = 0;
                animator.SetTrigger(JumpedID);
                body2D.velocity = new Vector2(body2D.velocity.x, jumpStrengh);
            }

            ////DUCK
            //if (player.GetAxis("Jump&Duck") < 0 && isGrounded == true && isCharging == false)
            //{
            //    isDucking = true;
            //    Debug.Log("Quack");
            //}
            //else
            //{
            //    isDucking = false;
            //}

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
    }

    //PARASITE
    void Parasite(GameObject parasiteObject)
    {
        if (Parasitable == true)
        {
            Parasiter = parasiteObject;

            headThrow = Parasiter.GetComponent<HeadThrow>();
            this.Parasitcontroller = headThrow.ParasiterController;
            Parasited = true;
            Parasitable = false;

            switch (Parasitcontroller)
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

            Parasiter.transform.parent = this.gameObject.transform;
            Parasiter.transform.position = new Vector3(this.gameObject.transform.position.y, this.gameObject.transform.position.x, 0);
        }
    }

    //EXPULSE
    void Expulse()
    {
        Debug.Log("Expulse()");
        headThrow = Parasiter.GetComponent<HeadThrow>();
        headThrow.Expulsed = true;
        //body2D = Parasiter.GetComponent<Rigidbody2D>();
        //throw expulsed

        UnParasited();
    }


    //UNPARASITED
    void UnParasited()
    {
        Debug.Log("UnParasited()");
        Parasitcontroller = Controller.NONE;
        Parasiter = null;
        Parasited = false;

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

        animator.SetTrigger(GetHeadID);
    }


    //RETURN HEAD
    public void ReturnHead()
    {
        Debug.Log("ReturnHead()");
        animator.SetTrigger(GetHeadID);
        animator.SetBool(StaticID, false);
        Parasitable = false;
        if (Parasited == true)
        {
            Expulse();
        }
        else
        {
            UnParasited();
        }
    }


     
    //COLISIONS
    private void OnCollisioEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FlyingHead" && Parasitable == true) //PARASITE
        {
            Parasite(collision.gameObject);
            Debug.Log("Colision ENTER");
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

    //TRIGGERS
    private void OnTriggerStay2D(Collider2D collision) //PICKUP
    {
        if (collision.gameObject.tag == "PickUp" && player.GetButtonDown("Pickup") && isWeaponed == false && picking == false)
        {
            pickUpScript = collision.GetComponent<PickUpScript>();
            pickUpScript.picked = true;

            isWeaponed = true;

            Destroy(collision.gameObject);

            switch (pickUpScript.picktype)
            {
                case PickTypes.Sword:
                    if (Arms == 0) { Debug.Log("NONE, cant pick"); break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 1);
                    Sword.SetActive(true); //ACTIVATE
                    weaponScript = Sword.GetComponent<WeaponScript>();
                    weaponScript.Pickup();
                    break;
                case PickTypes.Axe:
                    if (Arms == 0) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 2);
                    Axe.SetActive(true); //ACTIVATE
                    weaponScript = Axe.GetComponent<WeaponScript>();
                    weaponScript.Pickup();
                    break;
                case PickTypes.Spear:
                    if (Arms == 0) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 3);
                    Spear.SetActive(true); //ACTIVATE
                    weaponScript = Spear.GetComponent<WeaponScript>();
                    weaponScript.Pickup();
                    break;
                case PickTypes.Bow:
                    if (Arms <= 1) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 4);
                    Bow.SetActive(true); //ACTIVATE
                    weaponScript = Bow.GetComponent<WeaponScript>();
                    weaponScript.Pickup();
                    break;
                case PickTypes.CrossBow:
                    if (Arms <= 1) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 5);
                    CrossBow.SetActive(true); //ACTIVATE
                    weaponScript = CrossBow.GetComponent<WeaponScript>();
                    weaponScript.Pickup();
                    break;
                case PickTypes.Boomerang:
                    if (Arms <= 1) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 6);
                    Boomerang.SetActive(true); //ACTIVATE
                    weaponScript = Boomerang.GetComponent<WeaponScript>();
                    weaponScript.Pickup();
                    break;
            }
             animator.SetTrigger(GetWeaponID);
            Debug.Log("GetWeapon");
        }
        else if (collision.gameObject.tag == "Stuck" && player.GetButtonDown("Pickup") && isWeaponed == false && picking == false)  //RE-PICKUP
        {
            isWeaponed = true;
            switch (collision.gameObject.name)
            {
                case "place_sword(Clone)":
                    if (Arms == 0) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 1);
                    Sword.SetActive(true); //ACTIVATE
                    weaponScript = Sword.GetComponent<WeaponScript>();
                    meleeScript = collision.gameObject.GetComponent<MeleeScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
                case "place_axe(Clone)":
                    if (Arms == 0) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 1);
                    Axe.SetActive(true); //ACTIVATE
                    weaponScript = Sword.GetComponent<WeaponScript>();
                    meleeScript = collision.gameObject.GetComponent<MeleeScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
                case "place_spear(Clone)":
                    if (Arms == 0) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 1);
                    Spear.SetActive(true); //ACTIVATE
                    weaponScript = Sword.GetComponent<WeaponScript>();
                    meleeScript = collision.gameObject.GetComponent<MeleeScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
                //case "place_boomerang(Clone)":
                //    if (Arms <= 1) { break; }
                //    picking = true;
                //    boomerangScript = collision.GetComponent<Boomerang>();
                //    boomerangScript.Picker = this.gameObject;
                //    animator.SetInteger(whichWeaponID, 6);
                //    Boomerang.SetActive(true); //ACTIVATE
                //    break;
            }
        }
        if (collision.gameObject.tag == "FreeArm" && player.GetButtonDown("Pickup") && picking == false)
        {
            switch (Arms)
            {
                case 0: //PICKUP ARM NOT HAVING ANY
                    picking = true;
                    Arms++;
                    Debug.Log(controller + "Armed:" + Arms);

                    collision.gameObject.transform.parent.transform.parent = this.transform;
                    collision.gameObject.transform.parent.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 1);
                    collision.gameObject.transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    collision.gameObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    break;

                case 1: //PICKUP ARM HAVING ONE
                    picking = true;
                    Arms++;
                    Debug.Log(controller + "Armed:" + Arms);

                    collision.gameObject.transform.parent.transform.parent = this.transform;
                    collision.gameObject.transform.parent.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -1);
                    collision.gameObject.transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
                    collision.gameObject.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    break; 

                case 2: //CANT PICKUP ARM
                    Debug.Log("Nope");
                    break;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision) //DAMAGE && PARASITE
    {
        if (collision.gameObject.tag == "Throwing"/* && collision.gameObject != PickedWeapon*/)
        {
            //animator.SetTrigger(DamagedID);
            GameObject head = Instantiate(HeadFallPrefab, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            animator.SetTrigger(LoseHeadID);

            headReturn = head.GetComponent<HeadReturn>();
            headReturn.controller = this.controller;
            //headReturn.skin = this.skin;
            headReturn.OriginalBody = this.gameObject;
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

            Parasitable = true;
        }

        if (collision.gameObject.tag == "Attacking"/* && collision.gameObject != PickedWeapon*/)
        {
            Rigidbody2D armRigid;
            BoxCollider2D armCollider;
            int whichWeapon = animator.GetInteger(whichWeaponID);
            //LOSE ARM
            switch (Arms)
            {
                case 1:
                    Debug.Log("armles"); //OUT RightArm
                    Arms--;

                    //ANIAMTOR LOSEARM
                    animator.SetTrigger(LoseArmID);
                    break;

                case 2:
                    Debug.Log("1 arm left"); //OUT LeftArm
                    Arms--;

                    //ANIAMTOR LOSEARM
                    animator.SetTrigger(LoseArmID);
                    break;
            }
            if (isWeaponed == true)
            {
                switch (whichWeapon)
                {
                    case 1:
                        if (Arms == 0)
                        {
                            weaponScript = Sword.GetComponent<WeaponScript>();
                            weaponScript.Drop();
                            Sword.SetActive(false);
                            isWeaponed = false;
                        }
                        break;
                    case 2:
                        if (Arms == 0)
                        {
                            weaponScript = Axe.GetComponent<WeaponScript>();
                            weaponScript.Drop();
                            Axe.SetActive(false);
                            isWeaponed = false;
                        }
                        break;
                    case 3:
                        if (Arms == 0)
                        {
                            weaponScript = Spear.GetComponent<WeaponScript>();
                            weaponScript.Drop();
                            Spear.SetActive(false);
                            isWeaponed = false;
                        }
                        break;
                    //case 4:
                    //    if (Arms <= 1)
                    //    {
                    //        PickedWeapon.transform.parent = null;
                    //        PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    //        bowScript = PickedWeapon.GetComponent<Bow>();
                    //        bowScript.Drop = true;
                    //        PickedWeapon = null;
                    //        isWeaponed = false;
                    //    }
                    //    break;
                    //case 5:
                    //    if (Arms <= 1)
                    //    {
                    //        PickedWeapon.transform.parent = null;
                    //        PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    //        crossbowScript = PickedWeapon.GetComponent<CrossBow>();
                    //        spearScript.Drop = true;
                    //        PickedWeapon = null;
                    //        isWeaponed = false;
                    //    }
                    //    break;
                    //case 6:
                    //    if (Arms <= 1)
                    //    {
                    //        PickedWeapon.transform.parent = null;
                    //        PickedWeapon.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    //        PickedWeapon.transform.GetComponent<BoxCollider2D>().enabled = true;
                    //        boomerangScript = PickedWeapon.GetComponent<Boomerang>();
                    //        boomerangScript.Drop = true;
                    //        PickedWeapon = null;
                    //        isWeaponed = false;
                    //    }
                    //    break;
                }
            }
        }

        if (collision.gameObject.tag == "Damage") //DEATH
        {
            Debug.Log("Touch Sierra [DEATH " + controller + "]");

            //LOSE HEAD
            GameObject head = Instantiate(HeadFallPrefab, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);

            Rigidbody2D headRigid;
            headRigid = head.GetComponent<Rigidbody2D>(); //ASIGN HEAD RIGID
            headCharge = 2f;
            headReturn = head.GetComponent<HeadReturn>();
            //headReturn.skin = this.skin;
            headReturn.controller = this.controller;
            headReturn.isDead = true;

            if (transform.position.x > collision.transform.position.x) //RIGHT
            {
                headRigid.velocity = new Vector2(headCharge, 2f);
            }
            else if (transform.position.x < collision.transform.position.x) //LEFT
            {
                headRigid.velocity = new Vector2(-headCharge, 2f);
            }
            else { Debug.LogError("Error Detectando Direccion de Collision"); }

            //LOSE ARM
            switch (Arms)
            {
                case 1:
                    //LEFT ARM DOWN
                    GameObject armDead = Instantiate(ArmFallPrefab, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);
                    Rigidbody2D armRigid;
                    armRigid = armDead.GetComponent<Rigidbody2D>();

                    if (transform.position.x > collision.transform.position.x) //RIGHT
                    {
                        armRigid.velocity = new Vector2(headCharge, 2f);
                    }
                    else if (transform.position.x < collision.transform.position.x) //LEFT
                    {
                        armRigid.velocity = new Vector2(-headCharge, 2f);
                    }
                    break;

                case 2:
                    //LEFT ARM DOWN
                    GameObject armDead1 = Instantiate(ArmFallPrefab, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);
                    Rigidbody2D armRigid1;
                    armRigid1 = armDead1.GetComponent<Rigidbody2D>();

                    //RIGHT ARM DOWN
                    GameObject armDead2 = Instantiate(ArmFallPrefab, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);
                    Rigidbody2D armRigid2;
                    armRigid2 = armDead2.GetComponent<Rigidbody2D>();

                    if (transform.position.x > collision.transform.position.x) //RIGHT
                    {
                        armRigid1.velocity = new Vector2(headCharge, 2f);
                        armRigid2.velocity = new Vector2(headCharge, 2f);
                    }
                    else if (transform.position.x < collision.transform.position.x) //LEFT
                    {
                        armRigid1.velocity = new Vector2(-headCharge, 2f);
                        armRigid2.velocity = new Vector2(-headCharge, 2f);
                    }
                    break;
            }
            Destroy(gameObject); //AUTODESTRUCCION
        }

        if (collision.gameObject.tag == "FlyingHead" && Parasitable == true) //PARASITE
        {
            Parasite(collision.gameObject);
        }
    }
}


