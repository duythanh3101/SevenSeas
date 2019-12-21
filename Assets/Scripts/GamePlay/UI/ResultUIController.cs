using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIController : MonoBehaviour
{
    public System.Action OnStartNewGameButtonClick = delegate { };

    [SerializeField]
    private Text totalGameText;
    [SerializeField]
    private Text pirateSunkText;
    [SerializeField]
    private Text treasureFoundText;
    [SerializeField]
    private Text finalScoreText;

    [SerializeField]
    private Button startNewGameButton;

    [SerializeField]
    private CanvasGroup canvasGroup;

    void Awake()
    {
        startNewGameButton.onClick.AddListener(() => OnStartNewGameButtonClick());
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
