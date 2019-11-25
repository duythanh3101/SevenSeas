using BaseSystems.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseGameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.RegisterListener(ObserverEventID.OnFindTreasureGameOver, (param) => OnFindTreasureGameOnver());
    }

    private void OnFindTreasureGameOnver()
    {
        new WaitForSeconds(3);
        SceneManager.LoadScene(CommonConstants.SceneName.MapMovementScene, LoadSceneMode.Single);
    }

}
