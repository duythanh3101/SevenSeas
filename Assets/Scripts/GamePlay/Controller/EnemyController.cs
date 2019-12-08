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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Projectile"))
            {
                GetDestroy();
            }
        }

        protected override void Start()
        {
            base.Start();
            targetTrans = FindObjectOfType<PlayerController>().transform;
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                MoveAndRotate(CalculateNextDirection());
            }
        }


        Vector2 offset;
        Vector2 direction;

        Direction CalculateNextDirection()
        {
             offset = targetTrans.position - transform.position;

            //Find the min angle between the offset vector and eight direction
            float minAngle = 0;
            int minIndex = 0;
            for (int i = 0; i < CommonConstants.DIRECTION_VECTORS.Length; i++ )
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
