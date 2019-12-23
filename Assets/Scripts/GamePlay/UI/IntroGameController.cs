using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SevenSeas
{
    public class IntroGameController : MonoBehaviour
    {

        [SerializeField]
        private Button playButton;

        // Start is called before the first frame update
        void Start()
        {
            playButton.onClick.AddListener(OnPlayButtonClick);
        }

        private void OnPlayButtonClick()
        {
            SceneLoader.Instance.LoadChooseLevelScene();
        }


    }
}

