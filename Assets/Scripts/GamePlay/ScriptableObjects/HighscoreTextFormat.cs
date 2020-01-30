using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    [CreateAssetMenu(menuName = "HighScore Text Format")]
    public class HighscoreTextFormat : ScriptableObject
    {
        public Color indexColor;
        public Color usernameColor;
        public Color scoreColor;
    }
}

