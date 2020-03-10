using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject Picker;
    private PlayerController2D pickerPlayerScript;
    private EmptyBody pickerEmptyScript;
    private PlayerController2D playerScript;
    private EmptyBody emptyScript;
    public Sprite arrowSprite;

    //VARIABLES
    private float actualStuck = 0f;
    public float stuckTime = 0.3f;

    BoxCollider2D collider2D;
    Rigidbody2D body2D;
    SpriteRenderer render;
    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        if (body2D.velocity.x < 0) { render.flipX = true; }
    }

    void FixedUpdate()
    {
        if (actualStuck >= stuckTime)
        {
            Debug.Log("Wp: Stuck");
            body2D.velocity = new Vector2(0, 0);
            actualStuck = 0f;
            collider2D.enabled = false;
            this.enabled = false;
            transform.gameObject.tag = "Untagged";
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            Debug.Log("Wp: Landed");
            actualStuck += Time.deltaTime * 10;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        }

        if (collision.gameObject.tag == "Player")
        {
            playerScript = collision.GetComponent<PlayerController2D>();
            if (Picker.gameObject.tag == "Player" && playerScript.controller != pickerPlayerScript.controller
                || Picker.gameObject.tag == "EmptyBody" && playerScript.controller != pickerEmptyScript.controller)
            {
                Debug.LogError("Wp: Hit");
                actualStuck += Time.deltaTime * 10;
                transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                this.transform.parent = collision.transform;
            }
        }

        if (collision.gameObject.tag == "EmptyBody")
        {
            emptyScript = collision.GetComponent<EmptyBody>();
            if (Picker.gameObject.tag == "Player" && emptyScript.controller != pickerPlayerScript.controller
                || Picker.gameObject.tag == "EmptyBody" && emptyScript.controller != pickerEmptyScript.controller)
            {
                Debug.LogError("Wp: Hit");
                actualStuck += Time.deltaTime * 10;
                transform.position = new Vector3(transform.position.x, transform.position.y, 1);
                this.transform.parent = collision.transform;
            }
        }
    }
}
