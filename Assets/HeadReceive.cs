using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadReceive : MonoBehaviour
{

    bool Parenting;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "FlyingHead" && Parenting == false)
        {
            
            //Destroy(collision.gameObject);
            Debug.Log("Head bump");
        }
    }
}
