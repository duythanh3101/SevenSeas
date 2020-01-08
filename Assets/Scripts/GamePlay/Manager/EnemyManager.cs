using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    public class EnemyManager : MonoBehaviour
    {

        public static EnemyManager Instance = null;

        public static event System.Action OnAllEnemyActivityCompleted;

        public int CurrentEnemyCount { get; private set; }
       void Awake()
        {

           if (Instance == null)
               Instance = this;
           else if (Instance != this)
           {
               DestroyImmediate(gameObject);
           }

            EnemyController.OnBoatActivityCompleted += EnemyController_OnBoatActivityCompleted;
            EnemyController.OnEnemyDestroyed += EnemyController_OnEnemyDestroyed;
        }

       private void EnemyController_OnEnemyDestroyed(EnemyController enemyController)
       {
           CurrentEnemyCount--;
           //Debug.Log("Current enemy count: " + CurrentEnemyCount);
           //Debug.Log("destroy: " + enemyController.gameObject);
           Destroy(enemyController.gameObject, 0.02f);
           if (CurrentEnemyCount == 0)
           {
               Invoke("CheckForGameWin", 0.01f);
               
           }
       }

        void CheckForGameWin()
       {
           //Debug.Log("Game state from enemy: " + GameManager.Instance.GameState);
           if (GameSessionInfoManager.Instance.playerInfoSession.playerHealth > 0)
               GameManager.Instance.GameWin();
       }

        void Start()
       {
        
           CurrentEnemyCount = transform.childCount;
           
       }
        void OnDestroy()
       {
           EnemyController.OnBoatActivityCompleted -= EnemyController_OnBoatActivityCompleted;
           EnemyController.OnEnemyDestroyed -= EnemyController_OnEnemyDestroyed;
           
       }

        int currentChangeTurn = 0;
        
        
       private void EnemyController_OnBoatActivityCompleted(BoatController boatController)
       {
           var boatType = boatController.GetType();
           
           //We just care about the normal and advance enemy controller, because the firing enemy controller's TURN will be controlled by the effect manager
          if (boatType == typeof(NormalEnemyController) || boatType == typeof(AdvanceEnemyController))
          {
              currentChangeTurn++;

              if (currentChangeTurn == transform.childCount)
              {
                  currentChangeTurn = 0;
                  OnAllEnemyActivityCompleted();
              }
          }
          else if (boatType == typeof(FiringEnemyController))
          {
              currentChangeTurn = 0;// this will be controled by the effect manager
          }
       }
    }
}

