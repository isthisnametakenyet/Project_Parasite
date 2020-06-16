using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmPush : MonoBehaviour
{
    bool pushing = false;

    public float pushStrengh;

    private CircleCollider2D ArmCol;

    Rigidbody2D ColRig; 

    void Start()
    {
        ArmCol = GetComponent<CircleCollider2D>();
        ArmCol.enabled = false;
    }

    public void PushAactivate()
    {
        if (pushing == false)
        {
            Debug.Log("Arm: PushActivate()");
            bool pushing = true;
            ArmCol.enabled = true;
            StartCoroutine("PushDesactivate");
        }
    }

    IEnumerator PushDesactivate()
    {
        yield return new WaitForSeconds(0.6f);
        Debug.Log("Arm: PushDesactivate()");
        bool pushing = false;
        ArmCol.enabled = false;
    }


    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject != this.gameObject.transform.parent)
        {
            Debug.Log("Pushing Player1");
            ColRig = collision.GetComponent<Rigidbody2D>();

            if (transform.position.x > collision.transform.position.x) //RIGHT
            {
                ColRig.velocity = new Vector2(ColRig.velocity.x - pushStrengh, 2f); ///axis X: teletransporta en lugar de empujar. axis Y: empuja bien... ????
                //ColRig.AddForce(Vector3.left * pushStrengh, ForceMode2D.Impulse); ///teletransporta en lugar de empujar
            }
            else if (transform.position.x < collision.transform.position.x) //LEFT
            {
                ColRig.velocity = new Vector2(ColRig.velocity.x + pushStrengh, 2f);
                //ColRig.AddForce(Vector3.right * pushStrengh, ForceMode2D.Impulse);
            }
        }
    }
}
