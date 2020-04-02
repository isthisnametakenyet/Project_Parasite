using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : Singleton <PlayerManager>
{
    public int numPlayers = 4;
    public bool gameON = false;
    private bool WinRound = false;
    private bool RestartingRound = false;
    private bool WinGame = false;
    public int Round = 0;
    public bool DeleteProps = false;
    public bool GameEnd = false;

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

    //public bool[] isAlivePlayers;
    public bool isAlivePlayer1;
    public bool isAlivePlayer2;
    public bool isAlivePlayer3;
    public bool isAlivePlayer4;

    int tmpInt = 0;
    private void FixedUpdate()
    {
        if (gameON == true)
        {
            tmpInt = 0;
            if(isAlivePlayer1 == true) { tmpInt++; }
            if(isAlivePlayer2 == true) { tmpInt++; }
            if(isAlivePlayer3 == true) { tmpInt++; }
            if(isAlivePlayer4 == true) { tmpInt++; }
            if (tmpInt == 1) { WinRound = true; } //1 ALIVE
        }
        if (WinRound == true)
        {

            if (isAlivePlayer1 == true) { ScorePlayer1++; Round = 1; }
            if (isAlivePlayer2 == true) { ScorePlayer2++; Round = 2; }
            if (isAlivePlayer3 == true) { ScorePlayer3++; Round = 3; }
            if (isAlivePlayer4 == true) { ScorePlayer4++; Round = 4; }
            Debug.Log("a: RestartRound();");
            RestartRound();
        }

        if (ScorePlayer1 == 5) { WinGame = true; }
        else if (ScorePlayer2 == 5) { WinGame = true; }
        else if (ScorePlayer3 == 5) { WinGame = true; }
        else if (ScorePlayer4 == 5) { WinGame = true; }
    }

    private void RestartRound()
    {
        Debug.Log("PlayerManager: RestartRound();");
        DeleteProps = true;
        //DeleteProps = false; //PlayerGenerator lo pone en false y re-Spawnea los jugadores
    }

    private void EndGame()
    {
        Debug.Log("PlayerManager: EndGame();");
        DeleteProps = true;
        gameON = false;
    }

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

        if (Player1ON == true) { isAlivePlayer1 = true; Debug.Log("PlayerManager: FKU();"); }
        if (Player2ON == true) { isAlivePlayer2 = true; }
        if (Player3ON == true) { isAlivePlayer3 = true; }
        if (Player4ON == true) { isAlivePlayer4 = true; }

        DeleteProps = false;
        gameON = true;
        Debug.Log("PlayerManager: Startgame();");
    }
}
