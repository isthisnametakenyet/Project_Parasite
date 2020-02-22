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

    Color selectRed;
    Color disconectRed;

    string playerNumString;

    void Start()
    {
        //selectRed = new Color(200, 200, 200, 255);
        ColorUtility.TryParseHtmlString("#FF0000", out selectRed);
        ColorUtility.TryParseHtmlString("#8C0000", out disconectRed); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerDisconnect(Player1Icon, disconectRed, Player1Text, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerConnect(Player1Icon, selectRed, Player1Text, 1);
        }
    }

    void PlayerConnect(GameObject icon, Color colorChange, GameObject playerTxt, int numPlayer)
    {
        SpriteRenderer playerColor = icon.GetComponent<SpriteRenderer>();
        playerColor.color = colorChange;

        Text actualTxt = playerTxt.GetComponent<Text>();
        playerNumString = numPlayer.ToString();
        actualTxt.text = "Player " + playerNumString + " Joined";
    }
    void PlayerDisconnect(GameObject icon, Color colorChange, GameObject playerTxt, int numPlayer)
    {
        SpriteRenderer playerColor = icon.GetComponent<SpriteRenderer>();
        playerColor.color = colorChange;

        Text actualTxt = playerTxt.GetComponent<Text>();
        playerNumString = numPlayer.ToString();
        actualTxt.text = "Player " + playerNumString + " Disconnected";
    }
    void PressAnim()
    {

    }
    void AllConnected(GameObject PressToJoin)
    {
        Text actualTxt = PressToJoin.GetComponent<Text>();
        actualTxt.text = "";
    }
}
