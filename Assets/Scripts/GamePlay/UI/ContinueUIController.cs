using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueUIController : MonoBehaviour
{

    public System.Action OnYesButtonClick = delegate { };
    public System.Action OnNoButtonClick = delegate { };

    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;
    [SerializeField]
    private CanvasGroup canvasGroup;


    void Awake()
    {
        yesButton.onClick.AddListener(() => OnYesButtonClick());
        noButton.onClick.AddListener(() => OnNoButtonClick());
    }

    public void Show()
    {
        Display(true);
    }

    public void Hide()
    {
        Display(false);
    }

    void Display(bool isShowing)
    {
        canvasGroup.alpha = isShowing ? 1 : 0;
        canvasGroup.blocksRaycasts = isShowing;
        canvasGroup.interactable = isShowing;
    }


}
