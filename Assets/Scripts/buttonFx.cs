using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class buttonFx : MonoBehaviour
{

    public void Sound() {

        SoundManager.instance.Play("ButtonSFX");

    }

    TextMeshProUGUI textChild;

    public void SelectChild(TextMeshProUGUI textChild2)
    {
        textChild = textChild2;
    }

    public void ChangeMTPColor(string SelectedColor)
    {
        Color selectColor;
        ColorUtility.TryParseHtmlString(SelectedColor, out selectColor);
        textChild.color = selectColor;
        textChild = null;
    }
}
