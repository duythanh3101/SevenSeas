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

        private int notEffectEnemyCount;
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
           notEffectEnemyCount = transform.childCount - MapConstantProvider.Instance.firingEnemyCount;
       }
        void OnDestroy()
       {
           EnemyController.OnBoatActivityCompleted -= EnemyController_OnBoatActivityCompleted;
           
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

       public void UpdateEnemyCount()
       {
           CurrentEnemyCount--;
           if (CurrentEnemyCount <= 0 )
           {

               
               if (PlayerInfoManager.Instance.playerInfoSession.playerHealth > 0)
                   GameManager.Instance.GameWin();
              
               CurrentEnemyCount = 0;
           }
       }
    }
}

