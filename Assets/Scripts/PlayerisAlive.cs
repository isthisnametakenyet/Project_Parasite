using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerisAlive : MonoBehaviour
{
    public PlayerController2D PlayerScript;

    ///este script no sirve para nada mas que para mandarle al PlayerManager la señal
    ///de que el player a sido creado y/o esta listo para jugar TODO: IMPLEMENTARLO EN PaterController2D SCRIPT

    void Start()
    {
        switch (PlayerScript.controller)
        {
            case Controller.PLAYER0:
                PlayerManager.Instance.isAlivePlayer1 = true;
                break;

            case Controller.PLAYER1:
                PlayerManager.Instance.isAlivePlayer2 = true;
                break;

            case Controller.PLAYER2:
                PlayerManager.Instance.isAlivePlayer3 = true;
                break;

            case Controller.PLAYER3:
                PlayerManager.Instance.isAlivePlayer4 = true;
                break;
        }
    }
}
