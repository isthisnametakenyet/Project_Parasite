using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerManager : Singleton <PlayerManager>
{
    public int numPlayers = 4;
    public int[] scorePlayers;
    public bool Player1ON;
    public bool Player2ON;
    public bool Player3ON;
    public bool Player4ON;

    public int asignPlayerID;

    public void StartGame() //GAME
    {
        for (int i = 0; i < numPlayers; i++)
        {
            scorePlayers[i] = 0;
        }
    }
}
