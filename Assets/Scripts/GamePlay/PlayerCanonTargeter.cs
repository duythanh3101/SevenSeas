using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SevenSeas
{
    [RequireComponent(typeof(AimFireCanonballWithCrosshair))]
    public class PlayerCanonTargeter : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
    {
        
        private bool isTargeting = false;
        private AimAndFireCanonball firingSystem;
        private PlayerController playerController;

        // Start is called before the first frame update
        void Start()
        {
            firingSystem = GetComponent<AimAndFireCanonball>();
            playerController = GetComponentInParent<PlayerController>();
        }

        void Update()
        {
            //if (isTargeting)
            //{
            //    Debug.Log("Targeting");
            //}
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isTargeting = true;
            if (playerController.BoatState == BoatState.Idle)
                firingSystem.CanonTargeting(playerController.currentDirection);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isTargeting = false;
            firingSystem.ResetData();
        }
    }
}

