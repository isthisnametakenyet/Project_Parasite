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
    public float MAX_DamageStun = 1f;

    //GROUND
    public bool Grounded;

    //ATTACK
    bool Attacking; 

    //DAMAGE
    bool Damaged;   

    //WEAPONS
    bool Weaponed;

    float charge = 0f;



    bool IndividualWait = false;

    KeyCode leftButton = KeyCode.None;
    KeyCode rightButton = KeyCode.None;
    KeyCode jumpButton = KeyCode.None;
    KeyCode attackButton = KeyCode.None;
    KeyCode chargeButton = KeyCode.None;
    KeyCode headButton = KeyCode.None;

    private int IdleID;
    private int RunID;
    private int JumpID;
    private int HitID;
    private int HurtID;
    private int HeadID;

    Animator animator;
    Rigidbody2D body2D;
    SpriteRenderer spriteRenderer; //Debug.Log("" + Time.time);

    void Start(){
        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //switch (controller)
        //{
        //    case Controller.PLAYER1:    //error
        //        {
        //            leftButton = KeyCode.A;
        //        }
        //    case Controller.PLAYER2:    //error
        //        {
        //            leftButton = KeyCode.LeftArrow;
        //        }

        //    default: break;
        //}

        //https://docs.unity3d.com/ScriptReference/KeyCode.html
        if (controller == Controller.PLAYER1)
        {
            leftButton = KeyCode.A;
            rightButton = KeyCode.D;
            jumpButton = KeyCode.Space;
            attackButton = KeyCode.E;
            chargeButton = KeyCode.Mouse1;
            headButton = KeyCode.Z;

            IdleID = Animator.StringToHash("placeholder_Idle");
            RunID = Animator.StringToHash("placeholder_Move");
            //JumpID = Animator.StringToHash("");
            HitID = Animator.StringToHash("Player_Hit");
            HurtID = Animator.StringToHash("");
            // charge1Anim = animator.Play("Player_Charge1");
            // charge2Anim = animator.Play("Player_Charge2");
            HeadID = Animator.StringToHash("");
        }
        else if (controller == Controller.PLAYER2)
        {
            leftButton = KeyCode.LeftArrow;
            rightButton = KeyCode.RightArrow;
            jumpButton = KeyCode.UpArrow;
            attackButton = KeyCode.None;
            chargeButton = KeyCode.None;
            IdleID = Animator.StringToHash("");
            RunID = Animator.StringToHash("");
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
    }

    private void FixedUpdate ()
    {
    //GetKey: repite cada segundo q se presiona | GetKeyDown: solo one vez al presionar | GetKeyUp: solo one vez al soltar
        
        //MOVEMENT
        if(Input.GetKey(rightButton) && Attacking == false){
            body2D.velocity = new Vector2(runSpeed, body2D.velocity.y); //(new x, velocidad actual y)
            animator.Play(RunID);
            spriteRenderer.flipX = true;
        }
        else if(Input.GetKey(leftButton) && Attacking == false){
            body2D.velocity = new Vector2(-runSpeed, body2D.velocity.y);
            animator.Play(RunID);
            spriteRenderer.flipX = false;
        }

        //IDLE
        else if(Attacking == false){
            body2D.velocity = new Vector2(0, body2D.velocity.y); //Si no se pulsa nada = Idle
            animator.Play(IdleID);
        }

        //JUMP
        if(Input.GetKey(jumpButton) && Grounded == true && Attacking == false){
            body2D.velocity = new Vector2(body2D.velocity.x, jumpStrengh); //(velocidad actual x, new y)
        }

        //ATTACK NORMAL
        if(Input.GetKeyDown(attackButton)){
            Attacking = true;
            //animator.Play(HitID);
            StartCoroutine(ExecuteAfterTime(0.45f, () => {
                Attacking = false;
            }));
        }

        //ATTACK CHARGED
        if (Input.GetKey(chargeButton)){
            Attacking = true;
            //animator.Play("charge1Anim");
            if (charge < 3f){
                charge += Time.deltaTime*1f;
                Debug.Log(charge);
            }
            else if (charge == 3f){
                Debug.Log("MaxCharge");
            }
        }
        else if (charge > 0){{
            //animator.Play("charge2Anim");
            StartCoroutine(ExecuteAfterTime(1, () => { Attacking = false; }));
            charge = 0;
            }
        }

        if(Input.GetKey(rightButton) && Attacking == true){
            body2D.velocity = new Vector2(runSpeed * 50 / 100, body2D.velocity.y); //(50% de max speed, velocidad actual y)
            spriteRenderer.flipX = true;
        }
        else if(Input.GetKey(leftButton) && Attacking == true){
            body2D.velocity = new Vector2(-runSpeed * 50 / 100, body2D.velocity.y);
            spriteRenderer.flipX = false;
        }

        //DAMAGED
        if(Damaged == true && IndividualWait == false){
            //animator.Play(HurtID);
            HeadOFF();
        }

        //HEAD THROW
        if (Input.GetKey(headButton) && Attacking == false)
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


        IEnumerator ExecuteAfterTime(float seconds, Action task) { //WAIT TIME
        if (IndividualWait == false)
            {
            IndividualWait = true;

            yield return new WaitForSeconds(seconds);
            task();

            IndividualWait = false;
            }
        }   
    }

    private void OnCollisionStay2D(Collision2D collision) { //ON STAY SOLO CON EL SUELO, Q ES MUY PESADO EN CPU

        Vector3 hit = collision.contacts[0].normal;
        float angle = Vector3.Angle(hit, Vector3.up);


        if (collision.gameObject.tag == "Floor")
        {

            if (Mathf.Approximately(angle, 0))
            {
                Grounded = true; //Collision Down
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Damage")   {
            Damaged = true;
        }

        if (collision.gameObject.tag == "Pickup")
        {
            Weaponed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.tag == "Floor") {
            Grounded = false;
        }

        if (collision.gameObject.tag == "Damage")   {
            Damaged = false;
        }
    }

    private void HeadOFF() {
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