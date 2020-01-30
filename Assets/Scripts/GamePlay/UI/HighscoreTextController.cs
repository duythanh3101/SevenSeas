using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SevenSeas
{
    
    public class HighscoreTextController : MonoBehaviour
    {
        [SerializeField]
        private HighscoreTextFormat highscoreTextFormat;
        [SerializeField]
        private Text usernameText;
        [SerializeField]
        private Text scoreText;
        
        
        void Start()
        {
           
            gameObject.SetActive(false);
        }

        public void SetData(int index, HighScoreModel highscore)
        {
            gameObject.SetActive(true);
            string usernameStr = FormatText(highscore.username, highscoreTextFormat.usernameColor);
            string indexStr = FormatText(index.ToString(), highscoreTextFormat.indexColor) + ".";
            usernameText.text = string.Format("{0}    {1}", indexStr, usernameStr);
            scoreText.text = highscore.score.ToString();
        }

        private string FormatText(string content, Color color)
        {

            return string.Format("<color={0}>{1}</color>", "#" +  ColorUtility.ToHtmlStringRGB(color), content);

        }
    }
}

