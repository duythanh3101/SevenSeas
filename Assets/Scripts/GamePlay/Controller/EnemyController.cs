using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;

namespace SevenSeas
{
    public class EnemyController : BoatController
    {

        [Header("AI")]

        [Header("Debug")]
        [SerializeField]
        private bool drawRayToTarget = false;
        private Transform targetTrans;

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
        private void OnTriggerEnter2D(Collider2D other)
        {
            bool isDestroy = other.CompareTag("Projectile") || other.CompareTag("Obstacle") || other.CompareTag("PlayerShip") || other.CompareTag("Enemy");

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

            if (isDestroy)
                EnemyManager.Instance.UpdateEnemyCount();
        }


        void DestroyByObstacle()
        {
            BoatState = BoatState.Destroyed;

            EffectManager.Instance.SpawnEffect(EffectManager.Instance.explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayDestroyShipSound();

            Destroy(gameObject);
        }

        void DestroyByBoat()
        {
            BoatState = BoatState.Destroyed;

            EffectManager.Instance.SpawnEffect(EffectManager.Instance.explosion, transform.position, Quaternion.identity);
            SoundManager.Instance.PlayDestroyShipSound();
            MapConstantProvider.Instance.SpawnUnitOnDestroyedObject(skullPrefab, transform.position, gameObject);

            Destroy(gameObject);

            
        }

        Vector2 GetSnapPosition()
        {
            return MapConstantProvider.Instance.dynamicObjectDicts[gameObject] + MapConstantProvider.Instance.TileSize * UtilMapHelpers.GetDirectionVector(currentDirection);
        }

        protected override void Start()
        {
            base.Start();
            targetTrans = FindObjectOfType<PlayerController>().transform;
        }

        Vector2 offset;
        Vector2 direction;

        Direction CalculateNextDirection()
        {
             offset = targetTrans.position - transform.position;

            //Find the min angle between the offset vector and eight direction
             float minAngle = Vector2.Angle(offset, CommonConstants.DIRECTION_VECTORS[0]);
            int minIndex = 0;
            for (int i = 1; i < CommonConstants.DIRECTION_VECTORS.Length; i++ )
            {
                float angle = Vector2.Angle(offset, CommonConstants.DIRECTION_VECTORS[i]);
               
                if (angle <= minAngle)
                {
                    minAngle = angle;
                    minIndex = i;
                }
            }
            return UtilMapHelpers.VectorToDirection(CommonConstants.DIRECTION_VECTORS[minIndex]);
        }
    }

}
