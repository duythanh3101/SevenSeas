using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class XSign: MonoBehaviour
    {
        [SerializeField]
        private TextMesh xSignText;

        public XSign(TextMesh xSignText)
        {
            this.xSignText = xSignText;
        }

        public TextMesh XSignText
        {
            get => xSignText;
            set => xSignText = value;
        }

        public void SetText(string contentOfNumber)
        {
            xSignText.text = contentOfNumber;
        }

        public void SetText(int contentOfNumber)
        {
            xSignText.text = contentOfNumber.ToString();
        }
    }
}
