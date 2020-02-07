using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public class HeadReturn : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    private Player player;

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
        switch (controller)
        {
            case Controller.PLAYER0:
                player = ReInput.players.GetPlayer(3);
                break;

            case Controller.PLAYER1:
                player = ReInput.players.GetPlayer(0);
                break;

            case Controller.PLAYER2:
                player = ReInput.players.GetPlayer(1);
                break;

            case Controller.PLAYER3:
                player = ReInput.players.GetPlayer(2);
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.GetButtonDown("Head Return") && Stunned == false)
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
