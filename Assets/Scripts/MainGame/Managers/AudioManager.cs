using BaseSystems.Observer;
using BaseSystems.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSourceBackground;

    [SerializeField] private AudioClip audioWinGameClip;
    [SerializeField] private AudioClip audioBackGroundClip;
    [SerializeField] private AudioClip audioClickedFindTreasureClip;
    [SerializeField] private AudioClip audioFindTreasureClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        this.RegisterListener(ObserverEventID.OnFindTreasureGameOver, (param) => OnFindTreasureGameOver());
        this.RegisterListener(ObserverEventID.OnCheckPointMapStarted, (param) => OnCheckPointMapStarted());
        this.RegisterListener(ObserverEventID.OnClickedTreasureMap, (param) => OnClickedTreasureMap());
        this.RegisterListener(ObserverEventID.OnFindTreasureGameStarted, (param) => OnFindTreasureGameStarted());
    }

    private void OnFindTreasureGameStarted()
    {
        audioSourceBackground.clip = audioBackGroundClip;
        audioSourceBackground.Play();
    }

    private void OnClickedTreasureMap()
    {
        audioSource.clip = audioClickedFindTreasureClip;
        audioSource.PlayOneShot(audioSource.clip, 1f);
    }

    public void OnCheckPointMapStarted()
    {
        StartCoroutine("PlayAudioBackground", audioSource.isPlaying);
    }

    private IEnumerator PlayAudioBackground()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        audioSource.clip = audioBackGroundClip;
        audioSource.Play();
        yield return null;
    }

    private void OnFindTreasureGameOver()
    {
        audioSource.clip = audioWinGameClip;
        audioSource.PlayOneShot(audioWinGameClip, 1f);
    }
}
