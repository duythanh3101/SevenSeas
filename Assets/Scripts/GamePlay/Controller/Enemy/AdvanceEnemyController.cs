using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;

namespace SevenSeas
{
    public class AdvanceEnemyController : EnemyController
    {
        protected override void Awake()
        {
            base.Awake();
            Type = ObjectType.AdvanceEnemy;
        }

        protected override Direction CalculateNextDirection()
        {
            offset = targetTrans.position - transform.position;

            //Init cache value
            Vector2[] directionVectors = CommonConstants.DIRECTION_VECTORS;
            int directionVectorsLength = directionVectors.Length;

            //Find the movable neighbor direction of enemy
            List<Vector2> movableDirection = new List<Vector2>();
            for (int i = 0 ; i < directionVectorsLength;i++)
            {
                if (IsMovable(directionVectors[i]))
                {
                    //Debug.Log("Movable direction: " + UtilMapHelpers.VectorToDirection(directionVectors[i]));
                    movableDirection.Add(directionVectors[i]);
                }
            }
          

            //Find the min direction in the movable direction vector list
            float minAngle = Vector2.Angle(offset, movableDirection[0]);
            int minIndex = 0;
            for (int i = 1; i < movableDirection.Count;i++)
            {
                float angle = Vector2.Angle(offset, movableDirection[i]);

                if (angle <= minAngle)
                {
                    minAngle = angle;
                    minIndex = i;
                }
            }

            //Convert vector to direction
            return UtilMapHelpers.VectorToDirection(movableDirection[minIndex]);
          
        }

        //Check for the position which enemy attend to move is movable (Obstacle or other Enemy boat placed on it)   
        private bool IsMovable(Direction dir)
        {
            //Calculate the attended move position
            Vector2 attendedMovePos = (Vector2) transform.position +  UtilMapHelpers.GetDirectionVector(dir)  * MapConstantProvider.Instance.TileSize;
            //Debug.Log("Attend direction: " + dir);
            return !MapConstantProvider.Instance.ContainsPosInUnitDictionary(attendedMovePos);
            
        }

        //Check for the position which enemy attend to move is movable (Obstacle or other Enemy boat placed on it)   
        private bool IsMovable(Vector2 dir)
        {
            Vector2 attendMovePos = (Vector2)transform.position + dir * MapConstantProvider.Instance.TileSize;
            //return !MapConstantProvider.Instance.ContainsPosInUnitDictionary(attendMovePos);
            return MapConstantProvider.Instance.ContainsInPossiblePositionIncludePlayer(attendMovePos);
        }
      
    }
}

