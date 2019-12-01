﻿using System.Collections;
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


        [Header("Object References")]
        [SerializeField]
        protected GameObject isometricModel;
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

        protected void MoveAndRotate(Direction dir)
        {
            if (BoatState == BoatState.MoveAndRotate)
                return;

            //Get input: target position, target direction
            targetDirection = dir;
            targetPosition = (Vector2)transform.position + MapConstantProvider.Instance.TileSize * UtilMapHelpers.GetDirectionVector(targetDirection);

            if (moveAndRotateCR != null)
                StopCoroutine(moveAndRotateCR);
            moveAndRotateCR = StartCoroutine(CR_MoveAndRotate(targetPosition, targetDirection));

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

        IEnumerator CR_MoveAndRotate(Vector2 targetPos, Direction toDirection)
        {
            //Start moving and rotating the model
            BoatState = BoatState.MoveAndRotate;

            Vector2 startPos = transform.position;
            float deltaAngle = GetDeltaAngle(currentDirection, toDirection);

            Quaternion startRot = isometricModel.transform.localRotation;
            //NOTE: must multiply by the rotation to create a local space rotation
            Quaternion endRot = Quaternion.AngleAxis(-deltaAngle, modelUp) * isometricModel.transform.localRotation;

            float t = 0;
            while (t < moveAndRotateTime)
            {
                t += Time.deltaTime;
                float fraction = t / moveAndRotateTime;
                rb2D.MovePosition(Vector2.Lerp(startPos, targetPos, fraction));
                isometricModel.transform.localRotation = Quaternion.Lerp(startRot, endRot, fraction);
                yield return null;
            }

            //Update the current Direction
            currentDirection = toDirection;

            //Update boat state to idle after finishing moving and rotating
            BoatState = BoatState.Idle;

            //Fire the moved position event
            if (OnBoatMovedPosition != null)
                OnBoatMovedPosition(gameObject, transform.position);
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

            //Instantiate a skull represent the boat grave
            Instantiate(skullPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}


