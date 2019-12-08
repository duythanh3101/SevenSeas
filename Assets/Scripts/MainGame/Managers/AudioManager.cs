using BaseSystems.Observer;
using BaseSystems.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource audioWinGame;
    [SerializeField] private AudioClip audioWinGameClip;

    [SerializeField] private AudioSource audioBackGround;
    [SerializeField] private AudioClip audioBackGroundClip;

    // Start is called before the first frame update
    void Start()
    {
        audioWinGame = GetComponent<AudioSource>();
        this.RegisterListener(ObserverEventID.OnFindTreasureGameOver, (param) => OnFindTreasureGameOver());
        this.RegisterListener(ObserverEventID.OnCheckPointMapStarted, (param) => OnCheckPointMapStarted());
    }

    private void OnCheckPointMapStarted()
    {
        audioWinGame.clip = audioWinGameClip;
        audioWinGame.Play();
    }

    private void OnFindTreasureGameOver()
    {
        audioWinGame.clip = audioWinGameClip;
        audioWinGame.PlayOneShot(audioWinGameClip, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
