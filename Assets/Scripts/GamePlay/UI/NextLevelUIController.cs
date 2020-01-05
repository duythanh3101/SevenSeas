using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SevenSeas
{
    public class NextLevelUIController : MonoBehaviour
{
    public System.Action OnNextButtonClick = delegate { };

    [SerializeField]
    private Text levelTitleText;

    [SerializeField]
    private Text pirateSunkText;

    [SerializeField]
    private Text currentScoreText;
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


    public void SetData(GameSessionInfoManager.PlayerInfoSession session)
    {
        levelTitleText.text = "LEVEL " + session.levelInCheckPoint + " COMPLETED";

        //Debug.Log("Player score: " + session.playerScore);
        currentScoreText.text = "Current score: " + session.playerScore;

        //Debug.Log("pirates sunk: " + session.piratesSunk);
        pirateSunkText.text = "Pirates sunk: " + session.piratesSunk;
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
}

