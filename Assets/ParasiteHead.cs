using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public class ParasiteHead : MonoBehaviour
{

    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    private Player player;

    public GameObject HeadFall;
    private HeadReturn headReturn;


    void Start()
    {
        switch (controller)
        {
            case Controller.PLAYER1:
                player = ReInput.players.GetPlayer(0);
                break;

            case Controller.PLAYER2:
                player = ReInput.players.GetPlayer(1);
                break;

            case Controller.PLAYER3:
                player = ReInput.players.GetPlayer(2);
                break;

            case Controller.PLAYER4:
                player = ReInput.players.GetPlayer(3);
                break;
        }
    }


    
}
