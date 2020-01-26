using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadThrow : MonoBehaviour
{
    public enum Controller { NONE, PLAYER1, PLAYER2, PLAYER3, PLAYER4 };
    public Controller controller = Controller.NONE;

    public enum Skin { NONE, SKIN1, SKIN2 };
    public Skin skin = Skin.NONE;

    bool BadThrow;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            BadThrow = false;
        }
    }
}
