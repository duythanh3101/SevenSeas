using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    [System.Serializable]
    public enum BattleState
    {
        PlayerTurn,
        EnemyTurn,
        EndBattle
    }

    public class TurnBasedSystemManager : MonoBehaviour
    {
        public static TurnBasedSystemManager Instance = null;

        public static  event System.Action<BattleState> BattleStateChanged = delegate { };

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
                    battleState = value;
                    BattleStateChanged(value);

                }
            }
        }

        [SerializeField]
        private BattleState battleState;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null)
                DestroyImmediate(gameObject);

            EffectManager.OnAllBehaviourCompleted += EffectManager_OnAllBehaviourCompleted;
            GameManager.GameStateChanged += GameManager_GameStateChanged;
        }

        void OnDestroy()
        {
            EffectManager.OnAllBehaviourCompleted -= EffectManager_OnAllBehaviourCompleted;
            GameManager.GameStateChanged -= GameManager_GameStateChanged;
        }

        void GameManager_GameStateChanged(GameState newState, GameState oldState)
        {
            if (newState == GameState.GameOver)
            {
                BattleState = BattleState.EndBattle;
            }
        }

        void EffectManager_OnAllBehaviourCompleted(BattleState oldState)
        {
           // Debug.Log("Change state from: " + BattleState);

            if (BattleState == BattleState.EnemyTurn)
            {
                BattleState = BattleState.PlayerTurn;
            }
            else if (BattleState == BattleState.PlayerTurn)
            {
                BattleState = BattleState.EnemyTurn;
            }


        }
    }
}


