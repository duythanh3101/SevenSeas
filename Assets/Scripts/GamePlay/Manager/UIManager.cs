
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SevenSeas
{
    public class UIManager : MonoBehaviour
    {

        private GameObject treasureGameOverPanel;
        public static UIManager Instance = null;

        [SerializeField]
        private Image[] healthImages;
        [SerializeField]
        private GameOverUIController gameOverUIController;
        [SerializeField]
        private ResultUIController resultUIController;
        [SerializeField]
        private QuitUIController quitUIController;
        [SerializeField]
        private OptionUIController optionUIController;
        [SerializeField]
        private NextLevelUIController nextLevelUIController;
        [SerializeField]
        private MenuLeftUIController menuLeftUIController;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                DestroyImmediate(gameObject);

            GameManager.GameStateChanged += GameManager_GameStateChanged;

            resultUIController.OnStartNewGameButtonClick += resultUIController_OnStartNewGameButtonClick;

            quitUIController.OnQuitButtonClick += quitUIController_OnQuitButtonClick;

            nextLevelUIController.OnNextButtonClick += nextLevelUIController_OnNextButtonClick;

            menuLeftUIController.OnExitButtonClick += menuLeftUIController_OnExitButtonClick;
            menuLeftUIController.OnOptionButtonClick += menuLeftUIController_OnOptionButtonClick;

            optionUIController.OnCloseButtonClick += optionUIController_OnCloseButtonClick;
        }

       
        void OnDestroy()
        {
            GameManager.GameStateChanged -= GameManager_GameStateChanged;

            resultUIController.OnStartNewGameButtonClick -= resultUIController_OnStartNewGameButtonClick;

            quitUIController.OnQuitButtonClick -= quitUIController_OnQuitButtonClick;

            nextLevelUIController.OnNextButtonClick -= nextLevelUIController_OnNextButtonClick;

            menuLeftUIController.OnExitButtonClick -= menuLeftUIController_OnExitButtonClick;
            menuLeftUIController.OnOptionButtonClick -= menuLeftUIController_OnOptionButtonClick;

            optionUIController.OnCloseButtonClick -= optionUIController_OnCloseButtonClick;
        }

        private void optionUIController_OnCloseButtonClick()
        {
            optionUIController.Hide();
            GameManager.Instance.ResumeGame();
        }

        void gameOverUIController_OnRestartButtonClick()
        {
            GameManager.Instance.RestartGame();
        }

        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.GameOver)
            {
                //Debug.Log("Game over");
                StartCoroutine(CR_DelayGameOverUI());
            }
            else if (newState == GameState.GameWin)
            {
                //Debug.Log("Game win");
                StartCoroutine(CR_ShowNextLevelUI());
            }
        }

        void Start()
        {
            InitValues();
        }


        private void menuLeftUIController_OnOptionButtonClick()
        {
            optionUIController.Show();
            GameManager.Instance.PauseGame();
        }

        private void menuLeftUIController_OnExitButtonClick()
        {
            quitUIController.Show();
        }

        void InitValues()
        {
            int healthCount = FindObjectOfType<PlayerController>().playerHealth;
            for (int i = 0; i < healthCount; i++)
            {
                healthImages[i].gameObject.SetActive(true);
            }
        }

        public void DecreaseHealth(int index)
        {
            healthImages[index].gameObject.SetActive(false);
        }

        public void ShowFindTreasureGameOver()
        {
            treasureGameOverPanel.SetActive(true);
        }

        private void nextLevelUIController_OnNextButtonClick()
        {
            GameManager.Instance.RestartGame();
        }

        private void quitUIController_OnQuitButtonClick()
        {
            SceneLoader.Instance.LoadChooseLevelScene();
        }

        private void resultUIController_OnStartNewGameButtonClick()
        {
            GameManager.Instance.RestartGame();
        }

       IEnumerator CR_DelayGameOverUI()
        {
            yield return new WaitForSeconds(1);
            gameOverUIController.Show();
            yield return new WaitForSeconds(gameOverUIController.timeDisplay + 2);
            resultUIController.Show();

            
        }

        IEnumerator  CR_ShowNextLevelUI()
       {
           yield return new WaitForSeconds(1);

           nextLevelUIController.Show();
       }
    }
}
