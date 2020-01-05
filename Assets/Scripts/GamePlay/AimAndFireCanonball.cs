using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Extensions.Utils;

namespace SevenSeas
{
    public struct ShootDirection
    {
        public Vector2 leftSide;
        public Vector2 rightSide;
    }

    public class AimAndFireCanonball : MonoBehaviour
    {
        [Header("Object references")]
        [SerializeField]
        private GameObject canonBall;
      
        [Header("Debug")]
        [SerializeField]
        private bool drawTargetLine;

        [Header("Shooting detection configuration")]
        [SerializeField]
        private LayerMask shootingInteraction;
        
        [SerializeField]
        private float maxShootingTile = 3;

        private const float distanceThreshold = 0.001f;

        #region Cache value

        [HideInInspector]
        public BoxCollider2D boxCollider2D;

        //Structs
        private RaycastHit2D rightHit;
        private RaycastHit2D leftHit;
        private ShootDirection shootDirection;
        
        //Vector2
        protected Vector2 currentPosition;
        protected Vector2 leftTargetPosition;
        protected Vector2 rightTargetPosition;

        private Vector2 endRight;
        private Vector2 endLeft;

        #endregion 

        // Start is called before the first frame update
        protected virtual void Start()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        void OnDrawGizmos()
        {
            if (drawTargetLine)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(currentPosition, endRight);
                Gizmos.DrawLine(currentPosition, endLeft);
            }
        }

        public void FireCanonballs(Direction toDirection, bool doneTarget)
        {
            //Check if we have not targeted 
            if (!doneTarget)
                CanonTargeting(toDirection);

            boxCollider2D.enabled = false;

            FireCanonball(transform.position, leftTargetPosition);
            FireCanonball(transform.position,rightTargetPosition);

        }

        void FireCanonball(Vector2 startPos, Vector2 endPos)
        {
            if (endPos != currentPosition)
            {
                var canonballIns = Instantiate(canonBall, startPos, Quaternion.identity);
                EffectManager.Instance.SpawnEffect(EffectManager.Instance.canonFiring, startPos, Quaternion.identity);
                SoundManager.Instance.PlayFiringSound();
                canonballIns.GetComponent<CanonballController>().Launch(gameObject,endPos);
            }
        }

        public virtual void ResetData()
        {

        }

        public virtual void CanonTargeting(Direction toDirection)
        {
           currentPosition = transform.position;
           shootDirection = GetShootDirection(toDirection);

           //Add some threshold to make the casting line a bit longer to cast the edge 
           endLeft = currentPosition + shootDirection.leftSide * (maxShootingTile + distanceThreshold) * MapConstantProvider.Instance.TileSize;
           endRight = currentPosition + shootDirection.rightSide * (maxShootingTile  + distanceThreshold)* MapConstantProvider.Instance.TileSize;

            //Disable the box collider to prevent casting its self
           boxCollider2D.enabled = false;
           leftHit = Physics2D.Linecast(currentPosition, endLeft, shootingInteraction);
           rightHit = Physics2D.Linecast(currentPosition, endRight, shootingInteraction);

            //enable again
           boxCollider2D.enabled = true;

           //Check left side
           leftTargetPosition = GetTargetPosition(leftHit, endLeft, shootDirection.leftSide);

           //Check right side
           rightTargetPosition = GetTargetPosition(rightHit, endRight, shootDirection.rightSide);
        }

        private Vector2 GetTargetPosition(RaycastHit2D hit, Vector2 endPos,  Vector2 direction)
       {
          
           Vector2 target = Vector2.zero;
           if (hit.transform == null) //If there's nothing was hit
           {
               target = endPos;
           }
           else
           {
              
               //if this is obstacle
               if (hit.collider.CompareTag("Obstacle") || hit.collider.CompareTag("Teleporter"))
               {
                   //Return the previouse position base on the shoot direction
                   target = (Vector2)hit.transform.position - direction * MapConstantProvider.Instance.TileSize;
               }
               else if (hit.collider.CompareTag("Edge"))
               {
                   //If hit the edge
                   target = (Vector2)hit.point - direction * MapConstantProvider.Instance.TileSize;

               }
               else if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Player"))
               {
                   target = hit.transform.position;
                   //Debug.Log(hit.transform.position);
               }
           }

        //Make sure to round float to one digit after comma
           target.x = Mathf.Round(target.x * 10) / 10;
           target.y = Mathf.Round(target.y * 10) / 10;

           return target;
       }

       private ShootDirection GetShootDirection(Direction toDirection)
       {
           ShootDirection result;

           var moveDirection = UtilMapHelpers.GetDirectionVector(toDirection);

           //Calculate the length of move direction
           float shootsqrMagnitude = moveDirection.sqrMagnitude;

           float x, y;
           //Calculate the right side vector first
           if (moveDirection.x == 0)
           {
               y = 0;
               x = Mathf.Sqrt(shootsqrMagnitude - y * y);
           }
           else if (moveDirection.y == 0)
           {
               x = 0;
               y = Mathf.Sqrt(shootsqrMagnitude - x * x);
           }
           else
           {
               x = Mathf.Sqrt(shootsqrMagnitude / (1 + Mathf.Pow(moveDirection.x / moveDirection.y, 2)));
               y = -(moveDirection.x / moveDirection.y) * x;
           }

           result.leftSide = new Vector2((int)x, (int)y);
           result.rightSide = -result.leftSide;

           return result;
       }

        



    }

}
