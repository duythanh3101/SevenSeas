using BaseSystems.Singleton;
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


        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                DestroyImmediate(gameObject);

            GameManager.GameStateChanged += GameManager_GameStateChanged;

            gameOverUIController.OnRestartButtonClick += gameOverUIController_OnRestartButtonClick;
            resultUIController.OnStartNewGameButtonClick += resultUIController_OnStartNewGameButtonClick;
            quitUIController.OnQuitButtonClick += quitUIController_OnQuitButtonClick;
            resultUIController.OnStartNewGameButtonClick += quitUIController.OnQuitButtonClick;

        }

        private void quitUIController_OnQuitButtonClick()
        {
            
        }

        private void resultUIController_OnStartNewGameButtonClick()
        {
            
        }

        void OnDestroy()
        {
            GameManager.GameStateChanged -= GameManager_GameStateChanged;
            gameOverUIController.OnRestartButtonClick -= gameOverUIController_OnRestartButtonClick;
            resultUIController.OnStartNewGameButtonClick -= resultUIController_OnStartNewGameButtonClick;
            quitUIController.OnQuitButtonClick -= quitUIController_OnQuitButtonClick;
            resultUIController.OnStartNewGameButtonClick -= quitUIController.OnQuitButtonClick;
        }

        void gameOverUIController_OnRestartButtonClick()
        {
            GameManager.Instance.RestartGame();
        }

       

        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.GameOver)
            {
                StartCoroutine(CR_ShowGameOverUI());
            }
        }

        void Start()
        {
            InitValues();
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

       IEnumerator CR_ShowGameOverUI()
        {
            yield return new WaitForSeconds(1);

            gameOverUIController.Show();
        }
    }
}
