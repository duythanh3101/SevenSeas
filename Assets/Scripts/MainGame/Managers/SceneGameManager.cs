using BaseSystems.Observer;
using BaseSystems.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGameManager : Singleton<SceneGameManager>
{
    public static void LoadNextMapFindingTreasure(bool isFindingTreasureEnded = false)
    {
        if (isFindingTreasureEnded)
        {
            SceneManager.LoadScene(CommonConstants.SceneName.CheckPointMapScene, LoadSceneMode.Single);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
