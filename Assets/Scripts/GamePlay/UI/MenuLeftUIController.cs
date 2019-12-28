using System.Collections;
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
        private Text scoreTExt;

        [Header("Object References")]
        [SerializeField]
        private QuitUIController quitUIController;
        [SerializeField]
        private OptionUIController optionUIController;

        void Awake()
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
        }

       

        void OnDestroy()
        {

            if (quitUIController != null)
            {
                quitUIController.OnQuitButtonClick -= quitUIController_OnQuitButtonClick;
                quitUIController.OnCancelButtonClick -= quitUIController_OnCancelButtonClick;
            }
           

            optionUIController.OnCloseButtonClick -= optionUIController_OnCloseButtonClick;
        }

        void OnUndoButtonClick()
        {

        }

        void OnExitButtonClick()
        {
            quitUIController.Show();

            if (GameManager.Instance != null)
                GameManager.Instance.PauseGame();
            
        }

        void OnOptionButtonClick()
        {
            
            optionUIController.Show();

            if (GameManager.Instance != null)
                GameManager.Instance.PauseGame();
            
        }

        private void quitUIController_OnCancelButtonClick()
        {
            quitUIController.Hide();
            if (GameManager.Instance != null)
                GameManager.Instance.ResumeGame();
        }
            

        private void optionUIController_OnCloseButtonClick()
        {
            optionUIController.Hide();
            if (GameManager.Instance != null)
                GameManager.Instance.ResumeGame();
           
        }

        private void quitUIController_OnQuitButtonClick()
        {
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
    }

}

