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

        void Awake()
        {


            

            chooseLevelUIController.OnStartNewGameButtonClick += chooseLevelUIController_OnStartNewGameButtonClick;
            chooseLevelUIController.OnQuitToWindowButtonClick += chooseLevelUIController_OnQuitToWindowButtonClick;
            chooseLevelUIController.OnAverageButtonClick += chooseLevelUIController_OnAverageButtonClick;
            chooseLevelUIController.OnHardButtonClick += chooseLevelUIController_OnHardButtonClick;
            chooseLevelUIController.OnEasyButtonClick += chooseLevelUIController_OnEasyButtonClick;
        }



        void OnDestroy()
        {
            chooseLevelUIController.OnStartNewGameButtonClick -= chooseLevelUIController_OnStartNewGameButtonClick;
            chooseLevelUIController.OnQuitToWindowButtonClick -= chooseLevelUIController_OnQuitToWindowButtonClick;
            chooseLevelUIController.OnAverageButtonClick -= chooseLevelUIController_OnAverageButtonClick;
            chooseLevelUIController.OnHardButtonClick -= chooseLevelUIController_OnHardButtonClick;
            chooseLevelUIController.OnEasyButtonClick -= chooseLevelUIController_OnEasyButtonClick;
        }


        private void chooseLevelUIController_OnStartNewGameButtonClick()
        {
            SceneLoader.Instance.LoadPlayScene();
        }

        private void chooseLevelUIController_OnEasyButtonClick()
        {
           
        }

        private void chooseLevelUIController_OnHardButtonClick()
        {
            
        }

        private void chooseLevelUIController_OnAverageButtonClick()
        {
            
        }

        private void chooseLevelUIController_OnQuitToWindowButtonClick()
        {
            SceneLoader.Instance.ExitGame();
        }

        


    }

}
