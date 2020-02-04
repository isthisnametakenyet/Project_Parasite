using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public PickTypes picktype;
    
    public Sprite swordSprite, axeSprite, lanceSprite, bowSprite, crossbowSprite, boomerangSprite;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (picktype)
        {
            case PickTypes.Sword:
                spriteRenderer.sprite = swordSprite;
                break;
            case PickTypes.Axe:
                spriteRenderer.sprite = axeSprite;
                break;
            case PickTypes.Lance:
                spriteRenderer.sprite = lanceSprite;
                break;
            case PickTypes.Bow:
                spriteRenderer.sprite = bowSprite;
                break;
            case PickTypes.CrossBow:
                spriteRenderer.sprite = crossbowSprite;
                break;
            case PickTypes.Boomerang:
                spriteRenderer.sprite = boomerangSprite;
                break;
        }
    }
}