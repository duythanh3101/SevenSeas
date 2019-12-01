using BaseSystems.Observer;
using MainGame.Arrows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainGame
{
    public class ArrowController : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
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

        private bool CheckMovable(Collider2D other)
        {
            return other.CompareTag("Obstacle") || other.CompareTag("Edge") || other.CompareTag("Enemy");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CheckMovable(other))
            {
                isMovable = false;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (CheckMovable(other))
            {
                isMovable = true;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (CheckMovable(other))
            {
                isMovable = false;
            }
        }
        private void OnEnable()
        {
            //To prevent when the arrow is deactive, some how the on trigger exit cant call, then we reset the arrow
            isMovable = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
            if (isMovable && arrow.arrowSprite.gameObject.activeSelf)
            {
                arrow.arrowSprite.gameObject.SetActive(false);
                this.PostEvent(ObserverEventID.OnArrowDirectionClicked, arrow.direction);
                OnArrowClicked(arrow.direction);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isMovable)
            {
                arrow.arrowSprite.gameObject.SetActive(true);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            arrow.arrowSprite.gameObject.SetActive(false);
        }
    }
}