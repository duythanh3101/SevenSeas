using BaseSystems.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoController : MonoBehaviour
{
    public void Undo()
    {
        this.PostEvent(ObserverEventID.OnUndo);
    }
}
