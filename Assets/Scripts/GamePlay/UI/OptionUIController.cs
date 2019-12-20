using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUIController : MonoBehaviour
{


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
        closeButton.onClick.AddListener(Hide);
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
