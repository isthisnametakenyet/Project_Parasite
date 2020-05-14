using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

        public void ChangeScene(string name)
    {
        switch (name)
        {
            case "MainMenu":
                SoundManager.instance.Stop("sawIdle");
                SoundManager.instance.Stop("GameMusic");
                SceneManager.LoadScene(name);
                break;
            case "Cave":
                SoundManager.instance.Play("sawIdle");
                SoundManager.instance.Stop("MenuMusic");
                SoundManager.instance.Play("GameMusic");
                SceneManager.LoadScene(name);
                break;
            default:
                SceneManager.LoadScene(name);
                break;
        }
        
    }

    public void Quit()
    {
        Application.Quit();
    }
}
