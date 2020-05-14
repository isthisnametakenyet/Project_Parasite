using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonmanager : MonoBehaviour
{

    public Button red;
    public Button green;
    public Button yelow;
    public Button blue;

    public Sprite redsp;
    public Sprite greensp;
    public Sprite yelowsp;
    public Sprite bluesp;

    public void selected(int n) {

        switch (n) {
            case 1:
                SoundManager.instance.Play("ButtonSFX");
                red.image.sprite = redsp;
                break;
            case 2:
                SoundManager.instance.Play("ButtonSFX");
                green.image.sprite = greensp;

                break;
            case 3:
                SoundManager.instance.Play("ButtonSFX");
                yelow.image.sprite = yelowsp;

                break;
            case 4:
                SoundManager.instance.Play("ButtonSFX");
                blue.image.sprite = bluesp;

                break;

        }


    }




}
