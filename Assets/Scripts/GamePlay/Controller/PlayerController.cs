using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using Assets.Scripts.Extensions.Utils;

namespace SevenSeas
{
    public class PlayerController : BoatController, PlayerTriggerDetection.IPlayerTriggerDectecter
    {
        [Header("Aim and fire canonball")]
        [SerializeField]
        private GameObject arrowCollection;
        [SerializeField]
        private AimAndFireCanonball firingSystem;
        [SerializeField]
        private PlayerTriggerDetection playerDetecter;

        private bool isTargeting = false;
  
        void Awake()
        {
            ArrowController.OnArrowClicked += ArrowController_OnArrowClicked;
            EffectManager.OnAllEffectCompleted += EffectManager_OnAllEffectCompleted;

            playerDetecter.RegisterHandler(this);
        }
      
        void OnDestroy()
        {
            ArrowController.OnArrowClicked -= ArrowController_OnArrowClicked;
            EffectManager.OnAllEffectCompleted -= EffectManager_OnAllEffectCompleted;
        }

        void EffectManager_OnAllEffectCompleted()
        {
            BoatState = BoatState.Idle;
            arrowCollection.SetActive(true);
        }

        void ArrowController_OnArrowClicked(Direction dir)
        {
            MoveAndRotate(dir);
        }

        void CanonTargeting()
        {
            if (BoatState == BoatState.Idle)
            {
                isTargeting = true;
                firingSystem.CanonTargeting(currentDirection);
            }
               
        }

        void ResetCrosshair()
        {
            isTargeting = false;
            firingSystem.ResetData();
        }

        void FiringCanonballs()
        {
            //We have to wait until the firing complete - the splash or destroy effect completed - to enable the next boat activity
            if (BoatState == BoatState.Firing)
                return;

            BoatState = BoatState.Firing;
            firingSystem.ResetData();

            arrowCollection.SetActive(false);
            firingSystem.FireCanonballs(currentDirection, isTargeting);
        }


        public void OnPlayerPointerClick()
        {
            FiringCanonballs();
        }

        public void OnPlayerPointerEnter()
        {
            CanonTargeting();
        }

        public void OnPlayerPointerExit()
        {
            ResetCrosshair();
        }

        public void OnPlayerDestroyed()
        {
            GetDestroy();
        }
    }
}


