﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    [HideInInspector]
    public int simultaneousPlayCount = 0;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

     

    public enum PlayingState
    {
        Playing,
        Paused,
        Stopped
    }

    //public delegate void MusicStatusChangedHandler(bool isOn);
    //public static event MusicStatusChangedHandler MusicStatusChanged;

    //public delegate void SoundStatusChangedHandler(bool isOn);
    //public static event SoundStatusChangedHandler SoundStatusChanged;

    [Header("Max number allowed of same sounds playing together")]
    [SerializeField]
    private int maxSimultaneousSounds = 7;

    [Header("Background Music")]
    public Sound background;

    [Header("Gameplay Sound Effect")]
    public Sound shipExplosion;
    public Sound waterSplash;
    public Sound firingCanonball;
    public Sound winSound;
    public Sound loseSound;

    public Sound respawnSound;
    public Sound bonusSound;
    public Sound playerMoveSound;
    public Sound enemyMoveSound;
    public Sound sinkSound;
    public Sound riseUpSound;
    
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource specialSfxSource;

    public PlayingState MusicState { get => musicState;}
    

    public Sound buttonClick;

    [SerializeField]
    private PlayingState musicState = PlayingState.Stopped;

    private static readonly string MUSIC_VOLUMN_KEY = "MUSIC_VOLUMN";
    private static readonly string SFX_VOLUMN_KEY = "SFX_VOLUMN";

    public float MusicVolumn
    {
        get => PlayerPrefs.GetFloat(MUSIC_VOLUMN_KEY, 100);
        private set
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUMN_KEY, value);
            PlayerPrefs.Save();
        }
    }

    public float SFXVolumn
    {
        get => PlayerPrefs.GetFloat(SFX_VOLUMN_KEY, 100);
        private set
        {
            PlayerPrefs.SetFloat(SFX_VOLUMN_KEY, value);
            PlayerPrefs.Save();
        }
    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        else if (Instance != this)
            DestroyImmediate(gameObject);
    }

    /// <summary>
    /// Plays the given sound with option to progressively scale down volume of multiple copies of same sound playing at
    /// the same time to eliminate the issue that sound amplitude adds up and becomes too loud.
    /// </summary>
    /// <param name="sound">Sound.</param>
    /// <param name="isSpecialSound">Set to true if using ducking effect (additional setup needed)</param>
    /// <param name="autoScaleVolume">If set to <c>true</c> auto scale down volume of same sounds played together.</param>
    /// <param name="maxVolumeScale">Max volume scale before scaling down.</param>
    public void PlaySound(Sound sound, bool isSpecialSound = false, bool autoScaleVolume = true, float maxVolumeScale = 1f)
    {
        StartCoroutine(CRPlaySound(sound, isSpecialSound, autoScaleVolume, maxVolumeScale));
    }

    IEnumerator CRPlaySound(Sound sound, bool isSpecialSound = false, bool autoScaleVolume = true, float maxVolumeScale = 1f)
    {
        if (sound.simultaneousPlayCount >= maxSimultaneousSounds)
        {
            yield break;
        }

        sound.simultaneousPlayCount++;

        float vol = maxVolumeScale;

        // Scale down volume of same sound played subsequently
        if (autoScaleVolume && sound.simultaneousPlayCount > 0)
        {
            vol = vol / (float)(sound.simultaneousPlayCount);
        }

        AudioSource src = null;
        if (isSpecialSound)
            src = specialSfxSource;
        if (src == null)
            src = sfxSource;

        src.PlayOneShot(sound.clip, vol);

        // Wait til the sound almost finishes playing then reduce play count
        float delay = sound.clip.length * 0.7f;
        //Debug.Log(delay);


        yield return new WaitForSeconds(delay);

        sound.simultaneousPlayCount--;
    }

    /// <summary>
    /// Plays the given music.
    /// </summary>
    /// <param name="music">Music.</param>
    /// <param name="loop">If set to <c>true</c> loop.</param>
    public void PlayMusic(Sound music, bool loop = true)
    {
       
        bgmSource.clip = music.clip;
        bgmSource.loop = loop;
        bgmSource.Play();
        musicState = PlayingState.Playing;
    }

    /// <summary>
    /// Pauses the music.
    /// </summary>
    public void PauseMusic()
    {
        if (musicState == PlayingState.Playing)
        {
            bgmSource.Pause();
            musicState = PlayingState.Paused;
        }
    }

    /// <summary>
    /// Resumes the music.
    /// </summary>
    public void ResumeMusic()
    {
        if (musicState == PlayingState.Paused)
        {
            bgmSource.UnPause();
            musicState = PlayingState.Playing;
        }
    }

    /// <summary>
    /// Stop music.
    /// </summary>
    public void StopMusic()
    {
        bgmSource.Stop();
        musicState = PlayingState.Stopped;
    }

    public void PlayDestroyShipSound()
    {
        PlaySound(shipExplosion);
    }

    public void PlayWaterSlashSound()
    {
        PlaySound(waterSplash);
    }

    public void PlayFiringSound()
    {
        PlaySound(firingCanonball);
    }

    public void PlayLoseSound()
    {
        PlaySound(loseSound);
    }

    public void PlayWinSound()
    {
        PlaySound(winSound);
    }


    public void PlayRespawnSound()
    {
        PlaySound(respawnSound);
    }

    public void PlayBonusSound()
    {
 	  PlaySound(bonusSound);
    }

    public void PlayRiseUpSound()
    {
        PlaySound(riseUpSound);
    }

    public void PlaySinkSound()
    {
        PlaySound(sinkSound);
    }

    public void PlayPlayerMovementSound()
    {
        PlaySound(playerMoveSound);
    }

    public void PlayEnemyMovementSound()
    {
        PlaySound(enemyMoveSound);
    }

   

    public void setMusicVolume(float vol)
    {
        bgmSource.volume = vol;
    }

    public void setSFXVolume(float vol)
    {
        sfxSource.volume = vol;
    }

    public void ClickButton()
    {
        PlaySound(buttonClick);
    } 

    public void SaveMusicVolumn(float value)
    {
        MusicVolumn = value;
    }

    public void SaveSFXVolumn(float value)
    {
        SFXVolumn = value;
    }

}
