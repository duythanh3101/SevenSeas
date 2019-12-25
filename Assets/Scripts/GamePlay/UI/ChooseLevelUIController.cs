using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ChooseLevelUIController : MonoBehaviour
{

    public Action OnStartNewGameButtonClick = delegate { };
    public Action OnQuitToWindowButtonClick = delegate { };
    public Action OnEasyButtonClick = delegate { };
    public Action OnAverageButtonClick = delegate { };
    public Action OnHardButtonClick = delegate { };
    

    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Button startNewGameButton;
    [SerializeField]
    private Button quitToWindowButton;

    [SerializeField]
    private Button easyButton;
    [SerializeField]
    private Button averageButton;
    [SerializeField]
    private Button hardButton;

    void Awake()
    {
        startNewGameButton.onClick.AddListener(() => OnStartNewGameButtonClick());
        quitToWindowButton.onClick.AddListener(() => OnQuitToWindowButtonClick());
        averageButton.onClick.AddListener(() => OnAverageButtonClick());
        easyButton.onClick.AddListener(() => OnEasyButtonClick());
        hardButton.onClick.AddListener(() => OnHardButtonClick());

    }



}
