using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum ArmType { NONE, LEFT, RIGHT }; //MAYBE DELETE

public class Arm : MonoBehaviour
{
    //public ArmType armType = ArmType.NONE; //MAYBE DELETE

    public GameObject pickUpRange;

    BoxCollider2D collider2D;
    Rigidbody2D body2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
        Debug.Log("Hand: FreeHand");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            collider2D.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
            body2D.isKinematic = true;
            body2D.velocity = new Vector2(0, 0);

            BoxCollider2D pickUpCollider;
            pickUpCollider = pickUpRange.GetComponent<BoxCollider2D>();
            pickUpCollider.enabled = true;
        }
    }
}
