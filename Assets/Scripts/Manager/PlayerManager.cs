using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : Singleton <PlayerManager>
{
    public int numPlayers = 4;

    public int[] scorePlayers;

    public int[] skinPlayers;

    public bool[] onPlayers;
    public bool Player1ON;
    public bool Player2ON;
    public bool Player3ON;
    public bool Player4ON;

    private int[] inttemp;
    private bool[] booltemp;

    public void StartGame() //GAME
    {
        Debug.Log("PlayerManager: Startgame();");

        int[] scorePlayers = new int[numPlayers];
        //scorePlayers.CopyTo(inttemp, 0);
        scorePlayers = inttemp;

        bool[] booltemp = new bool[numPlayers];
        onPlayers.CopyTo(booltemp, 0);
        onPlayers = booltemp;

        //for (int i = 0; i < numPlayers; i++)
        //{
        //    scorePlayers[i] = 0;
        //}
    }
}
