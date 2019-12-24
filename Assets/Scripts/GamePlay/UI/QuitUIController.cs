using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitUIController : MonoBehaviour
{

    public System.Action OnQuitButtonClick = delegate { };

    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button noButton;

    void Awake()
    {
        noButton.onClick.AddListener(Hide);
        yesButton.onClick.AddListener(() => OnQuitButtonClick());
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
