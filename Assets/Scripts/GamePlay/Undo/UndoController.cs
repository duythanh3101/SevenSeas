using BaseSystems.Observer;
using BaseSystems.Singleton;
using SevenSeas;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoController : Singleton<UndoController>
{
    public bool isAllowedUndo;

    public Text WhateverTextThingy;  //Add reference to UI Text here via the inspector
    private float timeToAppear = 2f;
    private float timeWhenDisappear;

    //Call to enable the text, which also sets the timer
    public void EnableText()
    {
        WhateverTextThingy.gameObject.SetActive(true);
        timeWhenDisappear = Time.time + timeToAppear;
    }

    protected override void Awake()
    {
        isAllowedUndo = true;

        Observer.Instance.RegisterListener(ObserverEventID.OnCantUndo, (param) => OnCantUndo());
    }

    private void OnCantUndo()
    {
        isAllowedUndo = false;
        Observer.Instance.PostEvent(ObserverEventID.OnResetListUndo);
    }

    //We check every frame if the timer has expired and the text should disappear
    void Update()
    {
        if (WhateverTextThingy.enabled && (Time.time >= timeWhenDisappear))
        {
            WhateverTextThingy.gameObject.SetActive(false);
        }
    }

    public void Undo()
    {
        if (IsAllowedUndo() && GameManager.Instance.GameState == GameState.Playing)
        {
            Debug.Log("Allow");
            this.PostEvent(ObserverEventID.OnUndo);
        }
        else
        {
            isAllowedUndo = true;
            EnableText();
        }
    }

    private bool IsAllowedUndo()
    {
        return isAllowedUndo;
    }


}
