using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public float MusicVolume = 0.2f;
    public float SFXVolume = 0.2f;
    public float PitchGlobal = 1f;

    public Sound[] sounds;
    public static SoundManager instance;

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

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = PitchGlobal;
            s.source.loop = s.loop;
        }

        RegulateAll(MusicVolume, SFXVolume);
    }
    void Start()
    {
        //Play("Shot1");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        Debug.Log("Sound Playing: " + name);
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        Debug.Log("Sound Stopped: " + name);
        s.source.Stop();
    }
    public void StopAll()
    {
        Debug.Log("Stop All Audios");
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source.Stop();
        }
    }
    //public void Regulate(string name, float volume)
    //{
    //    Sound s = Array.Find(sounds, sound => sound.name == name);
    //    if (s == null)
    //        return;
    //    s.source.volume = volume;
    //}
    //public float GetVolume(string name)
    //{
    //    Sound s = Array.Find(sounds, sound => sound.name == name);
    //    if (s == null)
    //        return 0;

    //    return s.source.volume;
    //}
    //public void RegulateType(Sound.AudioType type, float volume)
    //{
    //    Debug.Log("Regulate Type Audios");
    //    for (int i = 0; i < sounds.Length; i++)
    //    {
    //        if (sounds[i].audioType == type)
    //        {
    //            sounds[i].volume = volume;
    //        }
    //    }
    //}
    public void RegulateAll(float Musicvolume, float SFXvolume)
    {
        Debug.Log("Regulate All Audios");
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].audioType == Sound.AudioType.MUSIC)
            {
                sounds[i].volume = Musicvolume;
                sounds[i].source.volume = Musicvolume;
            }
            else if (sounds[i].audioType == Sound.AudioType.SFX)
            {
                sounds[i].volume = SFXvolume;
                sounds[i].source.volume = SFXvolume;
            }
        }
    }
}