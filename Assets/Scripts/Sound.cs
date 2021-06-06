using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public AudioClip BGM;
    public AudioClip move;
    public AudioClip error;
    public AudioClip drop;
    public AudioClip clear;
    public AudioClip lose;
    public AudioSource BGMsource;
    public AudioSource SFXsource1;
    public AudioSource clearSource;

    private void Awake()
    {
        BGMsource.clip = BGM;
        BGMsource.loop = true;
        clearSource.clip = clear;
    }

    public void PlayBGM()
    {
        BGMsource.Play();
    }

    public void PlayMove()
    {
        SFXsource1.clip = move;
        SFXsource1.Play();
    }

    public void PlayDrop()
    {
        SFXsource1.clip = drop;
        SFXsource1.Play();
    }

    public void PlayError()
    {
        SFXsource1.clip = error;
        SFXsource1.Play();
    }

    public void PlayClear()
    {
        clearSource.Play();
    }

    public void PlayLose()
    {
        BGMsource.clip = lose;
        BGMsource.loop = false;
        BGMsource.Play();
    }

    public void Mute()
    {
        BGMsource.mute = true;
        SFXsource1.mute = true;
        clearSource.mute = true;
    }

    public void Unmute()
    {
        BGMsource.mute = false;
        SFXsource1.mute = false;
        clearSource.mute = false;
    }
}
