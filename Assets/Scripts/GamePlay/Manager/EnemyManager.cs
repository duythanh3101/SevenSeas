using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    public class EnemyManager : MonoBehaviour
    {
        public static System.Action OnAllEnemyActivityCompleted;

        private GameObject[] enemies;

        void Awake()
        {
            EnemyController.OnBoatActivityCompleted += EnemyController_OnActivityCompleted;
        }

        void OnDestroy()
        {
            EnemyController.OnBoatActivityCompleted -= EnemyController_OnActivityCompleted;
        }

        int currentEnemyChangeTurn = 0;

       
        private void EnemyController_OnActivityCompleted(BoatController obj)
        {
            if (obj.GetType() == typeof(EnemyController))
            {
                currentEnemyChangeTurn++;
                //Debug.Log("Current enemy change turn: " + currentEnemyChangeTurn + " enemy count: " + transform.childCount);
                if (currentEnemyChangeTurn == transform.childCount)
                {
                    currentEnemyChangeTurn = 0;
                    if (OnAllEnemyActivityCompleted != null)
                        OnAllEnemyActivityCompleted();
                }
            }
        }
    }
}

