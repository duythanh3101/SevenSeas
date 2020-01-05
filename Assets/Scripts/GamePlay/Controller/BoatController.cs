using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;

namespace SevenSeas
{
    public enum BoatState
    {
        Idle,
        MoveAndRotate,
        Firing,
        Teleporting,
        Respawning,
        Destroyed
    }

    public class BoatController : MonoBehaviour
    {
        public static System.Action<GameObject,Vector2> OnBoatMovedPosition;
        public static System.Action<GameObject, Vector2> OnSpawnSkull;
        public static System.Action<BoatController> OnBoatActivityCompleted = delegate { };

        [Header("Object References")]
        public GameObject isometricModel;
        
        [SerializeField]
        protected GameObject skullPrefab;

        [Header("Values Configuration")]
        [SerializeField]
        private float moveAndRotateTime = 0.3f;
        [SerializeField]
        private BoatState boatState = BoatState.Idle;

        //Public properties 
        public BoatState BoatState
        {
            get
            {
                return boatState;
            }
            protected set
            {
                boatState = value;
            }
        }

        #region Cache Value

        protected Rigidbody2D rb2D;
        protected Animator animator;

        //Vector
        private Vector2 targetPosition;
        private Vector3 modelUp;

        //Coroutine
        protected Coroutine moveAndRotateCR;

        //Directions
        private Direction targetDirection;
        public Direction currentDirection = Direction.East;
        #endregion

        protected virtual void Awake()
        {
            TurnBasedSystemManager.BattleStateChanged += TurnBasedSystemManager_BattleStateChanged;
        }

        protected virtual void  OnDestroy()
        {
            TurnBasedSystemManager.BattleStateChanged -= TurnBasedSystemManager_BattleStateChanged;
        }

        protected virtual void TurnBasedSystemManager_BattleStateChanged(BattleState newState)
        {
            
        }

        protected  void MoveAndRotate(Direction dir)
        {
            if (BoatState == BoatState.MoveAndRotate)
                return;

            //Get input: target position, target direction
            targetDirection = dir;
            targetPosition = (Vector2)transform.position + MapConstantProvider.Instance.TileSize * UtilMapHelpers.GetDirectionVector(targetDirection);

            PlayMovementSound();

            if (moveAndRotateCR != null)
                StopCoroutine(moveAndRotateCR);
            moveAndRotateCR = StartCoroutine(CR_MoveAndRotate(targetPosition, targetDirection, () => OnCompletedRotateAndMove()));
        }


        protected virtual void PlayMovementSound()
        {

        }

        protected virtual void OnCompletedRotateAndMove()
        {
            OnBoatActivityCompleted(this);
        }

        protected virtual void Start()
        {
            modelUp = isometricModel.transform.up;
            GetComponentValues();
        }
       
        void GetComponentValues()
        {
            rb2D = GetComponent<Rigidbody2D>();
            animator = isometricModel.transform.parent.GetComponent<Animator>();
        }

        IEnumerator CR_MoveAndRotate(Vector2 targetPos, Direction toDirection, System.Action completed = null)
        {
            //Start moving and rotating the model
            BoatState = BoatState.MoveAndRotate;
            var boxCollider = GetComponentInChildren<BoxCollider2D>();
            boxCollider.enabled = false;
            Vector2 startPos = transform.position;
            float deltaAngle = GetDeltaAngle(currentDirection, toDirection);

            Quaternion startRot = isometricModel.transform.localRotation;
            //NOTE: must multiply by the rotation to create a local space rotation
            Quaternion endRot = Quaternion.AngleAxis(-deltaAngle, modelUp) * isometricModel.transform.localRotation;

            //Fire the moved position event
            if (OnBoatMovedPosition != null)
                OnBoatMovedPosition(gameObject, targetPos); // This will update dictionary info when it's subscribed by the MapConstatnProvider

            float t = 0;
            while (t < moveAndRotateTime)
            {
                t += Time.deltaTime;
                float fraction = t / moveAndRotateTime;
                rb2D.MovePosition(Vector2.Lerp(startPos, targetPos, fraction));
                isometricModel.transform.localRotation = Quaternion.Lerp(startRot, endRot, fraction);
                yield return null;
            }

            ////Fire the moved position event
            //if (OnBoatMovedPosition != null)
            //    OnBoatMovedPosition(gameObject, targetPos); // This will update dictionary info when it's subscribed by the MapConstatnProvider

            boxCollider.enabled = true;

            //Update the current Direction
            currentDirection = toDirection;
            //Update boat state to idle after finishing moving and rotating
            BoatState = BoatState.Idle;

            //NOTE: After enable collider, we skip this frame to the box collider begin to check,because this boat can collide when box collider is enabled, check if the boat state is idle
            yield return new WaitForSeconds(0.1f);

            if (BoatState == BoatState.Idle)
            {
                if (completed != null)
                    completed();
            }
        }

        private float GetDeltaAngle(Direction currentDirection, Direction toDirection)
        {
            float angle = 0f;
            if (currentDirection == toDirection)
            {
                return angle;
            }

            angle = CalculateDeltaAngle(UtilMapHelpers.GetDirectionAngle(currentDirection), UtilMapHelpers.GetDirectionAngle(toDirection));

            return angle;
        }

        private float CalculateDeltaAngle(float currentAngle, float targetAngle)
        {
            float deltaAngle = targetAngle - currentAngle;

            //Check if |delta angle| is greater > 180
            if (Mathf.Abs(deltaAngle) >= 180)
            {
                //If true, then we reverse the turn direction with shotter delta angle
                if (deltaAngle <= 0)
                {
                    deltaAngle = deltaAngle + 360;
                }
                else
                {
                    deltaAngle = deltaAngle - 360;
                }
            }

            return deltaAngle;
        }

        protected virtual void GetDestroy()
        {
            BoatState = BoatState.Destroyed;

            //Effect and sound
            EffectManager.Instance.SpawnEffect(EffectManager.Instance.explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayDestroyShipSound();

            //SpawnSkull();
            MapConstantProvider.Instance.SpawnUnitOnDestroyedObject(skullPrefab, transform.position, gameObject);

            Destroy(gameObject);
        }

        protected void SpawnSkull()
        {
            //Instantiate a skull represent the boat grave
            var skull = Instantiate(skullPrefab, transform.position, Quaternion.identity);
            if (OnSpawnSkull != null)
                OnSpawnSkull(skull, skull.transform.position);
        }
    }
}


