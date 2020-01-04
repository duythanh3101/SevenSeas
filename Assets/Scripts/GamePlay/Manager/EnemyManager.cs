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
            
        }

        void Start()
       {
           CurrentEnemyCount = transform.childCount;
       }
        void OnDestroy()
       {
           EnemyController.OnBoatActivityCompleted -= EnemyController_OnBoatActivityCompleted;
           
       }

        int currentChangeTurn = 0;
        
       private void EnemyController_OnBoatActivityCompleted(BoatController boatController)
       {
           var boatType = boatController.GetType();
          if (boatType == typeof(NormalEnemyController) || boatType == typeof(AdvanceEnemyController))
          {
              currentChangeTurn++;

              if (currentChangeTurn == transform.childCount)
              {
                  currentChangeTurn = 0;
                  OnAllEnemyActivityCompleted();
              }
          }
       }

       public void UpdateEnemyCount()
       {
           CurrentEnemyCount--;
           if (CurrentEnemyCount <= 0 )
           {
               //Debug.Log(CurrentEnemyCount);
               
               if (PlayerInfoManager.Instance.playerInfoSession.playerHealth > 0)
                   GameManager.Instance.GameWin();
              
               CurrentEnemyCount = 0;
           }
       }
    }
}

