using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SevenSeas
{
    public class ResultUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField]
        private Text checkPointText;
        [SerializeField]
        private Text pirateSunkText;
        [SerializeField]
        private Text treasureFoundText;
        [SerializeField]
        private Text finalScoreText;

        [SerializeField]
        private Button startNewGameButton;
        [SerializeField]
        private Button submitButton;

        [SerializeField]
        private CanvasGroup canvasGroup;

        [Header("Script References")]
        [SerializeField]
        private SubmitHighscoreUIController submitController;
        [SerializeField]
        private List<HighscoreTextController> highscoreTextControllers;

        #region Cache values
        private bool firstShowSubmitUI;
        private bool lockElements;

        #endregion

        void Awake()
        {
            startNewGameButton.onClick.AddListener(OnStartNewGameButtonClick);
            submitButton.onClick.AddListener(OnSubmitButtonClick);

            submitController.OnCancelButtonClick += submitController_OnCancelButtonClick;
            submitController.OnYesButtonClick += submitController_OnYesButtonClick;

            GameSessionInfoManager.OnLeaderboardDataLoaded += GameSessionInfoManager_OnLeaderboardDataLoaded;
        }

        void OnDestroy()
        {
            submitController.OnCancelButtonClick -= submitController_OnCancelButtonClick;
            submitController.OnYesButtonClick -= submitController_OnYesButtonClick;

            GameSessionInfoManager.OnLeaderboardDataLoaded -= GameSessionInfoManager_OnLeaderboardDataLoaded;
        }

        private void GameSessionInfoManager_OnLeaderboardDataLoaded()
        {
            List<HighScoreModel> highscores = GameSessionInfoManager.Instance.highscores;

            for (int i = 0; i < highscores.Count;i++)
            {
                highscoreTextControllers[i].SetData(i+ 1,highscores[i]);
            }
        }

        private void OnStartNewGameButtonClick()
        {
            if (lockElements)
                return;

            GameSessionInfoManager.Instance.ClearGameSession();
            SceneLoader.Instance.LoadChooseLevelScene();
        }

        private void OnSubmitButtonClick()
        {
            submitController.Show();
        }

        private void submitController_OnCancelButtonClick()
        {
            if (lockElements)
                return;

            lockElements = false;
            submitController.Hide();
        }

        private void submitController_OnYesButtonClick()
        {
            if (lockElements)
                return;

            lockElements = true;
            string username = submitController.nameField.text;
            submitController.ShowResultText("Uploading highscore to leaderboard...");
            if (!string.IsNullOrEmpty(username))
            {
                LeaderboardManager.Instance.UploadScore(new HighScoreModel(username, GameSessionInfoManager.Instance.playerInfoSession.playerScore),
                () =>
                {
                    lockElements = false;
                    submitController.ShowResultText("Your highscore was uploaded successfully!");
                    GameSessionInfoManager.Instance.LoadLeaderboard();
                },
                () =>
                {
                    lockElements = false;
                    submitController.ShowResultText("Upload failed! Please try again later!");
                });
            }
        }

        void Display(bool isShowing)
        {
            canvasGroup.alpha = isShowing ? 1 : 0;
            canvasGroup.blocksRaycasts = isShowing;
            canvasGroup.interactable = isShowing;
        }

        public void SetData(GameSessionInfoManager.PlayerInfoSession session)
        {

            checkPointText.text = session.checkPoint.ToString();
            treasureFoundText.text = session.treasureFound.ToString();
            finalScoreText.text = session.playerScore.ToString();
            pirateSunkText.text = session.piratesSunk.ToString();
        }

        public void Show()
        {
            Display(true);
            if (!firstShowSubmitUI)
            {
                submitController.Show();
                firstShowSubmitUI = true;
            }
        }

        public void Hide()
        {
            Display(false);
        }
    }
}


