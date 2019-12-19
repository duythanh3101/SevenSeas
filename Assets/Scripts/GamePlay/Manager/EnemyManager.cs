using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    public class EnemyManager : MonoBehaviour
    {

        public static event System.Action OnAllEnemyActivityCompleted;

       void Awake()
        {
            EnemyController.OnBoatActivityCompleted += EnemyController_OnBoatActivityCompleted;

        }
        void OnDestroy()
       {
           EnemyController.OnBoatActivityCompleted -= EnemyController_OnBoatActivityCompleted;
       }

        int currentChangeTurn = 0;
       private void EnemyController_OnBoatActivityCompleted(BoatController boatController)
       {
          if (boatController.GetType() == typeof(EnemyController))
          {
              currentChangeTurn++;

              if (currentChangeTurn == transform.childCount)
              {
                  currentChangeTurn = 0;
                  OnAllEnemyActivityCompleted();
              }
          }
       }

       
    }
}

