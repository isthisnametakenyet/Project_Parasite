using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : Singleton <PlayerManager>
{
    public int numPlayers = 4;

    //public bool[] onPlayers;
    public bool Player1ON;
    public bool Player2ON;
    public bool Player3ON;
    public bool Player4ON;

    //public int[] scorePlayers = new int[4];
    //public IList<int> scorePlayersList = new List<int>(4);
    public int ScorePlayer1;
    public int ScorePlayer2;
    public int ScorePlayer3;
    public int ScorePlayer4;

    //public int[] skinPlayers = new int[4];
    public int SkinPlayer1;
    public int SkinPlayer2;
    public int SkinPlayer3;
    public int SkinPlayer4;

    public int tempskin;
    public void SetSkin (int temp) { tempskin = temp; }

    public void SetSkinTOPlayer(int player)
    {
        if (tempskin == -1) { Debug.LogError("ERROR: SkinSet(); skintemp was not changed (-1)"); return; }
        Debug.Log("player " + player + " now skin: " + tempskin);
        switch (player) {
            case 1:
                PlayerManager.Instance.SkinPlayer1 = tempskin;
                break;
            case 2:
                PlayerManager.Instance.SkinPlayer2 = tempskin;
                break;
            case 3:
                PlayerManager.Instance.SkinPlayer3 = tempskin;
                break;
            case 4:
                PlayerManager.Instance.SkinPlayer4 = tempskin;
                break;
        }
        tempskin = -1; //setea el temp a -1 al final de la operacion, para asegurarse de que en la siguiente se cambie antes de hacerla
    }

    public void StartGame() //GAME
    {
        PlayerManager.Instance.ScorePlayer1 = 0;
        PlayerManager.Instance.ScorePlayer2 = 0;
        PlayerManager.Instance.ScorePlayer3 = 0;
        PlayerManager.Instance.ScorePlayer4 = 0;

        Debug.Log("PlayerManager: Startgame();");
    }
}
