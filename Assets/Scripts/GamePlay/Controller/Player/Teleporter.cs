using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    public class Teleporter : MonoBehaviour
    {
        

        public void Teleport(GameObject teleUnit, bool isRecycle)
        {
            MapConstantProvider.Instance.LayoutUnitAtRandomPosition(teleUnit, isRecycle);
        }
    }
}

