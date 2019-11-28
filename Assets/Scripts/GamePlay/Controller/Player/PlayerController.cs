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

        [Header("Teleport")]
        [SerializeField]
        private Teleporter teleporter;

        #region Cache values
        //Structs, 
        private Coroutine teleCR;
        private AnimationClip[] animClips;

        //bool
        private bool isTargeting = false;

        //Float
        private float sinkTime;
        private float riseUpTime;

        //Int
        private static readonly int SINK_TRIGGER = Animator.StringToHash("isSink");
        private static readonly int RISEUP_TRIGGER = Animator.StringToHash("isRiseUp");

        #endregion

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

        protected override void Start()
        {
            base.Start();
            InitValues();
        }

        void InitValues()
        {
            animClips = animator.runtimeAnimatorController.animationClips;
            //Init data for animation
            foreach (var clip in animClips)
            {
                switch (clip.name)
                {
                    case "ModelSink": sinkTime = clip.length; break;
                    case "ModelRiseUp": riseUpTime = clip.length; break;
                }
            }
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

        void Teleport()
        {
            BoatState = BoatState.Teleporting;

            if (sinkTime == 0 || riseUpTime == 0)
                throw new UnityException("Animation for sink or rise up may not been setup correctly!");

            if (teleCR != null)
                StopCoroutine(teleCR);
            teleCR = StartCoroutine(CR_Teleport());
           
        }

        IEnumerator CR_Teleport()
        {
          
            //Sink time
            arrowCollection.SetActive(false); //Disable input
            animator.SetTrigger(SINK_TRIGGER);
            yield return new WaitForSeconds(sinkTime);

            //Really teleport
            isometricModel.SetActive(false);
            teleporter.Teleport(gameObject,true);

            //Wait some amount of time before rising up
            yield return new WaitForSeconds(1);

            //Show the gameobject
            isometricModel.SetActive(true);
            
            //Riseup time
            animator.SetTrigger(RISEUP_TRIGGER);
            yield return new WaitForSeconds(riseUpTime);
            arrowCollection.SetActive(true); //enable the input

            BoatState = BoatState.Idle;
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

        public void OnPlayerTeleporting()
        {
            Teleport();
        }
    }
}


