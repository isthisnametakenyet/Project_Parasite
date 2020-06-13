using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehavior : MonoBehaviour
{
    public GameObject PauseMenu;
    public Animator PauseMenuAnimator;

    //BOOLS
    bool finishedAnim = true;

    bool ScoreOn = false;

    //ANIAMTOR
    private int ActiveMenuID;

    void Start()
    {
        PauseMenu.active = false;
        PauseMenuAnimator = PauseMenu.GetComponent<Animator>();
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
        }

        //EXTRA

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
        }
        

        //EXTRA

    }

    public void ToggleScore()
    {
        if (ScoreOn)
        {

        }
        else
        {

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
}
