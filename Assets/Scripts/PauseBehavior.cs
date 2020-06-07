using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBehavior : MonoBehaviour
{
    public GameObject PauseMenu;
    public Animator PauseMenuAnimator;

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
        PauseMenu.active = true;
        PlayerManager.Instance.Paused = true;
        PauseMenuAnimator.SetBool(ActiveMenuID, true);

        //EXTRA

    }

    public void DesactivatePause()
    {
        //ESENTIAL
        PlayerManager.Instance.Paused = false;
        PauseMenuAnimator.SetBool(ActiveMenuID, false);

        //EXTRA

    }
}
