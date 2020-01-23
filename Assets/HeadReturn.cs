using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadReturn : MonoBehaviour
{
    public enum Controller { NONE, PLAYER1, PLAYER2, PLAYER3, PLAYER4 };
    public Controller controller = Controller.NONE;

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
        Body = GameObject.Find("placeholder_Body(Clone)");
        animator = GetComponent<Animator>();
        body2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Stunned = false;

        if (controller == Controller.PLAYER1)
        {
            backButton = KeyCode.A;
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
            transform.position = Body.transform.position;
            animator.Play(BackID);
            spriteRenderer.flipX = true;
        }
    }
}
