using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    
    public class FiringEnemyController : AdvanceEnemyController
    {
        [SerializeField]
        private AimAndFireCanonball firingSystem;

        protected override void OnCompletedRotateAndMove()
        {
            FiringCanonballs();
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

