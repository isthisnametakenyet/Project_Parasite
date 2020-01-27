using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HeadReturn : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    public GameObject Body;
    public GameObject All;

    bool Stunned = true;

    KeyCode backButton = KeyCode.None;
    private int BackID;

    Animator animator;
    Rigidbody2D body2D;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Stunned = false;

        if (controller == Controller.PLAYER1)
        {
            backButton = KeyCode.Q;
            BackID = Animator.StringToHash("");
        }
        if (controller == Controller.PLAYER2)
        {
            backButton = KeyCode.B;
            BackID = Animator.StringToHash("");
        }
        if (controller == Controller.PLAYER3)
        {
            backButton = KeyCode.C;
            BackID = Animator.StringToHash("");
        }
        if (controller == Controller.PLAYER4)
        {
            backButton = KeyCode.D;
            BackID = Animator.StringToHash("");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(backButton) && Stunned == false)
        {
            //transform.position.y = Body.transform.position.y;
            animator.Play(BackID);
            //spriteRenderer.flipX = true;

            GameObject all = Instantiate(All, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            Destroy(Body);
            Destroy(gameObject);
        }
    }
}
