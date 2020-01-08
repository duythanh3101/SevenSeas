
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
        private NextLevelUIController nextLevelUIController;
        [SerializeField]
        private MenuLeftUIController menuLeftUIController;
        

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                DestroyImmediate(gameObject);

            //Event
            GameManager.GameStateChanged += GameManager_GameStateChanged;

            resultUIController.OnStartNewGameButtonClick += resultUIController_OnStartNewGameButtonClick;
            nextLevelUIController.OnNextButtonClick += nextLevelUIController_OnNextButtonClick;
            

            menuLeftUIController.SetData(GameSessionInfoManager.Instance.playerInfoSession);
            
        }

        void OnDestroy()
        {
            GameManager.GameStateChanged -= GameManager_GameStateChanged;

            resultUIController.OnStartNewGameButtonClick -= resultUIController_OnStartNewGameButtonClick;
            nextLevelUIController.OnNextButtonClick -= nextLevelUIController_OnNextButtonClick;
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
            DecreaseHealth(GameSessionInfoManager.Instance.playerInfoSession.playerHealth);
        }

        int healthCount;

        void InitValues()
        {
            healthCount = GameSessionInfoManager.Instance.playerInfoSession.playerHealth;
            for (int i = 0; i < healthCount; i++)
            {
                healthImages[i].gameObject.SetActive(true);
            }
        }

        public void DecreaseHealth(int index)
        {
            if (index == healthCount)
                return;
            healthImages[index].gameObject.SetActive(false);
        }

        public void IncreaseHealth(int index)
        {

            if (index == 3)
                return;

            //Debug.Log("index: " + index);
            healthImages[index].gameObject.SetActive(true);
        }

        public void ShowFindTreasureGameOver()
        {
            treasureGameOverPanel.SetActive(true);
        }

        public void UpdateScore(int amount)
        {
            menuLeftUIController.UpdateScore(amount);
        }

        private void nextLevelUIController_OnNextButtonClick()
        {
            GameManager.Instance.GoToNextLevel();
        }

        private void resultUIController_OnStartNewGameButtonClick()
        {
            GameSessionInfoManager.Instance.ClearGameSession();
            SceneLoader.Instance.LoadChooseLevelScene();
        }

       IEnumerator CR_DelayGameOverUI()
        {
            
            SetDataForResultUI(GameSessionInfoManager.Instance.playerInfoSession);
            menuLeftUIController.enabled = false; 

            yield return new WaitForSeconds(1);
            gameOverUIController.Show();
            yield return new WaitForSeconds(gameOverUIController.timeDisplay + 2);
            resultUIController.Show();

        }

        IEnumerator  CR_ShowNextLevelUI()
       {

           SetDataForNextLevelUI(GameSessionInfoManager.Instance.playerInfoSession);
           menuLeftUIController.enabled = false;

           yield return new WaitForSeconds(1);

           nextLevelUIController.Show();
       }

        public void SetDataForResultUI(GameSessionInfoManager.PlayerInfoSession session)
        {
            resultUIController.SetData(session);
        }


        public void SetDataForNextLevelUI(GameSessionInfoManager.PlayerInfoSession session)
        {
            nextLevelUIController.SetData(session);
        }
    }
}
