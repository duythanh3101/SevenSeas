using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;

namespace SevenSeas
{
    public class EnemyController : BoatController
    {
        [Header("AI")]
        [SerializeField]
        private Transform targetTrans;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Projectile"))
            {
                GetDestroy();
            }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                MoveAndRotate(CalculateNextDirection());
            }
        }

        Direction CalculateNextDirection()
        {
            Vector2 offset = targetTrans.position - transform.position;

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

            //Convert to that min direction
            currentDirection = UtilMapHelpers.VectorToDirection(CommonConstants.DIRECTION_VECTORS[minIndex]);
            return currentDirection;
        }
    }

}
