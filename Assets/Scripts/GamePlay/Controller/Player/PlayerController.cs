using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using Assets.Scripts.Extensions.Utils;
using BaseSystems.Observer;

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
        private Coroutine respawnCR;
        private AnimationClip[] animClips;

        //bool
        private bool isTargeting = false;

        //Float
        private float sinkTime;
        private float riseUpTime;

        //Int
        public int currentPlayerHealth;
        private static readonly int SINK_TRIGGER = Animator.StringToHash("isSink");
        private static readonly int RISEUP_TRIGGER = Animator.StringToHash("isRiseUp");

        #endregion

        protected override  void Awake()
        {
            base.Awake();
            ArrowController.OnArrowClicked += ArrowController_OnArrowClicked;
            playerDetecter.RegisterHandler(this);
            Type = ObjectType.Player;
        }
      
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ArrowController.OnArrowClicked -= ArrowController_OnArrowClicked;
        }


        protected override void PlayMovementSound()
        {
            SoundManager.Instance.PlayPlayerMovementSound();
        }

        protected override void Start()
        {
            base.Start();
            InitValues();
        }

        protected override void TurnBasedSystemManager_BattleStateChanged(BattleState newState)
        {
            if (newState == BattleState.PlayerTurn)
            {
                if (BoatState != BoatState.Respawning)
                {
                    TogglePlayerInput(true);
                    BoatState = BoatState.Idle;
                    firingSystem.boxCollider2D.enabled = true;
                }
            }
            else if (newState == BattleState.EndBattle)
            {
                TogglePlayerInput(false);
            }
        }

        void InitValues()
        {
            currentPlayerHealth = GameSessionInfoManager.Instance.playerInfoSession.playerHealth;
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

        #region Input dectection callback event

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
            Observer.Instance.PostEvent(ObserverEventID.OnCantUndo);
        }

        public void OnPlayerTeleporting()
        {
            Teleport();
            Observer.Instance.PostEvent(ObserverEventID.OnCantUndo);
        }

        #endregion

        void ArrowController_OnArrowClicked(Direction dir)
        {
            TogglePlayerInput(false);
            MoveAndRotate(dir);

        }

        void CanonTargeting()
        {
            if (BoatState == BoatState.Idle && TurnBasedSystemManager.Instance.BattleState == BattleState.PlayerTurn)
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
            firingSystem.FireCanonballs(currentDirection,isTargeting);

            TogglePlayerInput(false);
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
            TogglePlayerInput(false);
           // arrowCollection.SetActive(false); //Disable input
            animator.SetTrigger(SINK_TRIGGER);
            SoundManager.Instance.PlaySinkSound();
            yield return new WaitForSeconds(sinkTime);

            //Really teleport
            isometricModel.SetActive(false);
            teleporter.Teleport(gameObject, true);

            //Wait some amount of time before rising up
            yield return new WaitForSeconds(1);

            //Show the gameobject
            isometricModel.SetActive(true);

            //Riseup time
            SoundManager.Instance.PlayRiseUpSound();
            animator.SetTrigger(RISEUP_TRIGGER);
            yield return new WaitForSeconds(riseUpTime);
            
            

            BoatState = BoatState.Idle;
            OnBoatActivityCompleted(this);

        }

        private void Respawn()
        {
            BoatState = BoatState.Respawning;

            if (respawnCR != null)
                StopCoroutine(respawnCR);
               
            respawnCR = StartCoroutine(CR_Respawn());

        }

        private WaitForSeconds respawnIntervalWait = new WaitForSeconds(0.35f);
        private IEnumerator CR_Respawn()
        {

            //Disable input
            TogglePlayerInput(false);
            //Layout another position
            isometricModel.SetActive(false);

            while (EffectManager.Instance.effectPlaying)
            {
                yield return null;
            }

            MapConstantProvider.Instance.SetRespawningPosition(gameObject);
            for (int i = 0; i < 2; i++ )
            {
                SoundManager.Instance.PlayRespawnSound();
                isometricModel.SetActive(true);
                
                yield return respawnIntervalWait;
                isometricModel.SetActive(false);
                yield return respawnIntervalWait;
            }

            SoundManager.Instance.PlayRespawnSound();
            isometricModel.SetActive(true);
            //Enable input
            TogglePlayerInput(true);
            BoatState = BoatState.Idle;
          
        }

        void TogglePlayerInput(bool isEnable)
        {

            arrowCollection.SetActive(isEnable);
        }

        private Coroutine delayDestroyCR;
        protected override void GetDestroy()
        {
            if (delayDestroyCR != null)
                StopCoroutine(delayDestroyCR);
            delayDestroyCR = StartCoroutine(CR_DelayDestroy());
        }

        IEnumerator CR_DelayDestroy()
        {
            if (BoatState == BoatState.Respawning || BoatState == BoatState.Destroyed)
                yield return null;

            //Debug.Log("player destroyed");
            BoatState = BoatState.Destroyed;

            //Effect and sound
            EffectManager.Instance.SpawnEffect(EffectManager.Instance.explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayDestroyShipSound();
            MapConstantProvider.Instance.SpawnUnitOnDestroyedObject(skullPrefab, transform.position, gameObject);

            currentPlayerHealth--;
            GameSessionInfoManager.Instance.UpdatePlayerHealth(currentPlayerHealth);

            //UI
            UIManager.Instance.DecreaseHealth(currentPlayerHealth);

          
            if (currentPlayerHealth > 0)
            {
                //wait for end of frame for updating enemy count, if Game is win, then we dont have to respawn
                yield return new WaitForSeconds(0.01f);
                var gameState = GameManager.Instance.GameState;
                if (gameState == GameState.Playing)
                    Respawn();
                else if (gameState == GameState.GameWin)
                {
                    TogglePlayerInput(false);
                    gameObject.SetActive(false);
                }
                
            }
            else
            {
                TogglePlayerInput(false);
                gameObject.SetActive(false);
                //Debug.Log(EnemyManager.Instance.CurrentEnemyCount);
                GameManager.Instance.GameLose();
            }
        }
        
    }
}


