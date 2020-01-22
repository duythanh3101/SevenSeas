using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;
using BaseSystems.Observer;

namespace SevenSeas
{
    public class EnemyController : BoatController
    {
        public static System.Action<EnemyController> OnEnemyDestroyed = delegate { };

        [SerializeField]
        protected int destroyedByBoatScore = 2;

        [SerializeField]
        protected int normalDestroyScore = 1;

        [Header("Debug")]
        [SerializeField]
        private bool drawRayToTarget = false;

        #region Cache values
        protected Vector2 offset;
        protected Vector2 direction;

        protected Transform targetTrans;
        private int destroyScore;
        #endregion
       

        void OnDrawGizmos()
        {
            if (drawRayToTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, offset * 5f);

                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, direction * 5f);
            }
        }

        protected override void TurnBasedSystemManager_BattleStateChanged(BattleState newState)
        {
            if (newState == BattleState.EnemyTurn)
            {
                if (BoatState == BoatState.Destroyed)
                    return;
                MoveAndRotate(CalculateNextDirection());
            }

        }

        bool destroyed = false;
        private void OnTriggerEnter2D(Collider2D other)
        {
           
            if (other.CompareTag("Projectile"))
            {
                
                GetDestroy();
            }
            else if (other.CompareTag("Obstacle"))
            {
                DestroyByObstacle();
            }
            else if (other.CompareTag("PlayerShip") || other.CompareTag("Enemy"))
            {
                DestroyByBoat();
            }

            
            if (BoatState == BoatState.Destroyed && !destroyed)
            {
                //Debug.Log("Update data score, collide with: " + other.tag );
                //Update data for result
                GameSessionInfoManager.Instance.UpdateScore(destroyScore);
                GameSessionInfoManager.Instance.UpdatePirateSunk();
                UIManager.Instance.ShowFloatingScorePoint(transform.position, destroyScore);

                destroyed = true;
                if (OnEnemyDestroyed != null)
                    OnEnemyDestroyed(this);
           
            }
               
        }

        
        protected override void PlayMovementSound()
        {
            SoundManager.Instance.PlayEnemyMovementSound();
        }

        protected override void GetDestroy()
        {
            destroyScore = normalDestroyScore;
            base.GetDestroy();
            
        }

        void DestroyByObstacle()
        {
            if (BoatState == BoatState.Destroyed)
                return;

            BoatState = BoatState.Destroyed;

            EffectManager.Instance.SpawnEffect(EffectManager.Instance.explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayDestroyShipSound();

            destroyScore = normalDestroyScore;
            Observer.Instance.PostEvent(ObserverEventID.OnCantUndo);

           
            isometricModel.SetActive(false);
            
        }

        void DestroyByBoat()
        {
            if (BoatState == BoatState.Destroyed)
                return;

            BoatState = BoatState.Destroyed;

            EffectManager.Instance.SpawnEffect(EffectManager.Instance.explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayDestroyShipSound();
            MapConstantProvider.Instance.SpawnUnitOnDestroyedObject(skullPrefab, transform.position, gameObject);

            destroyScore = destroyedByBoatScore;
            Observer.Instance.PostEvent(ObserverEventID.OnCantUndo);

            
            isometricModel.SetActive(false);
           
        }

        protected override void Start()
        {
            base.Start();
            targetTrans = FindObjectOfType<PlayerController>().transform;
        }


        protected virtual Direction CalculateNextDirection()
        {
            return Direction.East;
        }
    }

}
