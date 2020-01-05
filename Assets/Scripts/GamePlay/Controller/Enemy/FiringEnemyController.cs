using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    
    public class FiringEnemyController : AdvanceEnemyController
    {
        [SerializeField]
        private AimAndFireCanonball firingSystem;

        public FiringEnemyController()
        {
            Type = ObjectType.FiringEnemy;
        }

        protected override void OnCompletedRotateAndMove()
        {
            FiringCanonballs();


            firingSystem.boxCollider2D.enabled = true;
            BoatState = BoatState.Idle;
           
            OnBoatActivityCompleted(this);
           
        }

        void FiringCanonballs()
        {
            if (BoatState == BoatState.Firing)
                return;

            BoatState = BoatState.Firing;
            firingSystem.FireCanonballs(currentDirection, false);
        }
    }
}

