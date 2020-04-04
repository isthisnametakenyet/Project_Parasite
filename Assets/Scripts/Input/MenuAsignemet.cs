using UnityEngine;
using System.Collections.Generic;
using Rewired;
using System;

[AddComponentMenu("")]
public class MenuAsignemet : MonoBehaviour
{
    // Static
    private static MenuAsignemet instance;
    public static MenuAsignemet Set { get; private set; }

    public static Rewired.Player GetRewiredPlayer(int gamePlayerId)
    {
        if (!Rewired.ReInput.isReady) return null;
        if (instance == null)
        {
            Debug.LogError("Not initialized. Do you have a MenuAsignemet in your scene?");
            return null;
        }
        for (int i = 0; i < instance.playerMap.Count; i++)
        {
            if (instance.playerMap[i].gamePlayerId == gamePlayerId) return ReInput.players.GetPlayer(instance.playerMap[i].rewiredPlayerId);
        }
        return null;
    }

    // Instance
    public int maxPlayers = 4;
    public int actualPlayers = 0;

    private List<PlayerMap> playerMap; // Maps Rewired Player ids to game player ids
    private int gamePlayerIdCounter = 0;

    void Awake()
    {
        playerMap = new List<PlayerMap>();
        instance = this; // set up the singleton
    }

    void Start()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            Player rewiredPlayer = ReInput.players.GetPlayer(i);

            // Enable the Assignment map category in Player
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Assignment");

            // Disable Control for this Player
            rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Default");
        }
        
    }

    void Update()
    {
        if (!PlayerManager.Instance) { Debug.LogError("Not initialized. Do you have an PlayerManager in your scene?"); }
        PlayerManager.Instance.numPlayers = actualPlayers;

        // Watch for JoinGame action in each Player
        for (int i = 0; i < ReInput.players.playerCount; i++) //ERROR: Rewired is not initialized. Do you have a Rewired Input Manager in the scene and enabled?
        {
            if (ReInput.players.GetPlayer(i).GetButtonDown("JoinGame"))
            {
                AssignNextPlayer(i);
            }
        }
    }

    void AssignNextPlayer(int rewiredPlayerId)
    {
        if (playerMap.Count >= maxPlayers)
        {
            Debug.LogError("Max player limit already reached!");
            return;
        }

        int gamePlayerId = GetNextGamePlayerId();
        actualPlayers++;

        // Add the Rewired Player as the next open game player slot
        playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

        Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

        // Disable the Assignment map category in Player so no more JoinGame Actions return
        rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");

        // Enable UI control for this Player now that he has joined
        rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");

        Debug.Log("Added Rewired Player id " + rewiredPlayerId + " to game player " + gamePlayerId);
    }

    private int GetNextGamePlayerId()
    {
        return gamePlayerIdCounter++;
    }

    // This class is used to map the Rewired Player Id to your game player id
    private class PlayerMap
    {
        public int rewiredPlayerId;
        public int gamePlayerId;

        public PlayerMap(int rewiredPlayerId, int gamePlayerId)
        {
            this.rewiredPlayerId = rewiredPlayerId;
            this.gamePlayerId = gamePlayerId;
        }
    }
}
