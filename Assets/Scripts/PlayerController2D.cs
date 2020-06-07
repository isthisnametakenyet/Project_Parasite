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

    #region InstanceVariables
    //PREFABS
    [Header("Prefabs")]
    public GameObject ArmFallPrefab;
    public GameObject HeadFallPrefab;
    public GameObject HeadThrowPrefab;
    public GameObject PickUpPrefab;

    //PAUSE MENU
    private PauseBehavior pauseBehavior;

    //SCRIPTS
    private HeadReturn headReturn;
    private HeadThrow headThrow;

    private PickUpScript pickUpScript;

    private WeaponScript weaponScript;
    private MeleeScript meleeScript;

    //PARASITER
    [Header("Parasiter")]
    public GameObject Parasiter;
    public Controller Parasitcontroller = Controller.NONE;

    //VARIABLES
    [Header("Variables")]
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

    [HideInInspector] public bool Parasitable = false;
    private bool Parasited = false;

    //WEAPONS ATTACHED
    [Header("Weapons")]
    ///melee
    public GameObject Sword;
    public GameObject Axe;
    public GameObject Spear;
    ///ranged
    public GameObject Bow;
    public GameObject CrossBow;
    ///boomerang
    public GameObject Boomerang;

    //TEMPORALES
    float jumpTemp = 0;

    [HideInInspector] public bool facingright = true;
    float headCharge = 0f;
    float weaponCharge = 0f;
    [HideInInspector] public bool isWeaponed = false;

    bool picking = false;
    float pickTemp = 0;
    #endregion

    //CONDITIONS
    private int GroundingID;     ///Bool
    private int MovingID;        ///Bool
    private int JumpedID;        ///Trigger
    private int DuckingID;       ///Bool
    private int whichWeaponID;   ///Int
    private int AttackedID;      ///Trigger
    private int ChargingID;      ///Bool
    private int HeadingID;       ///Bool
    private int DamagedID;       ///Trigger
    private int LoseArmID;       ///Trigger
    private int GetArmID;        ///Trigger
    private int LoseHeadID;      ///Trigger
    private int GetHeadID;       ///Trigger
    private int StaticID;        ///Bool
    
    //COMPONENTS
    Animator animator;
    Rigidbody2D body2D;
    Rigidbody2D thisbody2D;
    BoxCollider2D box2D;
    CircleCollider2D circle2D;

    void Start()
    {
        //SETTERS
        animator = GetComponent<Animator>();
        thisbody2D = GetComponent<Rigidbody2D>();

        //PAUSE
        pauseBehavior = GameObject.Find("Must").GetComponent<PauseBehavior>();

        //CONTROLLER
        switch (controller)
        {
            case Controller.PLAYER0:
                if (PlayerManager.Instance.Player1ON == true) { playerReady = true; player = ReInput.players.GetPlayer(0); PlayerManager.Instance.isAlivePlayer1 = true; }
                break;

            case Controller.PLAYER1:
                if (PlayerManager.Instance.Player2ON == true) { playerReady = true; player = ReInput.players.GetPlayer(1); PlayerManager.Instance.isAlivePlayer2 = true; }
                break;

            case Controller.PLAYER2:
                if (PlayerManager.Instance.Player2ON == true) { playerReady = true; player = ReInput.players.GetPlayer(2); PlayerManager.Instance.isAlivePlayer3 = true; }
                break;

            case Controller.PLAYER3:
                if (PlayerManager.Instance.Player2ON == true) { playerReady = true; player = ReInput.players.GetPlayer(3); PlayerManager.Instance.isAlivePlayer4 = true; }
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
        HeadingID =     Animator.StringToHash("Heading");
        DamagedID =     Animator.StringToHash("Damaged");
        LoseArmID =     Animator.StringToHash("LoseArm");
        GetArmID =      Animator.StringToHash("GetArm");
        LoseHeadID =    Animator.StringToHash("LoseHead");
        GetHeadID =     Animator.StringToHash("GetHead");
        StaticID =      Animator.StringToHash("Static");
    }


    private void Update() //todo en el update para q no hayan delays de ningun tipo
    {
        if (PlayerManager.Instance.Paused == false)
        {
            if (player.GetButtonDown("Pause"))
            {
                pauseBehavior.ActivatePause();
                Debug.Log("Player: Pause");
            }

            //IDLE IS AUTOMATIC
            thisbody2D.velocity = new Vector2(0, thisbody2D.velocity.y);

            //MOVEMENT
            Movement();

            //BOOLS
            int whichWeapon = animator.GetInteger(whichWeaponID);
            bool isCharging = animator.GetBool(ChargingID);
            bool isHeading = animator.GetBool(HeadingID);
            bool isDucking = animator.GetBool(DuckingID);
            bool isStatic = animator.GetBool(StaticID);

            //ATTACK, CHARGE, HEAD THROW
            if (isStatic == false)
            {
                //ATTACK
                if (player.GetButtonDown("Attack"))
                {
                    animator.SetBool(AttackedID, true);
                    Debug.Log("Player: Attack");
                }

                //CHARGED
                if (player.GetButton("Charge") && isHeading == false && isDucking == false && isWeaponed == true)
                {
                    animator.SetBool(ChargingID, true);
                    if (weaponCharge < maxWeaponCharge)
                    {
                        weaponCharge += Time.deltaTime;
                        //Debug.Log(weaponCharge);
                    }
                    else if (weaponCharge >= maxWeaponCharge)
                    {
                        Debug.Log("MaxCharge");
                    }
                }
                else if (weaponCharge < forgetWeaponChargeRange) { weaponCharge = 0; animator.SetBool(ChargingID, false); }
                else if (weaponCharge > forgetWeaponChargeRange)
                {
                    animator.SetInteger(whichWeaponID, 0);
                    animator.SetBool(ChargingID, false);
                    SoundManager.instance.Play("Throw");
                    weaponCharge = 0;
                    isWeaponed = false;
                }

                if (Parasited == false)
                {
                    //HEAD THROW
                    if (player.GetAxis("HeadThrow&Return") > 0 && isCharging == false && isDucking == false)
                    {
                        {
                            animator.SetBool(HeadingID, true);
                            if (headCharge <= headThrowCharge)
                            {
                                headCharge += Time.deltaTime;
                                //Debug.Log(headCharge);
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
                        GameObject head = Instantiate(HeadThrowPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);

                        headThrow = head.GetComponent<HeadThrow>();
                        headThrow.ParasiterController = this.controller;
                        //headThrow.skin = this.skin;
                        headThrow.OriginalBody = this.gameObject; //REFERENCE THIS IN HEAD THROW TO KNOW ORIGIN

                        Rigidbody2D headRigid;
                        headRigid = head.GetComponent<Rigidbody2D>(); //GET HEAD RIGIDBODY

                        Physics2D.IgnoreCollision(head.GetComponent<Collider2D>(), GetComponent<Collider2D>()); //IGNORE COLLISION WITH THIS OBJECT

                        if (facingright == true) //THROW HEAD with headCharge as force
                        {
                            SoundManager.instance.Play("Parasit");
                            headRigid.velocity = new Vector2(headCharge * 4f, 3f);
                        }
                        else if (facingright == false)
                        {
                            SoundManager.instance.Play("Parasit");
                            headRigid.velocity = new Vector2(-headCharge * 4f, 3f);
                        }

                        headCharge = 0;
                        Parasitable = true;
                    }
                }
                //IF PARASITED == TRUE, LO HACE EL HEAD THROW JUNTO CON LA FUNCION HeadReturn()
            }
        }
    }

    private void FixedUpdate(){
        //PLAYER MANAGEMENT
        if(!ReInput.isReady || player == null) {
            Debug.Log("not set or Disconnected"); //TODO: MESSAGE IN SCREEN
            return;
        }
        else if (playerReady == false)
        {
            Debug.Log("player not ready" + controller);
            return;
        };

        //TEMPS
        if (pickDelay <= pickTemp && picking == true) { picking = false; pickTemp = 0; }
        else if (picking == true) { pickTemp += Time.deltaTime; }

        if (jumpTemp <= jumpCooldown) { jumpTemp += Time.deltaTime; }
    }



    //MOVEMENT
    /// Movement (Left, Right, Jump, Duck, SlowLeft, SlowRight)
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
                SoundManager.instance.Play("WalkSFX");
                //Debug.Log("Moving > 0");
                if (isCharging == false && isHeading == false && isDucking == false)
                {
                    thisbody2D.velocity = new Vector2(runSpeed, thisbody2D.velocity.y);
                    animator.SetBool(MovingID, true);
                    
                }
            }
            else if (player.GetAxis("Move") < 0)
            {

                SoundManager.instance.Play("WalkSFX");
                //Debug.Log("Moving < 0");
                if (isCharging == false && isHeading == false && isDucking == false)
                {
                    thisbody2D.velocity = new Vector2(-runSpeed, thisbody2D.velocity.y);
                    animator.SetBool(MovingID, true);
                }
            }
            else
            {
                SoundManager.instance.Stop("WalkSFX");

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
            if (player.GetButtonDown("Jump&Duck") && jumpTemp > jumpCooldown && isGrounded == true && isDucking == false)
            {
                jumpTemp = 0;
                animator.SetTrigger(JumpedID);
                SoundManager.instance.Play("JumpSFX");
                thisbody2D.velocity = new Vector2(thisbody2D.velocity.x, jumpStrengh);
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
                SoundManager.instance.Play("WalkSFX");
                if (isCharging == true || isHeading == true)
                {
                    thisbody2D.velocity = new Vector2(runSpeed * 50 / 100, thisbody2D.velocity.y); //(50% de max speed, velocidad actual y)
                    facingright = true;
                }
            }
            else if (player.GetAxis("Move") < 0)
            {
                SoundManager.instance.Play("WalkSFX");
                if (isCharging == true || isHeading == true)
                {
                    thisbody2D.velocity = new Vector2(-runSpeed * 50 / 100, thisbody2D.velocity.y);
                    facingright = false;
                }
            }
        }
    }


    //PARASITE
    /// Function executed when this body gets parasited by enemy HeadThrow
    public void Parasite(GameObject parasiteObject)
    {
        if (Parasitable == true)
        {
            //SET FROM THROW
            Parasiter = parasiteObject;
            animator.SetBool(StaticID, false);
            animator.SetTrigger(GetHeadID);

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


            //SET THROW
            Parasiter.transform.parent = this.gameObject.transform;
            Parasiter.transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, 0);
            Parasiter.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);

            body2D = Parasiter.GetComponent<Rigidbody2D>();
            //body2D.bodyType = RigidbodyType2D.Static;
            body2D.bodyType = RigidbodyType2D.Kinematic;
            body2D.velocity = Vector3.zero;
            body2D.angularVelocity = 0;

            circle2D = Parasiter.GetComponent<CircleCollider2D>();
            circle2D.isTrigger = true;
        }
    }


    //RETURN HEAD -> EXPULSE
    /// Function executed when Headthrow of this body returns back
    public void ReturnHead()
    {
        Debug.Log("ReturnHead()");
        SoundManager.instance.Play("Parasit");
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


    //RUN HEAD -> UNPARASITED
    /// Function executed if the Parasiter goes back to their original body
    public void RunHead()
    {
        animator.SetTrigger(LoseHeadID);
        animator.SetBool(StaticID, true);
        SoundManager.instance.Play("Parasit");
        UnParasited();
    }


    //EXPULSE -> UNPARASITED
    /// Function executed when Headthrow of this body returns back and this body is parasited
    void Expulse()
    {
        Debug.Log("Expulse()");
        headThrow = Parasiter.GetComponent<HeadThrow>();
        headThrow.Expulse();
        SoundManager.instance.Play("PushSFX");
        UnParasited();
    }


    //UNPARASITED
    /// Function executed to clean Parasiter && ParasiterController varaibles and make the controller this body's original controller
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

    }


    //COLLISION THROW
    /// Function executed when Throw Weapon collides with player
    public void ThrowCollision(GameObject collision)
    {
        if (Parasitable != true && Parasited == false)
        {
            Debug.Log("ThrowCollision()");

            //animator.SetTrigger(DamagedID);
            GameObject head = Instantiate(HeadFallPrefab, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            animator.SetTrigger(LoseHeadID);
            animator.SetBool(StaticID, true);
            SoundManager.instance.Play("ArmLoseSFX");
            headReturn = head.GetComponent<HeadReturn>();
            headReturn.controller = this.controller;
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
        else if (Parasited == true)
        {
            headThrow = Parasiter.GetComponent<HeadThrow>();
            headThrow.Expulse();
        }
    }


    //DROP ARM
    /// Function used every time player loses an arm, instantiate FreeArm & hides player arm
    public void DropArm(GameObject collision)
    {
        Arms--;
        animator.SetTrigger(LoseArmID);
        SoundManager.instance.Play("loosearm");
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
    }


    //WEAPON BREAK
    /// Function used when uses of the current weapon reach 0
    public void WeaponBreak()
    {
        SoundManager.instance.Play("BreakWeapon");
        animator.SetInteger(whichWeaponID, 0);
    }



    //COLISIONS
    private void OnCollisionStay2D(Collision2D collision) //GROUNDING TRUE
    {
        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool(GroundingID, true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision) //GROUNDING FALSE
    {
        if (collision.gameObject.tag == "Floor")
        {
            animator.SetBool(GroundingID, false);
        }
    }


    //TRIGGERS
    private void OnTriggerStay2D(Collider2D collision) //PICKUP, STUCK, FREEARM
    {
        //PICK WEAPON
        if (collision.gameObject.tag == "PickUp" && player.GetButtonDown("Pickup") && isWeaponed == false && picking == false)
        {
            
            pickUpScript = collision.GetComponent<PickUpScript>();
            switch (pickUpScript.picktype)
            {
                case PickTypes.Sword:
                    if (Arms == 0) { Debug.Log("NONE, cant pick"); return; }
                    picking = true;
                    SoundManager.instance.Play("PickSword");
                    animator.SetInteger(whichWeaponID, 1);
                    Sword.SetActive(true); //ACTIVATE
                    weaponScript = Sword.GetComponent<WeaponScript>();
                    weaponScript.Pickup();
                    break;
                case PickTypes.Axe:
                    if (Arms == 0) { Debug.Log("NONE, cant pick"); return; }
                    picking = true;
                    SoundManager.instance.Play("PickAxe");
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

            pickUpScript.picked = true;
            isWeaponed = true;

            Destroy(collision.gameObject);
        }

        //PICK STUCK
        else if (collision.gameObject.tag == "Stuck" && player.GetButtonDown("Pickup") && isWeaponed == false && picking == false)
        {
            meleeScript = collision.GetComponent<MeleeScript>();
            switch (pickUpScript.picktype)
            {
                case PickTypes.Sword:
                    if (Arms == 0) { Debug.Log("NONE, cant pick"); return; }
                    picking = true;
                    SoundManager.instance.Play("PickSword");
                    animator.SetInteger(whichWeaponID, 1);
                    Sword.SetActive(true); //ACTIVATE
                    weaponScript = Sword.GetComponent<WeaponScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
                case PickTypes.Axe:
                    if (Arms == 0) { Debug.Log("NONE, cant pick"); return; }
                    picking = true;
                    SoundManager.instance.Play("PickAxe");
                    animator.SetInteger(whichWeaponID, 2);
                    Axe.SetActive(true); //ACTIVATE
                    weaponScript = Axe.GetComponent<WeaponScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
                case PickTypes.Spear:
                    if (Arms == 0) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 3);
                    Spear.SetActive(true); //ACTIVATE
                    weaponScript = Spear.GetComponent<WeaponScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
                case PickTypes.Bow:
                    if (Arms <= 1) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 4);
                    Bow.SetActive(true); //ACTIVATE
                    weaponScript = Bow.GetComponent<WeaponScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
                case PickTypes.CrossBow:
                    if (Arms <= 1) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 5);
                    CrossBow.SetActive(true); //ACTIVATE
                    weaponScript = CrossBow.GetComponent<WeaponScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
                case PickTypes.Boomerang:
                    if (Arms <= 1) { break; }
                    picking = true;
                    animator.SetInteger(whichWeaponID, 6);
                    Boomerang.SetActive(true); //ACTIVATE
                    weaponScript = Boomerang.GetComponent<WeaponScript>();
                    weaponScript.Uses = meleeScript.Uses;
                    break;
            }

            isWeaponed = true;

            Destroy(collision.gameObject);
        }

        //PICK FREEARM
        if (collision.gameObject.tag == "FreeArm" && player.GetButtonDown("Pickup") && picking == false && Arms < 2)
        {
            picking = true;
            SoundManager.instance.Play("PickArm");
            Arms++;
            animator.SetTrigger(GetArmID);

            Destroy(collision.gameObject.transform.parent.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) //DAMAGE, ATTACKING
    {
        //LOSE ARM
        if (collision.gameObject.tag == "Attacking")
        {
            int whichWeapon = animator.GetInteger(whichWeaponID);

            //LOSE ARM
            if (Arms > 0) { DropArm(collision.gameObject); SoundManager.instance.Play("FallWeapon"); }

            //DROP WEAPON
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
                            animator.SetInteger(whichWeaponID, 0);
                        }
                        break;
                    case 2:
                        if (Arms == 0)
                        {
                            weaponScript = Axe.GetComponent<WeaponScript>();
                            weaponScript.Drop();
                            Axe.SetActive(false);
                            isWeaponed = false;
                            animator.SetInteger(whichWeaponID, 0);
                        }
                        break;
                    case 3:
                        if (Arms == 0)
                        {
                            weaponScript = Spear.GetComponent<WeaponScript>();
                            weaponScript.Drop();
                            Spear.SetActive(false);
                            isWeaponed = false;
                            animator.SetInteger(whichWeaponID, 0);
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


        //DEATH
        if (collision.gameObject.tag == "Damage") 
        {
            Debug.Log("Touch Sierra [DEATH " + controller + "]");
            SoundManager.instance.Play("Death");
            //RETURN PARASITE
            if (Parasited == true)
            {
                headThrow = Parasiter.GetComponent<HeadThrow>();
                headThrow.Parasiting = false;
                headThrow.GoBack();
            }

            //LOSE ARMS
            if (Arms > 0) { DropArm(collision.gameObject); }
            if (Arms > 0) { DropArm(collision.gameObject); }


            //LOSE ROUND
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
            Destroy(gameObject); //AUTODESTRUCCION
        }
    }
}


