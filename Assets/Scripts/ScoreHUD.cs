using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHUD : MonoBehaviour
{
    public float delayEnd;

    public Sprite HeadSkin1;
    public Sprite HeadSkin2;
    public Sprite HeadSkin3;

    public GameObject P1B;
    public GameObject P2B;
    public GameObject P3B;
    public GameObject P4B;
    public GameObject P1H;
    public GameObject P2H;
    public GameObject P3H;
    public GameObject P4H;

    //P1
    public GameObject P1_1;
    public GameObject P1_2;
    public GameObject P1_3;
    public GameObject P1_4;
    public GameObject P1_5;
    //P2
    public GameObject P2_1;
    public GameObject P2_2;
    public GameObject P2_3;
    public GameObject P2_4;
    public GameObject P2_5;
    //P3
    public GameObject P3_1;
    public GameObject P3_2;
    public GameObject P3_3;
    public GameObject P3_4;
    public GameObject P3_5;
    //P4
    public GameObject P4_1;
    public GameObject P4_2;
    public GameObject P4_3;
    public GameObject P4_4;
    public GameObject P4_5;

    private Animator animP1_1;
    private Animator animP1_2;
    private Animator animP1_3;
    private Animator animP1_4;
    private Animator animP1_5;
    private Animator animP2_1;
    private Animator animP2_2;
    private Animator animP2_3;
    private Animator animP2_4;
    private Animator animP2_5;
    private Animator animP3_1;
    private Animator animP3_2;
    private Animator animP3_3;
    private Animator animP3_4;
    private Animator animP3_5;
    private Animator animP4_1;
    private Animator animP4_2;
    private Animator animP4_3;
    private Animator animP4_4;
    private Animator animP4_5; 
    private int OldID;
    private int WinID;
    private int EndID;


    void Start()
    {
        animP1_1 = P1_1.GetComponent<Animator>();
        animP1_2 = P1_2.GetComponent<Animator>();
        animP1_3 = P1_3.GetComponent<Animator>();
        animP1_4 = P1_4.GetComponent<Animator>();
        animP1_5 = P1_5.GetComponent<Animator>();
        animP2_1 = P1_1.GetComponent<Animator>();
        animP2_2 = P1_2.GetComponent<Animator>();
        animP2_3 = P1_3.GetComponent<Animator>();
        animP2_4 = P1_4.GetComponent<Animator>();
        animP2_5 = P1_5.GetComponent<Animator>();
        animP3_1 = P1_1.GetComponent<Animator>();
        animP3_2 = P1_2.GetComponent<Animator>();
        animP3_3 = P1_3.GetComponent<Animator>();
        animP3_4 = P1_4.GetComponent<Animator>();
        animP3_5 = P1_5.GetComponent<Animator>();
        animP4_1 = P1_1.GetComponent<Animator>();
        animP4_2 = P1_2.GetComponent<Animator>();
        animP4_3 = P1_3.GetComponent<Animator>();
        animP4_4 = P1_4.GetComponent<Animator>();
        animP4_5 = P1_5.GetComponent<Animator>();
        OldID = Animator.StringToHash("Old");
        WinID = Animator.StringToHash("Win");
        EndID = Animator.StringToHash("End");

        Debug.Log("ScoreHUD ON");
        P1B.SetActive(false);
        P1H.SetActive(false);
        P2B.SetActive(false);
        P2H.SetActive(false);
        P3B.SetActive(false);
        P3H.SetActive(false);
        P4B.SetActive(false);
        P4H.SetActive(false);
        P1_1.SetActive(false);
        P1_2.SetActive(false);
        P1_3.SetActive(false);
        P1_4.SetActive(false);
        P1_5.SetActive(false);
        P2_1.SetActive(false);
        P2_2.SetActive(false);
        P2_3.SetActive(false);
        P2_4.SetActive(false);
        P2_5.SetActive(false);
        P3_1.SetActive(false);
        P3_2.SetActive(false);
        P3_3.SetActive(false);
        P3_4.SetActive(false);
        P3_5.SetActive(false);
        P4_1.SetActive(false);
        P4_2.SetActive(false);
        P4_3.SetActive(false);
        P4_4.SetActive(false);
        P4_5.SetActive(false);
    }

    public void Activate()
    {
        Debug.Log("Hiding ScoreHUD");
        P1B.SetActive(true);
        P1H.SetActive(true);
        switch (PlayerManager.Instance.ScorePlayer1)
        {
            case 1:
                P1_1.SetActive(true);
                break;
            case 2:
                P1_1.SetActive(true);
                P1_2.SetActive(true);
                break;
            case 3:
                P1_1.SetActive(true);
                P1_2.SetActive(true);
                P1_3.SetActive(true);
                break;
            case 4:
                P1_1.SetActive(true);
                P1_2.SetActive(true);
                P1_3.SetActive(true);
                P1_4.SetActive(true);
                break;
            case 5:
                P1_1.SetActive(true);
                P1_2.SetActive(true);
                P1_3.SetActive(true);
                P1_4.SetActive(true);
                P1_5.SetActive(true);
                break;
            default:
                break;
        }
        if (PlayerManager.Instance.Player2ON == false) { return; }
        P2B.SetActive(true);
        P2H.SetActive(true);
        switch (PlayerManager.Instance.ScorePlayer2)
        {
            case 1:
                P2_1.SetActive(true);
                break;
            case 2:
                P2_1.SetActive(true);
                P2_2.SetActive(true);
                break;
            case 3:
                P2_1.SetActive(true);
                P2_2.SetActive(true);
                P2_3.SetActive(true);
                break;
            case 4:
                P2_1.SetActive(true);
                P2_2.SetActive(true);
                P2_3.SetActive(true);
                P2_4.SetActive(true);
                break;
            case 5:
                P2_1.SetActive(true);
                P2_2.SetActive(true);
                P2_3.SetActive(true);
                P2_4.SetActive(true);
                P2_5.SetActive(true);
                break;
            default:
                break;
        }
        if (PlayerManager.Instance.Player3ON == false) { return; }
        P3B.SetActive(true);
        P3H.SetActive(true);
        switch (PlayerManager.Instance.ScorePlayer3)
        {
            case 1:
                P3_1.SetActive(true);
                break;
            case 2:
                P3_1.SetActive(true);
                P3_2.SetActive(true);
                break;
            case 3:
                P3_1.SetActive(true);
                P3_2.SetActive(true);
                P3_3.SetActive(true);
                break;
            case 4:
                P3_1.SetActive(true);
                P3_2.SetActive(true);
                P3_3.SetActive(true);
                P3_4.SetActive(true);
                break;
            case 5:
                P3_1.SetActive(true);
                P3_2.SetActive(true);
                P3_3.SetActive(true);
                P3_4.SetActive(true);
                P3_5.SetActive(true);
                break;
            default:
                break;
        }
        if (PlayerManager.Instance.Player4ON == false) { return; }
        P4B.SetActive(true);
        P4H.SetActive(true);
        switch (PlayerManager.Instance.ScorePlayer4)
        {
            case 1:
                P4_1.SetActive(true);
                break;
            case 2:
                P4_1.SetActive(true);
                P4_2.SetActive(true);
                break;
            case 3:
                P4_1.SetActive(true);
                P4_2.SetActive(true);
                P4_3.SetActive(true);
                break;
            case 4:
                P4_1.SetActive(true);
                P4_2.SetActive(true);
                P4_3.SetActive(true);
                P4_4.SetActive(true);
                break;
            case 5:
                P4_1.SetActive(true);
                P4_2.SetActive(true);
                P4_3.SetActive(true);
                P4_4.SetActive(true);
                P4_5.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Desactivate()
    {
        Debug.Log("Revealing ScoreHUD");
        P1B.SetActive(false);
        P1H.SetActive(false);
        P1_1.SetActive(false);
        P1_2.SetActive(false);
        P1_3.SetActive(false);
        P1_4.SetActive(false);
        P1_5.SetActive(false);
        if (PlayerManager.Instance.Player2ON == false) { return; }
        P2B.SetActive(false);
        P2H.SetActive(false);
        P2_1.SetActive(false);
        P2_2.SetActive(false);
        P2_3.SetActive(false);
        P2_4.SetActive(false);
        P2_5.SetActive(false);
        if (PlayerManager.Instance.Player3ON == false) { return; }
        P3B.SetActive(false);
        P3H.SetActive(false);
        P3_1.SetActive(false);
        P3_2.SetActive(false);
        P3_3.SetActive(false);
        P3_4.SetActive(false);
        P3_5.SetActive(false);
        if (PlayerManager.Instance.Player4ON == false) { return; }
        P4B.SetActive(false);
        P4H.SetActive(false);
        P4_1.SetActive(false);
        P4_2.SetActive(false);
        P4_3.SetActive(false);
        P4_4.SetActive(false);
        P4_5.SetActive(false);
    }

    public void Round(int winner)
    {
        switch (winner)
        {
            case 0:
                Debug.LogError("Round Winner not set in PlayerManager");
                break;
            case 1:
                switch (PlayerManager.Instance.ScorePlayer1)
                {
                    case 1:
                        animP1_1.SetBool(WinID, true);
                        break;
                    case 2:
                        animP1_2.SetBool(WinID, true);
                        break;
                    case 3:
                        animP1_3.SetBool(WinID, true);
                        break;
                    case 4:
                        animP1_4.SetBool(WinID, true);
                        break;
                    case 5:
                        animP1_5.SetBool(WinID, true);
                        break;
                }
                break;
            case 2:
                switch (PlayerManager.Instance.ScorePlayer2)
                {
                    case 1:
                        animP2_1.SetBool(WinID, true);
                        break;
                    case 2:
                        animP2_2.SetBool(WinID, true);
                        break;
                    case 3:
                        animP2_3.SetBool(WinID, true);
                        break;
                    case 4:
                        animP2_4.SetBool(WinID, true);
                        break;
                    case 5:
                        animP2_5.SetBool(WinID, true);
                        break;
                }
                break;
            case 3:
                switch (PlayerManager.Instance.ScorePlayer3)
                {
                    case 1:
                        animP3_1.SetBool(WinID, true);
                        break;
                    case 2:
                        animP3_2.SetBool(WinID, true);
                        break;
                    case 3:
                        animP3_3.SetBool(WinID, true);
                        break;
                    case 4:
                        animP3_4.SetBool(WinID, true);
                        break;
                    case 5:
                        animP3_5.SetBool(WinID, true);
                        break;
                }
                break;
            case 4:
                switch (PlayerManager.Instance.ScorePlayer4)
                {
                    case 1:
                        animP4_1.SetBool(WinID, true);
                        break;
                    case 2:
                        animP4_2.SetBool(WinID, true);
                        break;
                    case 3:
                        animP4_3.SetBool(WinID, true);
                        break;
                    case 4:
                        animP4_4.SetBool(WinID, true);
                        break;
                    case 5:
                        animP4_5.SetBool(WinID, true);
                        break;
                }
                break;
            default:
                Debug.LogError("Round Winner received number not from 0 to 4");
                break;
        }
        PlayerManager.Instance.Round = 0;
    }

    public void End(int winner)
    {
        switch (winner)
        {
            case 1:
                animP1_5.SetBool(EndID, true);
                StartCoroutine("EndGame");
                break; 
            case 2:
                animP2_5.SetBool(EndID, true);
                StartCoroutine("EndGame");
                break;
            case 3:
                animP3_5.SetBool(EndID, true);
                StartCoroutine("EndGame");
                break;
            case 4:
                animP4_5.SetBool(EndID, true);
                StartCoroutine("EndGame");
                break;
        }
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(delayEnd);
        //CARGAR MAIN MENU
    }
}
