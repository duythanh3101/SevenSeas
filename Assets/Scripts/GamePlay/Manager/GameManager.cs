using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SevenSeas
{

    public enum GameState
    {
        Prepare,
        Playing,
        Pause,
        PregameOver,
        GameOver,
        GameWin
    }

    public class GameManager : MonoBehaviour
    {
        public static event System.Action<GameState, GameState> GameStateChanged;

        public static GameManager Instance = null;

        [SerializeField]
        private GameState _gameState = GameState.Prepare;

        [SerializeField]
        private int targetFrameRate = 60;

       
        public PlayerController playerController;
        

        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            set
            {
                if (value != _gameState)
                {
                    GameState oldState = _gameState;
                    _gameState = value;

                    if (GameStateChanged != null)
                        GameStateChanged(_gameState, oldState);
                }
               
            }
        }

        public static int LEVEL = 1;
       
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            } 
            else if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
        }

     
        void Start()
        {
            Application.targetFrameRate = targetFrameRate;
            //PrepareGame();
            StartGame();
        }

       
       public void GameWin()
        {
            GameSessionInfoManager.Instance.UpdateLevelInCheckPoint();

            SoundManager.Instance.PauseMusic();
            
            SoundManager.Instance.PlayWinSound();
            GameState = GameState.GameWin;
        }

        public void GameLose()
       {
        
           SoundManager.Instance.StopMusic();
           SoundManager.Instance.PlayLoseSound();

           GameState = GameState.GameOver;

       }

        bool isRestart = false;
        public void RestartGame()
        {
            isRestart = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void PrepareGame()
        {
            GameState = GameState.Prepare;
            if (isRestart)
            {
                StartGame();
                isRestart = false;
            }
        }

        public void StartGame()
        {
            GameState = GameState.Playing;

            if (SoundManager.Instance.MusicState == SoundManager.PlayingState.Paused)
                SoundManager.Instance.ResumeMusic();
            else
            {
                SoundManager.Instance.PlayMusic(SoundManager.Instance.background);
            }
            
        }

        GameState suspendState;
        public void PauseGame()
        {
            suspendState = GameState;
            GameState = GameState.Pause;

        }

        public void ResumeGame()
        {
            GameState = suspendState;
        }
        
        public void GoToNextLevel()
        {
            if (GameSessionInfoManager.Instance.playerInfoSession.levelInCheckPoint >= CommonConstants.MAX_LEVEL_PER_CHECKPOINT)
            {
                SceneLoader.Instance.LoadTreasureMapScene();
            }
            else
            {
                RestartGame();
            }
        }
    }
}


