using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUIController : MonoBehaviour
{
    [SerializeField]
    private Text levelTitleText;

    [SerializeField]
    private Text pirateSunkText;

    [SerializeField]
    private Text bonusPointText;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private CanvasGroup canvasGroup;

    void Start()
    {
        
    }

    void Display(bool isShowing)
    {
        canvasGroup.alpha = isShowing ? 1 : 0;
        canvasGroup.blocksRaycasts = isShowing;
        canvasGroup.interactable = isShowing;
    }

    void Show()
    {
        Display(true);
    }

    void Hide()
    {
        Display(false);
    }
}
