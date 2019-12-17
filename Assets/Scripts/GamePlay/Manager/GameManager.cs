﻿using System.Collections;
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
                DontDestroyOnLoad(gameObject);
                return;
            } 
            else if (Instance != null)
            {
                DestroyImmediate(gameObject);
            }
               
        }

        void Start()
        {
            Application.targetFrameRate = targetFrameRate;
        }


       public void GameWin()
        {
            GameState = GameState.GameWin;
        }

        public void GameLose()
       {

           SoundManager.Instance.PlayLoseSound();
           GameState = GameState.GameOver;
       }
    }
}


