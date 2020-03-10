using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public PickTypes picktype;

    public GameObject RadomSpawner;
    private RandomSpawnScript RadomSpawnerscript;

    public GameObject Picker;
    public GameObject Sword;
    public GameObject Axe;
    public GameObject Spear;
    public GameObject Bow;
    public GameObject CrossBow;
    public GameObject Boomerang;
    private Sword swordScript;
    private Axe axeScript;
    private Spear spearScript;
    private Bow bowScript;
    private CrossBow crossbowScript;
    private Boomerang boomerangScript;

    private PlayerController2D playerAllScript;
    private EmptyBody playerEmptyScript;

    public Sprite swordSprite, axeSprite, lanceSprite, bowSprite, crossbowSprite, boomerangSprite;
    SpriteRenderer spriteRenderer;

    public bool picked;

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
        if(picked == true)
        {
            RadomSpawnerscript = RadomSpawner.GetComponent<RandomSpawnScript>();
            RadomSpawnerscript.numSpawned--;
            playerAllScript = Picker.GetComponent<PlayerController2D>();
            playerEmptyScript = Picker.GetComponent<EmptyBody>();

            switch (picktype) //INSTANTIATE WEAPON OBJECT
            {
                case PickTypes.Sword:
                    GameObject sword = Instantiate(Sword, new Vector3(transform.position.x, transform.position.y + 0f, 0), Quaternion.identity);
                    swordScript = sword.GetComponent<Sword>();

                    swordScript.Picker = Picker;
                    sword.transform.parent = Picker.transform; //set parent

                    if (Picker.gameObject.tag == "Player") { playerAllScript.PickedWeapon = sword; } //reference this weapon to player
                    else if (Picker.gameObject.tag == "EmptyBody") { playerEmptyScript.PickedWeapon = sword; }
                    Destroy(gameObject); //AUTODESTRUCCION
                    break;

                case PickTypes.Axe:
                    GameObject axe = Instantiate(Axe, new Vector3(transform.position.x, transform.position.y + 0f, 0), Quaternion.identity);
                    axeScript = axe.GetComponent<Axe>();
                    
                    axeScript.Picker = Picker;
                    axe.transform.parent = Picker.transform;

                    if (Picker.gameObject.tag == "Player") { playerAllScript.PickedWeapon = axe; }
                    else if (Picker.gameObject.tag == "EmptyBody") { playerEmptyScript.PickedWeapon = axe; }
                    Destroy(gameObject); //AUTODESTRUCCION
                    break;

                case PickTypes.Spear:
                    GameObject spear = Instantiate(Spear, new Vector3(transform.position.x, transform.position.y + 0f, 0), Quaternion.identity);
                    spearScript = spear.GetComponent<Spear>();

                    spearScript.Picker = Picker;
                    spear.transform.parent = Picker.transform;

                    if (Picker.gameObject.tag == "Player") { playerAllScript.PickedWeapon = spear; }
                    else if (Picker.gameObject.tag == "EmptyBody") { playerEmptyScript.PickedWeapon = spear; }
                    Destroy(gameObject); //AUTODESTRUCCION
                    break;

                case PickTypes.Bow:
                    GameObject bow = Instantiate(Bow, new Vector3(transform.position.x, transform.position.y + 0f, 0), Quaternion.identity);
                    bowScript = bow.GetComponent<Bow>();

                    bowScript.Picker = Picker;
                    bow.transform.parent = Picker.transform;

                    if (Picker.gameObject.tag == "Player") { playerAllScript.PickedWeapon = bow; }
                    else if (Picker.gameObject.tag == "EmptyBody") { playerEmptyScript.PickedWeapon = bow; }
                    Destroy(gameObject); //AUTODESTRUCCION
                    break;

                case PickTypes.CrossBow:
                    GameObject crossbow = Instantiate(CrossBow, new Vector3(transform.position.x, transform.position.y + 0f, 0), Quaternion.identity);
                    crossbowScript = crossbow.GetComponent<CrossBow>();

                    crossbowScript.Picker = Picker;
                    crossbow.transform.parent = Picker.transform;

                    if (Picker.gameObject.tag == "Player") { playerAllScript.PickedWeapon = crossbow; }
                    else if (Picker.gameObject.tag == "EmptyBody") { playerEmptyScript.PickedWeapon = crossbow; }
                    Destroy(gameObject); //AUTODESTRUCCION
                    break;

                case PickTypes.Boomerang:
                    GameObject boomerang = Instantiate(Boomerang, new Vector3(transform.position.x, transform.position.y + 0f, 0), Quaternion.identity);
                    boomerangScript = boomerang.GetComponent<Boomerang>();

                    boomerangScript.Picker = Picker;
                    boomerang.transform.parent = Picker.transform;

                    if (Picker.gameObject.tag == "Player") { playerAllScript.PickedWeapon = boomerang; }
                    else if (Picker.gameObject.tag == "EmptyBody") { playerEmptyScript.PickedWeapon = boomerang; }
                    Destroy(gameObject); //AUTODESTRUCCION
                    break;
            }
        }
    }
}