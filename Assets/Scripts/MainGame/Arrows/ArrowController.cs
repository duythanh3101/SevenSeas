using BaseSystems.Observer;
using MainGame.Arrows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class ArrowController : MonoBehaviour
    {
        public static event System.Action<Direction> OnArrowClicked = delegate { };

        [SerializeField]
        private Arrow arrow = null;

        private bool isMovable;
    
        // Use this for initialization
        protected virtual void Start()
        {
            isMovable = true;
        }

        private void OnMouseOver()
        {
            if (isMovable)
            {
                arrow.arrowSprite.gameObject.SetActive(true);
            }
        }

        private void OnMouseExit()
        {
            arrow.arrowSprite.gameObject.SetActive(false);
        }

        private void OnMouseDown()
        {
            if (isMovable)
            {
                arrow.arrowSprite.gameObject.SetActive(false);
                this.PostEvent(ObserverEventID.OnArrowDirectionClicked, arrow.direction);
                OnArrowClicked(arrow.direction);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Obstacle" || other.tag == "Edge")
            {
                isMovable = false;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Obstacle" || other.tag == "Edge")
            {
                isMovable = true;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.tag == "Obstacle" || other.tag == "Edge")
            {
                isMovable = false;
            }
        }
        private void OnEnable()
        {
            //To prevent when the arrow is deactive, some how the on trigger exit cant call, then we reset the arrow
            isMovable = true;
        }
    }
}