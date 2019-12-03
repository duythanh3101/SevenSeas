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
    
    // Start is called before the first frame update
    void Start()
    {
        audioWinGame = GetComponent<AudioSource>();
        this.RegisterListener(ObserverEventID.OnFindTreasureGameOver, (param) => OnFindTreasureGameOnver());
    }

    private void OnFindTreasureGameOnver()
    {
        audioWinGame.clip = audioWinGameClip;
        audioWinGame.PlayOneShot(audioWinGameClip, 1f);
        Debug.Log("hahahahahaha");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
