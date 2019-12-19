using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    public enum BattleState
    {
        PlayerTurn,
        EnemyTurn,
        EndBattle
    }

    public class TurnBasedSystemManager : MonoBehaviour
    {
        public static TurnBasedSystemManager Instance = null;
        public static System.Action<BattleState> BattleStateChanged;

        [SerializeField]
        private BattleState battleState;

        public BattleState BattleState
        {
            get
            {
                return battleState;
            }
            private set
            {
                if (value != battleState)
                {
                    //Debug.Log("Change state to: " + value);
                    battleState = value;
                    BattleStateChanged(value);
                }
            }
        }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(gameObject);

            EffectManager.OnAllBehaviourCompleted += EffectManager_OnAllBehaviourCompleted;
            GameManager.GameStateChanged += GameManager_GameStateChanged;
        }

        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.GameOver || newState == GameState.GameWin)
            {
                BattleState = BattleState.EndBattle;
            }
        }

        void OnDestroy()
        {
            EffectManager.OnAllBehaviourCompleted -= EffectManager_OnAllBehaviourCompleted;
            GameManager.GameStateChanged -= GameManager_GameStateChanged;
        }

        private void EffectManager_OnAllBehaviourCompleted(BattleState oldState)
        {
            
            
            if (oldState == BattleState.EnemyTurn)
            {
               BattleState = BattleState.PlayerTurn;
            }
            else if (oldState == BattleState.PlayerTurn)
            {
                BattleState = BattleState.EnemyTurn;
            }

            //Debug.Log("Current state: " + BattleState);
        }
    }
}

