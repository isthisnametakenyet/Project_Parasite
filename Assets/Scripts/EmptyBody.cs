using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

//public enum Controller { NONE, PLAYER0, PLAYER1, PLAYER2, PLAYER3 };
//public enum Skin { NONE, SKIN1, SKIN2 };

public class EmptyBody : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    private Player player;

    public GameObject Parasite;
    private HeadThrow parasiteScript;

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
    public float maxWeaponCharge = 3f;
    public float forgetWeaponChargeRange = 0.4f;
    public float expulseStrengh = 2f;

    //TEMPORALES
    public bool parasited = false;
    public bool expulseParasite = false;
    bool facingright = true;
    float weaponCharge = 0f;

    //CONDITIONS
    //private int GroundingID;
    //private int MovingID;
    //private int JumpedID;
    //private int WeaponingID;
    //private int whichWeaponID;
    //private int AttackedID;
    //private int ChargingID;
    //private int HeadingID;
    //private int DamagedID;
    bool isGrounded;
    bool isMoving;
    bool isWeaponed;
    bool isCharging;
    bool isDucking = false;
    int whichWeapon;

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
        //GroundingID = Animator.StringToHash("Grounding");
        //MovingID = Animator.StringToHash("Moving");
        //JumpedID = Animator.StringToHash("Jumped");
        //WeaponingID = Animator.StringToHash("Weaponing");
        //whichWeaponID = Animator.StringToHash("whichWeapon");
        //AttackedID = Animator.StringToHash("Attacked");
        //ChargingID = Animator.StringToHash("Charging");
        //HeadingID = Animator.StringToHash("Heading");
        //DamagedID = Animator.StringToHash("Damaged");

    }

    void FixedUpdate()
    {
        if (parasited == false) { }
        else
        {
            //bool isGrounded = animator.GetBool(GroundingID);
            //bool isMoving = animator.GetBool(MovingID);
            //bool isWeaponed = animator.GetBool(WeaponingID);
            //int whichWeapon = animator.GetInteger(whichWeaponID);
            //bool isCharging = animator.GetBool(ChargingID);
            //bool isHeading = animator.GetBool(HeadingID);
            //bool isDucking = false;

            //IDLE IS AUTOMATIC
            body2D.velocity = new Vector2(0, body2D.velocity.y);

            //MOVEMENT
            if (player.GetAxis("Move Joystick") > 0 || player.GetButton("Move Right Keys"))
            {
                if (isCharging == false && isDucking == false)
                {
                    body2D.velocity = new Vector2(runSpeed, body2D.velocity.y);
                    spriteRenderer.flipX = true;
                    isMoving = true;
                    //animator.SetBool(MovingID, true);
                    facingright = true;
                }
            }
            else if (player.GetAxis("Move Joystick") < 0 || player.GetButton("Move Left Keys"))
            {
                if (isCharging == false && isDucking == false)
                {
                    body2D.velocity = new Vector2(-runSpeed, body2D.velocity.y);
                    spriteRenderer.flipX = false;
                    isMoving = true;
                    //animator.SetBool(MovingID, true);
                    facingright = false;
                }
            }
            else
            {
                isMoving = false;
                //animator.SetBool(MovingID, false);
            }

            //JUMP
            if (player.GetButtonDown("Jump") && isGrounded == true && isCharging == false && isDucking == false)
            {
                //animator.SetTrigger(JumpedID);
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
                            Debug.Log("EWeapon: 1");
                            break;
                        case 2:
                            axeScript = PickedWeapon.GetComponent<Axe>();
                            axeScript.Attack = true;
                            Debug.Log("EWeapon: 2");
                            break;
                        case 3:
                            spearScript = PickedWeapon.GetComponent<Spear>();
                            spearScript.Attack = true;
                            Debug.Log("EWeapon: 3");
                            break;
                        case 4:
                            bowScript = PickedWeapon.GetComponent<Bow>();
                            bowScript.Attack = true;
                            Debug.Log("EWeapon: 4");
                            break;
                        case 5:
                            crossbowScript = PickedWeapon.GetComponent<CrossBow>();
                            crossbowScript.Attack = true;
                            Debug.Log("EWeapon: 5");
                            break;
                        case 6:
                            boomerangScript = PickedWeapon.GetComponent<Boomerang>();
                            boomerangScript.Attack = true;
                            Debug.Log("EWeapon: 6");
                            break;
                    }
                }
            }

            //CHARGED
            if (player.GetButton("Charge") && isDucking == false && isWeaponed == true)
            {
                Debug.Log("E-StartCharge");
                //animator.SetBool(ChargingID, true);
                if (weaponCharge < maxWeaponCharge)
                {
                    weaponCharge += Time.deltaTime;
                    Debug.Log(weaponCharge);
                    if (weaponCharge < forgetWeaponChargeRange)
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
            else if (weaponCharge < forgetWeaponChargeRange) { weaponCharge = 0; /*animator.SetBool(ChargingID, false);*/ }
            else if (weaponCharge > forgetWeaponChargeRange)
            {
                Debug.Log("E-Weapon: THROW");
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
                //animator.SetBool(ChargingID, false); //END ANIAMTION, BACK TO IDLE

                PickedWeapon.transform.parent = null;
                Rigidbody2D weaponRigid;
                weaponRigid = PickedWeapon.GetComponent<Rigidbody2D>(); //ASIGN HEAD RIGIDBODY
                weaponRigid.bodyType = RigidbodyType2D.Dynamic;

                if (facingright == true) //THROW WEAPON with headCharge as force
                {
                    weaponRigid.velocity = new Vector2(weaponCharge * 1.8f, 2f);
                }
                else if (facingright == false)
                {
                    weaponRigid.velocity = new Vector2(-weaponCharge * 1.8f, 2f);
                }
                weaponCharge = 0;
                //animator.SetBool(WeaponingID, false);
                isWeaponed = false;
            }

            //50% MOVEMENT
            if (player.GetAxis("Move Joystick") > 0 || player.GetButton("Move Right Keys"))
            {
                if (isCharging == true)
                {
                    body2D.velocity = new Vector2(runSpeed * 50 / 100, body2D.velocity.y); //(50% de max speed, velocidad actual y)
                    spriteRenderer.flipX = true;
                    facingright = true;
                }
            }
            else if (player.GetAxis("Move Joystick") < 0 || player.GetButton("Move Left Keys"))
            {
                if (isCharging == true)
                {
                    body2D.velocity = new Vector2(-runSpeed * 50 / 100, body2D.velocity.y);
                    spriteRenderer.flipX = false;
                    facingright = false;
                }
            }

            //HEAD RETURN
            if (player.GetButton("Head Throw") && isCharging == false && isDucking == false)
            {
                parasiteScript = Parasite.gameObject.GetComponent<HeadThrow>();
                Parasite.transform.parent = null;
                parasiteScript.GoBack = true;
                parasited = false;
            }

            //EXPULSE
            if (expulseParasite == true)
            {
                parasiteScript = Parasite.gameObject.GetComponent<HeadThrow>();
                Parasite.transform.parent = null;
                parasiteScript.Expulsed = true;
                parasited = false;
            }


        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = true;
            //animator.SetBool(GroundingID, true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Floor")
        {
            isGrounded = false;
            //animator.SetBool(GroundingID, false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //bool isWeaponed = animator.GetBool(WeaponingID);
        if (collision.gameObject.tag == "PickUp" && player.GetButtonDown("PickUp") && isWeaponed == false)
        {
            pickUpScript = collision.GetComponent<PickUpScript>();
            pickUpScript.Picker = this.gameObject;
            pickUpScript.picked = true;
            //animator.SetBool(WeaponingID, true);
            isWeaponed = true;
           
            switch (pickUpScript.picktype)
            {
                case PickTypes.Sword:
                    //animator.SetInteger(whichWeaponID, 1);
                    whichWeapon = 1;
                    break;
                case PickTypes.Axe:
                    //animator.SetInteger(whichWeaponID, 2);
                    whichWeapon = 2;
                    break;
                case PickTypes.Spear:
                    //animator.SetInteger(whichWeaponID, 3);
                    whichWeapon = 3;
                    break;
                case PickTypes.Bow:
                    //animator.SetInteger(whichWeaponID, 4);
                    whichWeapon = 4;
                    break;
                case PickTypes.CrossBow:
                    //animator.SetInteger(whichWeaponID, 5);
                    whichWeapon = 5;
                    break;
                case PickTypes.Boomerang:
                    //animator.SetInteger(whichWeaponID, 6);
                    whichWeapon = 6;
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hit = collision.contacts[0].normal;
        float angle = Vector3.Angle(hit, Vector3.up);

        if (collision.gameObject.tag == "Damage")
        {
            //animator.SetTrigger(DamagedID);

            parasiteScript = Parasite.gameObject.GetComponent<HeadThrow>();
            Parasite.transform.parent = null;
            parasiteScript.Expulsed = true;
            parasited = false;

            Rigidbody2D parasiteRigid;
            parasiteRigid = Parasite.GetComponent<Rigidbody2D>(); //ASIGN ITS RIGID

            if (Mathf.Approximately(angle, 90)) //DETECT COLLISION SIDE
            {
                Vector3 cross = Vector3.Cross(Vector3.forward, hit);
                if (cross.y == 1f) //RIGHT
                {
                    parasiteRigid.velocity = new Vector2(expulseStrengh * 1.8f, 2f);
                }
                else if (cross.y == -1f) //LEFT
                {
                    parasiteRigid.velocity = new Vector2(-expulseStrengh * 1.8f, 2f);
                }
                else { Debug.Log("---------ERROR DETEC DIREC COLLISION"); }
            }
        }
    }
}
