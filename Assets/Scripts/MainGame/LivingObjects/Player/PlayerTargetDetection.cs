using Assets.Scripts.Extensions.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class ShootDirection
    {
        public Vector2 leftSide { get; set; }
        public Vector2 rightSide { get; set; }

        public ShootDirection()
        {
            leftSide = Vector2.zero;
            rightSide = Vector2.zero;
        }
        public ShootDirection(Vector2 leftSide, Vector2 rightSide)
        {
            this.leftSide = leftSide;
            this.rightSide = rightSide;
        }
    }

    public class PlayerTargetDetection : MonoBehaviour
    {
        public GameObject leftCrosshair;
        public GameObject rightCrosshair;
        public GameObject leftSide;
        public GameObject rightSide;

        public SpriteRenderer leftCrosshairSprite;
        public SpriteRenderer rightCrosshairSprite;

        public int maxShootingSquareDistance = 3;
        public LayerMask blockingLayer;

        private Direction playerDirection;
        private BoxCollider2D playerCollider;
        private float inverseMoveTime;
        private float modifiedSpeed;

        private Vector3 currentPosition;

        private bool isTargetDecting;

        //private bool isCrosshairMoving;
        private void Start()
        {
            playerCollider = GetComponent<BoxCollider2D>();
            modifiedSpeed = 1 / 0.2f;
            inverseMoveTime = 1f / 0.2f;

            isTargetDecting = false;

            currentPosition = transform.parent.position;
        }

       public void CanonTargeting(Direction toDirection)
        {
            isTargetDecting = true;

            currentPosition = transform.parent.position;
            var shootDirection = GetShootDirection(toDirection);

            Vector3 endRight = currentPosition + (Vector3)(shootDirection.rightSide * maxShootingSquareDistance);
            Vector3 endLeft = currentPosition + (Vector3)(shootDirection.leftSide * maxShootingSquareDistance);

            //Debug.DrawLine(transform.position, endRight, Color.red);
            //Debug.DrawLine(transform.position, endLeft, Color.red);

            var rightHit = Physics2D.Linecast(currentPosition, endRight, blockingLayer);
            var leftHit = Physics2D.Linecast(currentPosition, endLeft, blockingLayer);

            //Check left side
            Vector3 leftTargetPosition = GetTargetPosition(leftHit, endLeft, (Vector3)shootDirection.leftSide);

            //Check right side
            Vector3 rightTargetPosition = GetTargetPosition(rightHit, endRight, (Vector3)shootDirection.rightSide);

            //Set positoin and move crosshair
            SetPositionAndMoveCrosshair(leftTargetPosition, rightTargetPosition);

            isTargetDecting = false;
        }

        private ShootDirection GetShootDirection(Direction toDirection)
        {
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
            return new ShootDirection(new Vector2((int)x, (int)y), -new Vector2((int)x, (int)y));
        }

        public void ResetCrosshair()
        {
            leftCrosshair.transform.position = transform.position;
            rightCrosshair.transform.position = transform.position;

            leftCrosshairSprite.enabled = false;
            rightCrosshairSprite.enabled = false;
        }

        private void SetPositionAndMoveCrosshair(Vector3 leftPosition, Vector3 rightPosition)
        {
            //new WaitForSeconds(10);
            CrosshairMove(leftPosition, leftCrosshair, leftCrosshairSprite);
            CrosshairMove(rightPosition, rightCrosshair, rightCrosshairSprite);
        }

        private void CrosshairMove(Vector3 toPosition, GameObject crosshair, SpriteRenderer spriteCrosshair)
        {
            if ((toPosition - transform.position).magnitude <= Mathf.Epsilon) return;

            spriteCrosshair.enabled = true;
            crosshair.transform.position = Vector3.MoveTowards(crosshair.transform.position, toPosition, modifiedSpeed * Time.deltaTime);
        }
    
        private Vector3 GetTargetPosition(RaycastHit2D hit, Vector3 endPosition, Vector3 direction)
        {
            var target = Vector3.zero;
            //Check right side
            if (hit.transform == null)
            {
              return endPosition;
            }
            else
            {
                //If this is obstacle like island or edge
                if (hit.transform.tag == "Obstacle" || hit.transform.tag == "Edge" || hit.transform.tag == "Whirlpool")
                {
                    target = hit.transform.position - direction * CommonConstants.TILE_SIZE;
                }
                else if (hit.transform.tag == "Enemy")
                {
                    //Because when we change the z position, the speed will decrease, so we must recalculate the speed
                    target = hit.transform.position;

                    Vector3 AB = hit.transform.position;
                    Vector3 AC = new Vector3(hit.transform.position.x, hit.transform.position.y, -5);

                    modifiedSpeed = AC.magnitude * inverseMoveTime / AB.magnitude;

                    target = AC;

                }
                target.x = Mathf.Round(target.x * 10) / 10;
                target.y = Mathf.Round(target.y * 10) / 10;
            }


            return target;
        }

        public bool GetState()
        {
            return isTargetDecting;
        }
    }

}