using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

//public enum AsignerMenu { NONE, PLAYER0, PLAYER1, PLAYER2, PLAYER3 };

public class AsignerController : MonoBehaviour
{
    private Rewired.Player player { get { return MenuAsignemet.GetRewiredPlayer(((int)controller) - 1); } }

    //public AsignerMenu asignerMenu = AsignerMenu.NONE;
    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;

    public bool asignerReady = false;

    void Update()
    {
        if (!ReInput.isReady || player == null)
        {
            return;
        }
        if (player == null && asignerReady == true) { MenuAsignemet.Set.actualPlayers--; asignerReady = false; }
        else { asignerReady = true; }
    }
}
