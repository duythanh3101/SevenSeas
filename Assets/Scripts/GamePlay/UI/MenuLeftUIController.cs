﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SevenSeas
{
    public class MenuLeftUIController : MonoBehaviour
    {
       
        [Header("UI References")]
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
        private Text scoreText;

        [Header("Object References")]
        [SerializeField]
        private QuitUIController quitUIController;
        [SerializeField]
        private OptionUIController optionUIController;
        [SerializeField]
        private DescriptionUIController descriptionUIController;

        private bool blockElements;

        void OnEnable()
        {
            undoButton.onClick.AddListener(OnUndoButtonClick);
            optionButton.onClick.AddListener(() => OnOptionButtonClick());
            exitButton.onClick.AddListener(() => OnExitButtonClick());

            if (quitUIController != null)
            {
                quitUIController.OnQuitButtonClick += quitUIController_OnQuitButtonClick;
                quitUIController.OnCancelButtonClick += quitUIController_OnCancelButtonClick;
            }

            optionUIController.OnCloseButtonClick += optionUIController_OnCloseButtonClick;
            optionUIController.OnHelpButtonClick += optionUIController_OnHelpButtonClick;

            if (descriptionUIController != null)
                descriptionUIController.OnCloseButtonClick += descriptionUIController_OnCloseButtonClick;
            
        }

        private void optionUIController_OnHelpButtonClick()
        {
            blockElements = true;
            descriptionUIController.Show();
        }

        void OnDisable()
        {

            if (quitUIController != null)
            {
                quitUIController.OnQuitButtonClick -= quitUIController_OnQuitButtonClick;
                quitUIController.OnCancelButtonClick -= quitUIController_OnCancelButtonClick;
            }

            undoButton.onClick.RemoveAllListeners();
            optionButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();

            optionUIController.OnCloseButtonClick -= optionUIController_OnCloseButtonClick;

            optionUIController.OnHelpButtonClick -= optionUIController_OnHelpButtonClick;

            if (descriptionUIController != null)
                descriptionUIController.OnCloseButtonClick -= descriptionUIController_OnCloseButtonClick;
            
        }

        void OnUndoButtonClick()
        {
            UndoController.Instance.Undo();
        }

        void OnExitButtonClick()
        {
            if (blockElements)
                return;

            quitUIController.Show();

            blockElements = true;
            if (GameManager.Instance != null)
                GameManager.Instance.PauseGame();
           
        }

        void OnOptionButtonClick()
        {
            if (blockElements)
                return;

            optionUIController.Show();
            blockElements = true;

            if (GameManager.Instance != null)
                GameManager.Instance.PauseGame();
            
        }

        private void descriptionUIController_OnCloseButtonClick()
        {
            descriptionUIController.Hide();
            blockElements = false;
        }

        private void quitUIController_OnCancelButtonClick()
        {
            quitUIController.Hide();
            if (GameManager.Instance != null)
                GameManager.Instance.ResumeGame();
            blockElements = false;
        }
            

        private void optionUIController_OnCloseButtonClick()
        {
            optionUIController.Hide();
            if (GameManager.Instance != null)
                GameManager.Instance.ResumeGame();
            blockElements = false;
        }

        private void quitUIController_OnQuitButtonClick()
        {
            GameSessionInfoManager.Instance.SaveGameSession();

            SceneLoader.Instance.LoadChooseLevelScene();
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

        public void UpdateScore(int amount)
        {
            scoreText.text = amount.ToString();
        }

        public void SetData(GameSessionInfoManager.PlayerInfoSession session)
        {
            scoreText.text = session.playerScore.ToString();

            if (session.levelInCheckPoint >= CommonConstants.MAX_LEVEL_PER_CHECKPOINT)
            {
                levelText.text = "1";
            }
            else
            {
                levelText.text = (session.levelInCheckPoint + 1).ToString();
            }
            
        }
    }

}

