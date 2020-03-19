using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptableColorScript : MonoBehaviour
{
    public GameObject ParentController;
    private PlayerController2D parentScript;

    SpriteRenderer renderer;

    Color Player1Red;
    Color Player2Purple;
    Color Player3Yellow;
    Color Player4Green;
    //Red ON: FF0000
    //Purple ON: 7700E7
    //Yellow ON: FFFF00
    //Green ON: 00FF00

    void Start()
    {
        ColorUtility.TryParseHtmlString("#FF0000", out Player1Red);
        ColorUtility.TryParseHtmlString("#7700E7", out Player2Purple);
        ColorUtility.TryParseHtmlString("#FFFF00", out Player3Yellow);
        ColorUtility.TryParseHtmlString("#00FF00", out Player4Green);

        renderer = GetComponent<SpriteRenderer>();
        parentScript = ParentController.GetComponent<PlayerController2D>();
        switch (parentScript.controller)
        {
            case Controller.PLAYER0:
                renderer.color = Player1Red;
                break;
            case Controller.PLAYER1:
                renderer.color = Player2Purple;
                break;
            case Controller.PLAYER2:
                renderer.color = Player3Yellow;
                break;
            case Controller.PLAYER3:
                renderer.color = Player4Green;
                break;
        }
        this.enabled = false;
    }
}
