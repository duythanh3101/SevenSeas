using MainGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class XSignManager : MonoBehaviour
    {
        [SerializeField]
        private XSign xSign;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            xSign.SetText(string.Empty);
        }
    }
}
