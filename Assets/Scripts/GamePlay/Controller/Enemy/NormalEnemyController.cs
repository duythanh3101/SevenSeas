using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;

namespace SevenSeas
{
    public class NormalEnemyController : EnemyController
    {
        protected override Direction CalculateNextDirection()
        {
            offset = targetTrans.position - transform.position;

            //Find the min angle between the offset vector and eight direction
            float minAngle = Vector2.Angle(offset, CommonConstants.DIRECTION_VECTORS[0]);
            int minIndex = 0;
            for (int i = 1; i < CommonConstants.DIRECTION_VECTORS.Length; i++)
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
