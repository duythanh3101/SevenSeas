using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUIController : MonoBehaviour
{
    public System.Action OnCloseButtonClick = delegate { };
    [SerializeField]
    private Slider soundFxSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button helpButton;

    private void Start()
    {
        //Load music volumn and sfx from sound manager
        soundFxSlider.value = SoundManager.Instance.sfxSource.volume;
        musicSlider.value = SoundManager.Instance.bgmSource.volume;
    }

    void Awake()
    {
        closeButton.onClick.AddListener(() => OnCloseButtonClick());
        musicSlider.onValueChanged.AddListener((val) => OnMusicChangedValue(val));
        soundFxSlider.onValueChanged.AddListener((val) => OnSFXChangedValue(val));
    }

    private void OnMusicChangedValue(float val)
    {
        SoundManager.Instance.setMusicVolume(val);
    }

    private void OnSFXChangedValue (float val)
    {
        SoundManager.Instance.setSFXVolume(val);
    }

    void Display(bool isShowing)
    {
        canvasGroup.alpha = isShowing ? 1 : 0;
        canvasGroup.blocksRaycasts = isShowing;
        canvasGroup.interactable = isShowing;
    }

    

    public void Show()
    {
        Display(true);
    }

    public void Hide()
    {
        //Save music and sfx
        SoundManager.Instance.SaveMusicVolumn(musicSlider.value);
        SoundManager.Instance.SaveSFXVolumn(soundFxSlider.value);
        Display(false);
    }
}
