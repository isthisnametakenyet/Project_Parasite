﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour
{
    public GameObject Picker;
    private PlayerController2D pickerPlayerScript;
    private EmptyBody pickerEmptyScript;
    private PlayerController2D playerScript;
    private EmptyBody emptyScript;
    public Sprite crossbowSprite;
    public GameObject Arrow;
    private Arrow arrowScript;

    //STATE
    public bool Idle;
    public bool Attack = false;
    public bool Charging = false;
    public bool Thrown = false;
    public bool Drop = false;
    private bool inUse = false;

    //VARIABLES
    public int Uses = 1;
    public float AttackTime = 2f;
    private float actualAttack = 0f;
    public float arrowSpeed = 2f;
    BoxCollider2D collider2D;
    Rigidbody2D body2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        if (Picker.gameObject.tag == "Player") { pickerPlayerScript = Picker.GetComponent<PlayerController2D>(); }
        else if (Picker.gameObject.tag == "EmptyBody") { pickerEmptyScript = Picker.GetComponent<EmptyBody>(); }
        Idle = true;
        collider2D.isTrigger = true;
        body2D.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (Drop == true)
        {
            Idle = false;
        }
        else if (Idle == true && collider2D.enabled == true && inUse == false && Thrown == false)
        {
            Debug.Log("Wp: Idle");
            actualAttack = 0f;
            collider2D.enabled = false;
            transform.gameObject.tag = "Weapon";
            //START ANIMATION IDLE
        }
        else if (Attack == true && inUse == false && Thrown == false)
        {
            Debug.Log("Wp: Attack");
            collider2D.enabled = true;
            transform.gameObject.tag = "Attacking";
            //START ANIMATION ATTACKING
            actualAttack = 0f;
            inUse = true;
            Idle = false;
            Uses--;
        }
        else if (Charging == true && inUse == false && Thrown == false)
        {
            //Debug.Log("Wp: Charging");
            //BEFORE THIS, IDLE, ALLWAYS
            collider2D.enabled = false;
            //START ANIMATION CHARGING
            inUse = true;
        }
        else if (Thrown == true)
        {
            Debug.Log("Wp: Shot");
            Charging = false;
            Idle = false;
            inUse = false;


            GameObject arrow = Instantiate(Arrow, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            if (arrow == null) { Debug.LogError("arrow prefab not set"); }

            Rigidbody2D arrowRigid;
            arrowRigid = arrow.GetComponent<Rigidbody2D>(); //ASIGN ITS RIGID
            arrowRigid.velocity = this.body2D.velocity;
            this.body2D.velocity = new Vector2(0, 0);

            //START ANIMATION BREAK

            arrowScript = arrow.GetComponent<Arrow>();
            arrowScript.Picker = Picker;

            Destroy(gameObject);
        }
        if (actualAttack <= AttackTime && inUse == true && Charging == false) { actualAttack += Time.deltaTime; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" && Drop == true)
        {
            Debug.Log("Dropped");
            body2D.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            body2D.velocity = new Vector2(0, 0);
            transform.gameObject.tag = "Stuck";
        }
    }
}