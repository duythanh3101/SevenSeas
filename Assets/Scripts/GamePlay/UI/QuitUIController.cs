using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SevenSeas
{
    public class QuitUIController : MonoBehaviour
    {

        public System.Action OnQuitButtonClick = delegate { };
        public System.Action OnCancelButtonClick = delegate { };


        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Button yesButton;
        [SerializeField]
        private Button noButton;

        void Awake()
        {
            noButton.onClick.AddListener(() => OnCancelButtonClick());
            yesButton.onClick.AddListener(() => OnQuitButtonClick());
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

