using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerJoin : MonoBehaviour
{
    public GameObject Player1Icon;
    public GameObject Player2Icon;
    public GameObject Player3Icon;
    public GameObject Player4Icon;
    public GameObject Player1Text;
    public GameObject Player2Text;
    public GameObject Player3Text;
    public GameObject Player4Text;
    public GameObject PressToJoin;

    string playerNumString;

    void Start()
    {
        Text Player1TXT = Player1Text.GetComponent<Text>();
        Text Player2TXT = Player2Text.GetComponent<Text>();
        Text Player3TXT = Player3Text.GetComponent<Text>();
        Text Player4TXT = Player4Text.GetComponent<Text>();
        Text PressToJoinTXT = PressToJoin.GetComponent<Text>();
    }

    void Update()
    {

    }

    void PlayerConnect(Text playerTxt, int numPlayer)
    {
        playerNumString = numPlayer.ToString();
        playerTxt.text = "Player " + playerNumString + " Joined";
    }
    void PlayerDisconnect(Text playerTxt, int numPlayer)
    {
        playerNumString = numPlayer.ToString();
        playerTxt.text = "Player " + playerNumString + " Disconnected";
    }
}
