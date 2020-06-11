using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    [Header("Setters")]
    public ScoreHUD scoreScript;
    public PlayerGenerator playerGenerator;

    [Header("Delay")]
    public float scoreDelay;

    private bool once = false;

    void FixedUpdate()
    {
        //END ROUND
        if (PlayerManager.Instance.WinRound == true && once == false)
        {
            once = true;
            scoreScript.Activate();
            scoreScript.Round();
            if (PlayerManager.Instance.WinGame == false) { StartCoroutine("DelayHUD"); /*Debug.Log("PGenerator: Start DelayHUD()");*/ }
        }

        //END GAME
        if (PlayerManager.Instance.WinGame == true)
        {
            scoreScript.End();
        }
    }

    IEnumerator DelayHUD()
    {
        yield return new WaitForSeconds(scoreDelay);
        //Debug.Log("PGenerator: End DelayHUD()");
        once = false;
        scoreScript.Desactivate();
        PlayerManager.Instance.DeleteProps = false;
        PlayerManager.Instance.WinRound = false;
        playerGenerator.Spawn();
    }

    public void ResetNBackToMenu()
    {
        PlayerManager.Instance.gameON = false;
        PlayerManager.Instance.WinRound = false;
        PlayerManager.Instance.WinGame = false;

        PlayerManager.Instance.isAlivePlayer1 = false;
        PlayerManager.Instance.isAlivePlayer2 = false;
        PlayerManager.Instance.isAlivePlayer3 = false;
        PlayerManager.Instance.isAlivePlayer4 = false;

        PlayerManager.Instance.Paused = false;
    }
}
