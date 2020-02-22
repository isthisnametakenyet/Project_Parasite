using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    public int numPlayers;
    public int[] scorePlayers;
    public bool Player1ON;
    public bool Player2ON;
    public bool Player3ON;
    public bool Player4ON;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Debug.Log("WARNING: multiple " + this + " in scene");
        }
    }

    public void AddPlayer() //MENU 
    {
        numPlayers++;

    }

    public void DeletePlayer() //MENU
    {
        numPlayers--;

    }

    public void StartGame() //GAME
    {
        for (int i = 0; i < numPlayers; i++)
        {
            scorePlayers[i] = 0;

        }
    }
}
