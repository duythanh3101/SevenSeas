using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelUIController : MonoBehaviour
{
    public System.Action OnNextButtonClick = delegate { };

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

    void Awake()
    {
        nextButton.onClick.AddListener(() => OnNextButtonClick());
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
