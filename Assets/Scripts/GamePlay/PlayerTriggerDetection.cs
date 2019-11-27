using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenSeas
{
    [RequireComponent(typeof(AimFireCanonballWithCrosshair))]
    public class PlayerTriggerDetection : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {


        public interface IPlayerTriggerDectecter
        {
            void OnPlayerPointerClick();
            void OnPlayerPointerEnter();
            void OnPlayerPointerExit();
            void OnPlayerDestroyed();
        }


        private IPlayerTriggerDectecter detectionHandler;


        public void OnPointerEnter(PointerEventData eventData)
        {
            detectionHandler.OnPlayerPointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            detectionHandler.OnPlayerPointerExit();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            detectionHandler.OnPlayerPointerClick();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Projectile"))
            {
                detectionHandler.OnPlayerDestroyed();
            }
        }

        public void RegisterHandler(IPlayerTriggerDectecter playerHandler)
        {
            this.detectionHandler = playerHandler;
        }
    }
}

