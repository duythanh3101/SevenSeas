using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    
    public class FiringEnemyController : AdvanceEnemyController
    {
        public bool CanFire { get; set; }

        [SerializeField]
        private AimAndFireCanonball firingSystem;

        protected override void Awake()
        {
            base.Awake();
            Type = ObjectType.FiringEnemy;
        }

        protected override void OnCompletedRotateAndMove()
        {
          

            if (BoatState == BoatState.Firing)
                return;

            BoatState = BoatState.Firing;
            CanFire =  firingSystem.FireCanonballs(currentDirection, false);
            firingSystem.boxCollider2D.enabled = true;

            BoatState = BoatState.Idle;
            OnBoatActivityCompleted(this);
           
        }
    }
}

