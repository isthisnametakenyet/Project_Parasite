using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerisAlive : MonoBehaviour
{
    public GameObject Player;
    private PlayerController2D PlayerScript;

    void Start()
    {
        PlayerScript = Player.gameObject.GetComponent<PlayerController2D>();

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
