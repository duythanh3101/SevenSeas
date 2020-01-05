using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUIController : MonoBehaviour
{
    public System.Action OnCloseButtonClick = delegate{};
   
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private CanvasGroup canvasGroup;

    void Start()
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
