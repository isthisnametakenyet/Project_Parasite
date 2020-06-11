using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    [HideInInspector] public PickTypes picktype;

    [HideInInspector] public GameObject RadomSpawner;
    private RandomSpawnScript RadomSpawnerscript;
    [HideInInspector] public int numFilled;

    private PlayerController2D playerAllScript;

    public Sprite swordSprite, axeSprite, lanceSprite, bowSprite, crossbowSprite, boomerangSprite;
    SpriteRenderer spriteRenderer;

    [HideInInspector] public bool picked;

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
            case PickTypes.Spear:
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

    void FixedUpdate()
    {
        if (picked == true)
        {
            RadomSpawnerscript = RadomSpawner.GetComponent<RandomSpawnScript>();
            RadomSpawnerscript.filledPoints[numFilled] = false;
            if (RadomSpawnerscript.allFilled == true) { RadomSpawnerscript.allFilled = false; RadomSpawnerscript.RecalculateNext(); }
            Destroy(transform.parent.gameObject); //AUTODESTRUCCION
        }
    }
}