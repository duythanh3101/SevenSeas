using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SevenSeas
{
    public class EffectManager : MonoBehaviour
    {

        public static event Action<BattleState> OnAllBehaviourCompleted = delegate { };

        public static EffectManager Instance = null;

        [Header("Effects")]
        public GameObject waterSplash;
        public GameObject canonFiring;
        public GameObject explosion;

        private Dictionary<GameObject, Action<GameObject>> effectStopHandlers = new Dictionary<GameObject, Action<GameObject>>();

        [HideInInspector]
        public bool effectPlaying = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
                DestroyImmediate(gameObject);

            PlayerController.OnBoatActivityCompleted += PlayerController_OnBoatActivityCompleted;
            EnemyManager.OnAllEnemyActivityCompleted += EnemyManager_OnAllEnemyActivityCompleted;
        }

        void OnDestroy()
        {
            PlayerController.OnBoatActivityCompleted -= PlayerController_OnBoatActivityCompleted;
            EnemyManager.OnAllEnemyActivityCompleted -= EnemyManager_OnAllEnemyActivityCompleted;
        }

        private void PlayerController_OnBoatActivityCompleted(BoatController player)
        {
            if (player.GetType() == typeof(PlayerController))
            {
                if (!effectPlaying)
                {
                   // Debug.Log("Chane state not effect, current state:"  + TurnBasedSystemManager.Instance.BattleState);
                    OnAllBehaviourCompleted(TurnBasedSystemManager.Instance.BattleState);
                }
            }
        }

        private void EnemyManager_OnAllEnemyActivityCompleted()
        {
            if (!effectPlaying)
            {
               // Debug.Log("Chane state not effect: " + TurnBasedSystemManager.Instance.BattleState);
                OnAllBehaviourCompleted(TurnBasedSystemManager.Instance.BattleState);
            }
        }

        public void SpawnEffect(GameObject effect, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (effect == null)
            {
                throw new UnityException("The effect you're trying to spawn has not been assigned!");
            }

            effectPlaying = true;
            AutoStopEffect stopEffect = Instantiate(effect, position, rotation).GetComponent<AutoStopEffect>();
            stopEffect.OnEffectStoped += AutoStopEffect_OnEffectStoped;
            AddNewEffectHandler(stopEffect.gameObject, stopEffect.OnEffectStoped);


        }

        void AddNewEffectHandler(GameObject effect, Action<GameObject> callBack)
        {
            if (effectStopHandlers.ContainsKey(effect))
                return;

            effectStopHandlers.Add(effect, callBack);

        }

        void AutoStopEffect_OnEffectStoped(GameObject effect)
        {
            if (!effectStopHandlers.ContainsKey(effect))
                return;

            effectStopHandlers.Remove(effect);
            if (effectStopHandlers.Count == 0)
            {
                OnAllEffectCompleted();
            }
                
        }

        void OnAllEffectCompleted()
        {
            effectPlaying = false;
            //Debug.Log("Change state with effect, current state: " + TurnBasedSystemManager.Instance.BattleState );
            OnAllBehaviourCompleted(TurnBasedSystemManager.Instance.BattleState);
        }

    }
}

