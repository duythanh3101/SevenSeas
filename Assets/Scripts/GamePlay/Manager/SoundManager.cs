using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

     [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        [HideInInspector]
        public int simultaneousPlayCount = 0;
    }

    enum PlayingState
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

    [Header("Sounds")]
    public Sound background;
    public Sound shipExplosion;
    public Sound waterSplash;
    public Sound firingCanonball;
    public Sound winSound;
    public Sound loseSound;

    [SerializeField]
    private AudioSource bgmSource;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioSource specialSfxSource;

    private PlayingState musicState = PlayingState.Stopped;



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
}
