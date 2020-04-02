using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum Skin { NONE, SKIN1, SKIN2, SKIN3 };
// public enum Arms { NONE, ONE, TWO };

public class DummyController : MonoBehaviour
{
    //public Skin skin = Skin.NONE;
    public Arms arms = Arms.NONE;

    public bool playerReady = false;

    public GameObject HeadFall;
    public GameObject BodyEmpty;
    public GameObject RightArm;
    public GameObject LeftArm;
    private EmptyBody emptyBody;
    private Arm rightScript;
    private Arm leftScript;
    private Transform ArmParent;
    private SpriteRenderer LArmRenderer;
    private SpriteRenderer RArmRenderer;
    private SpriteRenderer HeadRenderer;
    
    Rigidbody2D body2D;
    SpriteRenderer spriteRenderer;
    BoxCollider2D box2D;

    void Start()
    {
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        LArmRenderer = LeftArm.GetComponent<SpriteRenderer>();
        RArmRenderer = RightArm.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) //DAMAGE
    {
        if (collision.gameObject.tag == "Throwing")
        {
            //animator.SetTrigger(DamagedID);
            GameObject head = Instantiate(HeadFall, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);
            GameObject body = Instantiate(BodyEmpty, new Vector3(transform.position.x, transform.position.y - 0.22f, 0), Quaternion.identity);

            Rigidbody2D headRigid;
            headRigid = head.GetComponent<Rigidbody2D>(); //ASIGN ITS RIGID
            float headCharge = 2f;
            
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

        if (collision.gameObject.tag == "Attacking")
        {
            Rigidbody2D armRigid;
            BoxCollider2D armCollider;
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
            
        }

        if (collision.gameObject.tag == "Damage") //DEATH
        {
            GameObject head = Instantiate(HeadFall, new Vector3(transform.position.x, transform.position.y + 0.3f, 0), Quaternion.identity);

            Rigidbody2D headRigid;
            headRigid = head.GetComponent<Rigidbody2D>(); //ASIGN HEAD RIGID
            float headCharge = 2f;


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
