using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SevenSeas
{
    public class SceneLoader : MonoBehaviour
{
        public static SceneLoader Instance = null;

        [SerializeField]
         private Sound playSceneMusic;
        [SerializeField]
        private Sound chooseLevelSceneMusic;
        [SerializeField]
        private Sound introSceneMusic;
        [SerializeField]
        private Sound treasureMapSceneMusic;
        [SerializeField]
        private Sound checkPointMapSceneMusic;
      

        private static readonly string PLAY_SCENE_NAME = "PlayScene";
        private static readonly string CHOOSE_LEVEL_SCENE_NAME = "ChooseLevel";
        private static readonly string INTRO_SCENE_NAME = "IntroGame";
        private static readonly string TREASURE_MAP_SCENE_NAME = "TreasureMapScene";
        private static readonly string CHECK_POINT_MAP_SCENE_NAME = "CheckPointMapScene";

        void Awake()
        {
            if (Instance == null)
            {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
                DestroyImmediate(gameObject);
        }

        void Start()
        {
            SoundManager.Instance.PlayMusic(introSceneMusic);
        }

        public void LoadPlayScene()
        {

            LoadScene(PLAY_SCENE_NAME);
           
        }

        public void LoadChooseLevelScene()
        {
            LoadScene(CHOOSE_LEVEL_SCENE_NAME);
            SoundManager.Instance.PlayMusic(chooseLevelSceneMusic);
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }


        public void ExitGame()
        {
            Application.Quit();
        }
}

}
