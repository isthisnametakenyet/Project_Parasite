using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class OptionsScript : MonoBehaviour
{
    public Slider Master;
    public Slider Music;
    public Slider SFX;

    void Start()
    {
        Music.value = SoundManager.instance.MusicVolume;
        SFX.value = SoundManager.instance.SFXVolume;
    }

    void Update()
    {
        SoundManager.instance.MusicVolume = Music.value;
        SoundManager.instance.SFXVolume = SFX.value;

        //PILLA VALOR DE SLIDER MUSIC Y SFX Y LOS MANDA PARA CAMBIAR EL VOLUMNE D LOS AUDIOS
        SoundManager.instance.RegulateAll(Music.value, SFX.value);
        //SoundManager.instance.RegulateType(Sound.AudioType.MUSIC, Music.value);
        //SoundManager.instance.RegulateType(Sound.AudioType.SFX, SFX.value);
    }
}
