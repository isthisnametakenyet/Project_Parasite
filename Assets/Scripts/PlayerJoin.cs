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
    Color selectPurple;
    Color disconectPurple;
    Color selectYellow;
    Color disconectYellow;
    Color selectGreen;
    Color disconectGreen;

    string playerNumString;

    float animDelay;
    float maxDelay = 100;
    int colorAnim = 0;

    void Start()
    {
        //Hexadecimal Colors:
        //Red ON: FF0000
        //Red Off 8C0000
        //Purple ON: 7700E7
        //Purple Off: 4E0098
        //Yellow ON: FFFF00
        //Yellow Off: 828200
        //Green ON: 00FF00
        //Green Off: 006A00

        ColorUtility.TryParseHtmlString("#FF0000", out selectRed);
        ColorUtility.TryParseHtmlString("#8C0000", out disconectRed);
        ColorUtility.TryParseHtmlString("#7700E7", out selectPurple);
        ColorUtility.TryParseHtmlString("#4E0098", out disconectPurple);
        ColorUtility.TryParseHtmlString("#FFFF00", out selectYellow);
        ColorUtility.TryParseHtmlString("#828200", out disconectYellow);
        ColorUtility.TryParseHtmlString("#00FF00", out selectGreen);
        ColorUtility.TryParseHtmlString("#006A00", out disconectGreen);
    }

    void Update()
    {
        if (animDelay > 0)
        {
            animDelay--;
        }
        else {
            animDelay = maxDelay;
            colorAnim++;
            if (colorAnim == 5) { colorAnim = 1; }
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerDisconnect(Player1Icon, disconectRed, Player1Text, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerConnect(Player1Icon, selectRed, Player1Text, 1);
        }

        if (PlayerManager.Instance.Player1ON == false && PlayerManager.Instance.Player2ON == false && PlayerManager.Instance.Player3ON == false && PlayerManager.Instance.Player4ON == false)
        {
            PressAnim();
        }
        else { AllConnected(); }
    }

    void PlayerConnect(GameObject icon, Color colorChange, GameObject playerTxt, int numPlayer)
    {
        SpriteRenderer playerColor = icon.GetComponent<SpriteRenderer>();
        playerColor.color = colorChange;

        Text actualTxt = playerTxt.GetComponent<Text>();
        playerNumString = numPlayer.ToString();
        actualTxt.text = "Player " + playerNumString + " Joined";

        switch (numPlayer)
        {
            case 1:
                PlayerManager.Instance.Player1ON = true;
                break;
            case 2:
                PlayerManager.Instance.Player2ON = true;
                break;
            case 3:
                PlayerManager.Instance.Player3ON = true;
                break;
            case 4:
                PlayerManager.Instance.Player4ON = true;
                break;
        }
    }
    void PlayerDisconnect(GameObject icon, Color colorChange, GameObject playerTxt, int numPlayer)
    {
        SpriteRenderer playerColor = icon.GetComponent<SpriteRenderer>();
        playerColor.color = colorChange;

        Text actualTxt = playerTxt.GetComponent<Text>();
        playerNumString = numPlayer.ToString();
        actualTxt.text = "Player " + playerNumString + " Disconnected";

        switch (numPlayer)
        {
            case 1:
                PlayerManager.Instance.Player1ON = false;
                break;
            case 2:
                PlayerManager.Instance.Player2ON = false;
                break;
            case 3:
                PlayerManager.Instance.Player3ON = false;
                break;
            case 4:
                PlayerManager.Instance.Player4ON = false;
                break;
        }
    }
    void PressAnim()
    {
        Text actualTxt = PressToJoin.GetComponent<Text>();
        switch (colorAnim)
        {
            case 1:
                actualTxt.color = selectRed;
                break;
            case 2:
                actualTxt.color = selectPurple;
                break;
            case 3:
                actualTxt.color = selectYellow;
                break;
            case 4:
                actualTxt.color = selectGreen;
                break;
        }
        //actualTxt.color = ;

    }
    void AllConnected()
    {
        Text actualTxt = PressToJoin.GetComponent<Text>();
        actualTxt.text = "";
    }
}
