﻿using BaseSystems.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject treasureGameOverPanel;
    
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowFindTreasureGameOver()
    {
        treasureGameOverPanel.SetActive(true);
    }
}
