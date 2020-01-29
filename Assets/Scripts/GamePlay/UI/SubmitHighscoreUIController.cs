using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmitHighscoreUIController : MonoBehaviour
{

    public event System.Action OnYesButtonClick = delegate { };
    public event System.Action OnCancelButtonClick = delegate { };

    public InputField nameField;
    [SerializeField]
    private Button yesButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Text resultText;

    void Awake()
    {
        yesButton.onClick.AddListener(() => OnYesButtonClick());
        cancelButton.onClick.AddListener(() => OnCancelButtonClick());
        
    }
    void Start()
    {
        Hide();
    }


    void Display(bool isShowing)
    {
        canvasGroup.alpha = isShowing ? 1 : 0;
        canvasGroup.interactable = isShowing;
        canvasGroup.blocksRaycasts = isShowing;
    }

   public void Show()
   {
       Display(true);
   }

    public void Hide()
   {
       resultText.gameObject.SetActive(false);
       Display(false);
   }

    public void ShowResultText(string result)
    {
        resultText.gameObject.SetActive(true);
        resultText.text = result;
    }
}
