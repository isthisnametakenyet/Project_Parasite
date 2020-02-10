using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewiredManager : MonoBehaviour
{
    public static RewiredManager Instance { get; private set; }
    public static GameObject Rewired { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("WARNING: multiple " + this + " in scene");
        }
    }
}
