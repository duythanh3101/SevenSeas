﻿using BaseSystems.Singleton;
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
            nextLevelUIController.OnNextButtonClick += nextLevelUIController_OnNextButtonClick;
        }


       
        void OnDestroy()
        {
            GameManager.GameStateChanged -= GameManager_GameStateChanged;
            gameOverUIController.OnRestartButtonClick -= gameOverUIController_OnRestartButtonClick;
            resultUIController.OnStartNewGameButtonClick -= resultUIController_OnStartNewGameButtonClick;
            quitUIController.OnQuitButtonClick -= quitUIController_OnQuitButtonClick;
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
 	
        }

        private void quitUIController_OnQuitButtonClick()
        {
            
        }

        private void resultUIController_OnStartNewGameButtonClick()
        {
            
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
