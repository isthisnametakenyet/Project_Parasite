using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParasiteHead : MonoBehaviour
{

    public Controller controller = Controller.NONE;
    public Skin skin = Skin.NONE;
    public Sprite NONE, Skin1, Skin2;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        switch (skin)
        {
            case Skin.NONE:
                spriteRenderer.sprite = NONE;
                break;
            case Skin.SKIN1:
                spriteRenderer.sprite = Skin1;
                break;
            case Skin.SKIN2:
                spriteRenderer.sprite = Skin2;
                break;
        }
    }
}
