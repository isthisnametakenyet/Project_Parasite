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
        if (PlayerManager.Instance.Player2ON == false) { return; }
        P2B.SetActive(true);
        P2H.SetActive(true);
        if (PlayerManager.Instance.Player3ON == false) { return; }
        P3B.SetActive(true);
        P3H.SetActive(true);
        if (PlayerManager.Instance.Player4ON == false) { return; }
        P4B.SetActive(true);
        P4H.SetActive(true);
    }

    public void Desactivate()
    {
        Debug.Log("Revealing ScoreHUD");
        P1B.SetActive(false);
        P1H.SetActive(false);
        if (PlayerManager.Instance.Player2ON == false) { return; }
        P2B.SetActive(false);
        P2H.SetActive(false);
        if (PlayerManager.Instance.Player3ON == false) { return; }
        P3B.SetActive(false);
        P3H.SetActive(false);
        if (PlayerManager.Instance.Player4ON == false) { return; }
        P4B.SetActive(false);
        P4H.SetActive(false);
    }

    public void Round(int winner)
    {
        switch (winner)
        {
            case 0:
                Debug.LogError("Round Winner not set in PlayerManager");
                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            default:
                Debug.LogError("Round Winner received number not from 0 to 4");
                break;
        }
        PlayerManager.Instance.Round = 0;
    }

    public void Win(int winner)
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
