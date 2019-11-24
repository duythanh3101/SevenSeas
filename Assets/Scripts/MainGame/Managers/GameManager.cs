using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SevenSeas
{

    public enum GameState
    {
        Prepare,
        Playing,
        Pause,
        PregameOver,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public static event System.Action<GameState, GameState> GameStateChanged;

        public static GameManager Instance = null;

        [SerializeField]
        private GameState _gameState = GameState.Prepare;

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
                DontDestroyOnLoad(gameObject);
                return;
            } 
            else if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
               
        }


       
    }
}


