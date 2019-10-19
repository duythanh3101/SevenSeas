using Assets.Scripts.Extensions.Utils;
using MainGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Public Properties
        public GameObject isometricModel;
        public float rotateSpeed = 5f;
        public float movingTime = 0.1f;

        [HideInInspector]
        public bool isFinished;

        #endregion Public Properties


        #region Private Properties
        private Direction currentDirection;
        private float inverseMoveTime;
        private float currentAngle;

        private bool isMoving;
        private bool isRotating;
        #endregion Private Properties


        #region Mono Behaviour
        // Start is called before the first frame update
        void Start()
        {
            currentDirection = Direction.East;
            currentAngle = 0;

            inverseMoveTime = 1f / movingTime;

            isMoving = false;
            isRotating = false;
            isFinished = false;
        }

        private void Update()
        {
            isFinished = !isMoving && !isRotating;
        }

        #endregion Mono Behaviour


        #region Public Methods

        public bool GetState()
        {
            return isFinished;
        }

        public void MoveAndRotate(Direction toDirection)
        {
            if (!isMoving && !isRotating)
            {
                Rotate(toDirection);

                MovePosition(toDirection);
            }
        }
      
        public void MovePosition(Direction toDirection)
        {
            isMoving = true;
            Vector3 targetPosition = (Vector2)transform.position + CommonConstants.TILE_SIZE * UtilMapHelpers.GetDirectionVector(toDirection);

            float sqrRemainingDistance = (transform.position - targetPosition).sqrMagnitude;
            if (sqrRemainingDistance <= float.Epsilon)
            {
                isMoving = false;
                return;
            }

            StartCoroutine(SmoothMovement(targetPosition));
        }

        public IEnumerator SmoothMovement(Vector3 targetPosition)
        {
            float sqrRemainingDistance = (transform.position - targetPosition).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon && isMoving)
            {
                //Set the new position to the point inverseMove * Time.deltaTime units closer to the target position
                //Move towards the rigidboy's position to the target end
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, inverseMoveTime * Time.deltaTime);

                //Recalculate the remaning distance in order to check the condition to end the function
                sqrRemainingDistance = (transform.position - targetPosition).sqrMagnitude;
                if (sqrRemainingDistance <= float.Epsilon)
                {
                    isMoving = false;
                    yield return null;
                }

                //Return null for waiting a frame and loop until sqrRemainingDistance is close enough to zero to end the function
                yield return null;
            }
        }
        #endregion Public Methods


        #region Private Methods
        private void Rotate(Direction toDirection)
        {
            isRotating = true;
            float deltaAngle = GetDeltaAngle(currentDirection, toDirection);

            if (Mathf.Abs(deltaAngle) < Mathf.Epsilon)
            {
                isRotating = false;
                return;
            }

            StartCoroutine(SmoothRotate(deltaAngle));

            //Update current direction
            currentDirection = toDirection;
        }

        private IEnumerator SmoothRotate(float deltaAngle)
        {
            var turnDirection = -Mathf.Sign(deltaAngle);

            deltaAngle = Mathf.Abs(deltaAngle);
            while (Mathf.Abs(currentAngle - deltaAngle) > float.Epsilon)
            {
                currentAngle = Mathf.MoveTowards(currentAngle, deltaAngle, rotateSpeed);
                isometricModel.transform.Rotate(0, turnDirection * rotateSpeed, 0, Space.Self);
                yield return null;
            }

            if (Mathf.Abs(currentAngle - deltaAngle) <= float.Epsilon)
            {
                currentAngle = 0;
                isRotating = false;
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

        private float CalculateDeltaAngle(float currentDirection, float targetDirection)
        {
            float deltaAngle = targetDirection - currentDirection;

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
        #endregion  Private Methods
    }

}