using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuLeftUIController : MonoBehaviour
{
    public System.Action OnOptionButtonClick = delegate { };
    public System.Action OnExitButtonClick = delegate { };

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Button undoButton;

    [SerializeField]
    private Button optionButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text scoreTExt;


    void Awake()
    {
        undoButton.onClick.AddListener(OnUndoButtonClick);
        optionButton.onClick.AddListener(() => OnOptionButtonClick());
        exitButton.onClick.AddListener(() => OnExitButtonClick());
    }


    void OnUndoButtonClick()
    {

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
