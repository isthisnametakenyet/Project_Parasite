using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadThrow : MonoBehaviour
{
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    bool BadThrow = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            BadThrow = true;
        }
    }
}
