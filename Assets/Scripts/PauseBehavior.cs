using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehavior : MonoBehaviour
{
    //SETTERS
    public GameObject PauseMenu;
    public Animator PauseMenuAnimator;
    private InGameManager inGameManager;
    public GameObject Controls;

    //BOOLS
    bool finishedAnim = true;
    bool ControlsON = false;

    //ANIAMTOR
    private int ActiveMenuID;

    void Start()
    {
        PauseMenu.active = false;

        PauseMenuAnimator = PauseMenu.GetComponent<Animator>();
        inGameManager = GetComponent<InGameManager>();

        ActiveMenuID = Animator.StringToHash("Active");
    }

    public void ActivatePause()
    {
        //ESENTIAL
        if (finishedAnim == true)
        {
            PauseMenu.active = true;
            PlayerManager.Instance.Paused = true;
            PauseMenuAnimator.SetBool(ActiveMenuID, true);
            finishedAnim = false;
            StartCoroutine("AnimFinished");

            //EXTRA

        }
    }

    public void DesactivatePause()
    {
        //ESENTIAL
        if (finishedAnim == true)
        {
            PauseMenuAnimator.SetBool(ActiveMenuID, false);
            StartCoroutine("MinimizeMenu");
            StartCoroutine("AnimFinished");
            finishedAnim = false;


            //EXTRA
            inGameManager.ScoreOFF();
        }
    }



    IEnumerator MinimizeMenu()
    {
        yield return new WaitForSeconds(1);
        PlayerManager.Instance.Paused = false;
        PauseMenu.active = false;
    }

    IEnumerator AnimFinished()
    {
        yield return new WaitForSeconds(1);
        finishedAnim = true;
    }

    public void ToggleControls()
    {
        if (ControlsON)
        {
            Controls.SetActive(false);
            ControlsON = false;
        }
        else {
            Controls.SetActive(true);
            ControlsON = true;
        }
    }
}
