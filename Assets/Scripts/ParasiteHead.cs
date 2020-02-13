using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParasiteHead : MonoBehaviour
{
    public Skin skin = Skin.NONE;
    public Sprite NONE,ASD;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (skin)
        {
            case Skin.NONE:
                spriteRenderer.sprite = NONE;
                break;
            //case Skin.SKIN1:
            //    spriteRenderer.sprite = Skin1;
            //    break;
            //case Skin.SKIN2:
            //    spriteRenderer.sprite = Skin2;
            //    break;
        }
    }
}
