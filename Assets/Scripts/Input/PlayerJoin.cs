using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerJoin : MonoBehaviour
{
    private MenuAsignemet assignementScript;

    public GameObject Player1;
    private AsignerController Player1Asigner;
    public GameObject Player2;
    private AsignerController Player2Asigner;
    public GameObject Player3;
    private AsignerController Player3Asigner;
    public GameObject Player4;
    private AsignerController Player4Asigner;
    public GameObject Player1Text;
    public GameObject Player2Text;
    public GameObject Player3Text;
    public GameObject Player4Text;

    public GameObject Player1SkinSelector;
    public GameObject Player2SkinSelector;
    public GameObject Player3SkinSelector;
    public GameObject Player4SkinSelector;

    public GameObject PressToJoin;
    public GameObject TutorialButton;
    public GameObject PlayButton;

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

        assignementScript = this.gameObject.GetComponent<MenuAsignemet>();
        Player1Asigner = Player1.GetComponent<AsignerController>();
        Player2Asigner = Player2.GetComponent<AsignerController>();
        Player3Asigner = Player3.GetComponent<AsignerController>();
        Player4Asigner = Player4.GetComponent<AsignerController>();

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
        if (Player1Asigner.asignerReady == true) { PlayerConnect(Player1, selectRed, Player1Text, 1);
            Player1SkinSelector.transform.gameObject.SetActive(true);
        }
        else { PlayerDisconnect(Player1, disconectRed, Player1Text, 1);
            Player1SkinSelector.transform.gameObject.SetActive(false);
        }
        if (Player2Asigner.asignerReady == true) { PlayerConnect(Player2, selectPurple, Player2Text, 2); }
        else { PlayerDisconnect(Player2, disconectPurple, Player2Text, 2); }
        if (Player3Asigner.asignerReady == true) { PlayerConnect(Player3, selectYellow, Player3Text, 3); }
        else { PlayerDisconnect(Player3, disconectYellow, Player3Text, 3); }
        if (Player4Asigner.asignerReady == true) { PlayerConnect(Player4, selectGreen, Player4Text, 4); }
        else { PlayerDisconnect(Player4, disconectGreen, Player4Text, 4); }

        if (PlayerManager.Instance.Player1ON == false &&
            PlayerManager.Instance.Player2ON == false &&
            PlayerManager.Instance.Player3ON == false &&
            PlayerManager.Instance.Player4ON == false)
        {
            PressAnim();
        }
        else { AllConnected(); }
    }

    //Player Join Functions
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

    private void AssignController()
    {
        // Check all joysticks for a button press and assign it tp
        // the first Player foudn without a joystick
        IList<Joystick> joysticks = ReInput.controllers.Joysticks;
        for (int i = 0; i < joysticks.Count; i++)
        {
            Debug.Log(joysticks[i]);
            Joystick joystick = joysticks[i];
            if (ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id)) continue; // joystick is already assigned to a Player

            // Chec if a button was pressed on the joystick
            if (joystick.GetAnyButtonDown())
            {

                // Find the next Player without a Joystick
                Player player = FindPlayerWithoutJoystick();
                if (player == null) return; // no free joysticks

                // Assign the joystick to this Player
                player.controllers.AddController(joystick, false);
            }
        }

        // If all players have joysticks, enable joystick auto-assignment
        // so controllers are re-assigned correctly when a joystick is disconnected
        // and re-connected and disable this script
        if (DoAllPlayersHaveJoysticks())
        {
            ReInput.configuration.autoAssignJoysticks = true;
            this.enabled = false; // disable this script
        }

    }

    // Searches all Players to find the next Player without a Joystick assigned
    private Player FindPlayerWithoutJoystick()
    {
        IList<Player> players = ReInput.players.Players;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].controllers.joystickCount > 0) continue;
            return players[i];
        }
        return null;
    }

    private bool DoAllPlayersHaveJoysticks()
    {
        return FindPlayerWithoutJoystick() == null;
    }

    //PressToJoin Animation
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
    }
    void AllConnected()
    {
        Text actualTxt = PressToJoin.GetComponent<Text>();
        actualTxt.text = "";
        TutorialButton.transform.gameObject.SetActive(true);
        PlayButton.transform.gameObject.SetActive(true);
    }
}
