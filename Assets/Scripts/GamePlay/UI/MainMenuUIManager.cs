using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SevenSeas
{
    public class MainMenuUIManager : MonoBehaviour
    {

        [SerializeField]
        private MenuLeftUIController menuLeftUIController;
        [SerializeField]
        private ChooseLevelUIController chooseLevelUIController;
        [SerializeField]
        private OptionUIController optionUIController;
        [SerializeField]
        private ContinueUIController continueUIController;


        private bool blockElements = false;

        void Awake()
        {

            chooseLevelUIController.OnStartNewGameButtonClick += chooseLevelUIController_OnStartNewGameButtonClick;
            chooseLevelUIController.OnQuitToWindowButtonClick += chooseLevelUIController_OnQuitToWindowButtonClick;
            chooseLevelUIController.OnAverageButtonClick += chooseLevelUIController_OnAverageButtonClick;
            chooseLevelUIController.OnHardButtonClick += chooseLevelUIController_OnHardButtonClick;
            chooseLevelUIController.OnEasyButtonClick += chooseLevelUIController_OnEasyButtonClick;

            continueUIController.OnNoButtonClick += continueUIController_OnNoButtonClick;
            continueUIController.OnYesButtonClick += continueUIController_OnYesButtonClick;

            //Debug.Log("End player session: " + PlayerInfoManager.Instance.EndPlayerSession);
        }

      
        void OnDestroy()
        {
            chooseLevelUIController.OnStartNewGameButtonClick -= chooseLevelUIController_OnStartNewGameButtonClick;
            chooseLevelUIController.OnQuitToWindowButtonClick -= chooseLevelUIController_OnQuitToWindowButtonClick;
            chooseLevelUIController.OnAverageButtonClick -= chooseLevelUIController_OnAverageButtonClick;
            chooseLevelUIController.OnHardButtonClick -= chooseLevelUIController_OnHardButtonClick;
            chooseLevelUIController.OnEasyButtonClick -= chooseLevelUIController_OnEasyButtonClick;

            continueUIController.OnNoButtonClick -= continueUIController_OnNoButtonClick;
            continueUIController.OnYesButtonClick -= continueUIController_OnYesButtonClick;
        }

        private void chooseLevelUIController_OnStartNewGameButtonClick()
        {
            if (blockElements)
                return;

            if (!PlayerInfoManager.Instance.EndPlayerSession)
            {
                continueUIController.Show();
                menuLeftUIController.enabled = false;
                blockElements = true;
            }
            else
            {
                SceneLoader.Instance.LoadPlayScene();
            }
            
        }

        private void chooseLevelUIController_OnEasyButtonClick()
        {
            if (blockElements)
                return;

        }

        //Load last game session
        private void continueUIController_OnYesButtonClick()
        {
          
            //Load from json file to player info session
            PlayerInfoManager.Instance.LoadPlayerSession();

            //Load Play Scene
            SceneLoader.Instance.LoadPlayScene();

        }

        //Start a new game session
        private void continueUIController_OnNoButtonClick()
        {
          
            menuLeftUIController.enabled = true;

            //End current session
            PlayerInfoManager.Instance.ClearPlayerSession();

            //Load play scene
            SceneLoader.Instance.LoadPlayScene();
            
        }

        private void chooseLevelUIController_OnHardButtonClick()
        {
            if (blockElements)
                return;

        }

        private void chooseLevelUIController_OnAverageButtonClick()
        {
            if (blockElements)
                return;
        }

        private void chooseLevelUIController_OnQuitToWindowButtonClick()
        {
            if (blockElements)
                return;

            SceneLoader.Instance.ExitGame();
        }

        


    }

}
