using Assets.Scripts.Extensions.Utils;
using BaseSystems.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class Player : LivingObject
    {
        #region Public Properties
        
        /// <summary>
        /// movement of player
        /// </summary>
        public PlayerMovement playerMovement;

        /// <summary>
        /// Player dectect some obstacles
        /// </summary>
        public PlayerTargetDetection playerTargetDetection;

        /// <summary>
        /// Player dectect some obstacles
        /// </summary>
        public PlayerTeleporter playerTeleporter;
        #endregion Public Properties

        #region Private Properties
        private bool isMovingAndRotating;
        private bool isTargetDecting;
        private bool isTeleporting;

        #endregion

        #region Private Properties

        /// <summary>
        /// Direction of player
        /// </summary>
        private Direction toDirection;
        
        /// <summary>
        /// Player is moving
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Crosshair is skiped 1 frame after player moving
        /// </summary>
        private bool isCrosshairSkiped;

        private Vector3 mousePositionClicked;

        #endregion Private Properties


        #region Mono Behaviour
        public void Start()
        {
            playerTeleporter = GameObject.FindObjectOfType<PlayerTeleporter>();
            playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
            playerTargetDetection = GameObject.FindObjectOfType<PlayerTargetDetection>();

            isMoving = false;
            isCrosshairSkiped = false;

            toDirection = Direction.East;

            this.RegisterListener(ObserverEventID.OnArrowDirectionClicked, (param) => OnArrowDirectionClicked((Direction)param));
        }


        private void Updated()
        {
            isMovingAndRotating = playerMovement.GetState();
            isTargetDecting = playerTargetDetection.GetState();
            isTeleporting = playerTeleporter.GetState();
        }

        public void FixedUpdate()
        {
            if (isMoving)
            {
                isCrosshairSkiped = true;
                playerMovement.MoveAndRotate(toDirection);
                isMoving = false;
            }

        }

        public void Update()
        {
            if (mousePositionClicked != Input.mousePosition && !isMoving && playerMovement.isFinished)
            {
                isCrosshairSkiped = false;
            }
        }
        #endregion Mono Behaviour


        #region Private Methods
        private void OnArrowDirectionClicked(Direction direction)
        {
            if (!isMoving)
            {
                // Store mouse position clicked
                mousePositionClicked = Input.mousePosition;

                // Get diretion of arrow
                toDirection = direction;

                // Arrow clicked and player is moved
                isMoving = true;
                
                isCrosshairSkiped = true;
            }
        }

        private void OnMouseOver()
        {
            if (isCrosshairSkiped || isMoving) return;

            if (!isMoving && !isCrosshairSkiped)
            {
                //new WaitForSeconds(10);
                //Debug.Log("hehehe" + transform.position);
                playerTargetDetection.CannonTargeting(toDirection);
                
            }
        }

        private void OnMouseExit()
        {
            if (!isMoving)
            {
                playerTargetDetection.ResetCrosshair();          
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Whirlpool")
            {
                //isTargetDecting = false;
                isCrosshairSkiped = true;
                //playerTeleporter = other.gameObject.GetComponent<PlayerTeleporter>();
                playerTeleporter.Teleport(gameObject);
                //isMoving = false;
                //isCrosshairSkiped = false;
                //isTargetDecting = false;

            }
        }

        #endregion Private Methods


        #region Override Methods

        protected override void Die()
        {
            this.PostEvent(ObserverEventID.OnGameOver);
            base.Die();
        }

        #endregion Override Methods

    }
}