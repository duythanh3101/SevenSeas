using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        menuLeftUIController.OnOptionButtonClick += menuLeftUIController_OnOptionButtonClick;
        optionUIController.OnCloseButtonClick += optionUIController_OnCloseButtonClick;
       
    }

    private void optionUIController_OnCloseButtonClick()
    {
        optionUIController.Hide();
    }

    void OnDestroy()
    {
        menuLeftUIController.OnOptionButtonClick -= menuLeftUIController_OnOptionButtonClick;
        optionUIController.OnCloseButtonClick -= optionUIController_OnCloseButtonClick;
    }

    private void menuLeftUIController_OnOptionButtonClick()
    {
        optionUIController.Show();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }
}
