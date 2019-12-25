﻿using System.Collections;
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


    void Awake()
    {
        closeButton.onClick.AddListener(() => OnCloseButtonClick());
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
        Display(false);
    }
}