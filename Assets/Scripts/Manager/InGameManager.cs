using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    [Header("Setters")]
    public ScoreHUD scoreScript;
    public PlayerGenerator playerGenerator;
    public RandomSpawnScript weaponGenerator;

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

    IEnumerator DelayHUD() ///When this is executed, the next round starts
    {
        yield return new WaitForSeconds(scoreDelay);
        //Debug.Log("PGenerator: End DelayHUD()");
        once = false;
        scoreScript.Desactivate();

        PlayerManager.Instance.DeleteProps = false;
        PlayerManager.Instance.WinRound = false;
        PlayerManager.Instance.RoundTimer = 0;

        playerGenerator.Spawn();
        weaponGenerator.RestartFilledArray();
    }

    public void ResetNBackToMenu()
    {
        PlayerManager.Instance.ResetNBackToMenu();
    }
}
