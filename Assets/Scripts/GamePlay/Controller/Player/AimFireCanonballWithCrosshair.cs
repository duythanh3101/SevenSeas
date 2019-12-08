using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    public class AimFireCanonballWithCrosshair : AimAndFireCanonball
    {
        [Header("Crosshair config")]
        [SerializeField]
        private GameObject leftCrosshair;
        [SerializeField]
        private GameObject rightCrosshair;

        [SerializeField]
        private float crosshairMoveSpeed = 7.5f;

        #region Cache value
        //Components
        private SpriteRenderer leftCrosshairSprite;
        private SpriteRenderer rightCrosshairSprite;


        //Structs
        private Coroutine leftCrosshairCR;
        private Coroutine rightCrosshairCR;

        #endregion

        protected override void Start()
        {
            base.Start();
            //crosshairMoveSpeed = 1 / crosshairMoveTime;
            leftCrosshairSprite = leftCrosshair.GetComponent<SpriteRenderer>();
            rightCrosshairSprite = rightCrosshair.GetComponent<SpriteRenderer>();
        }

        public  override void ResetData()
        {
            leftCrosshair.transform.position = transform.position;
            rightCrosshair.transform.position = transform.position;

            DisplayCrosshair(leftCrosshairSprite, false);
            DisplayCrosshair(rightCrosshairSprite, false);

            if (rightCrosshairCR != null)
                StopCoroutine(rightCrosshairCR);

            if (leftCrosshairCR != null)
                StopCoroutine(leftCrosshairCR);

            
        }

        void SetCrosshairPosition(Vector2 leftTargetPos, Vector2 rightTargetPos)
        {
            leftCrosshair.transform.position = leftTargetPos;
            rightCrosshair.transform.position = rightTargetPos;

            //DisplayCrosshairs(true);
            
        }

        public override void CanonTargeting(Direction toDirection)
        {
            base.CanonTargeting(toDirection);

            SetAndMoveCrosshairPosition(leftTargetPosition, rightTargetPosition);
        }

        void SetAndMoveCrosshairPosition(Vector2 leftTargetPos, Vector2 rightTargetPos)
        {
            if (leftTargetPos != currentPosition)
            {
                DisplayCrosshair(leftCrosshairSprite, true);
                if (leftCrosshairCR != null)
                    StopCoroutine(leftCrosshairCR);
                leftCrosshairCR = StartCoroutine(CR_MoveCrosshair(leftCrosshair.transform, leftTargetPos));

            }

            if (rightTargetPos != currentPosition)
            {
                DisplayCrosshair(rightCrosshairSprite, true);
                if (rightCrosshairCR != null)
                    StopCoroutine(rightCrosshairCR);
                rightCrosshairCR = StartCoroutine(CR_MoveCrosshair(rightCrosshair.transform, rightTargetPos));
            }
        }

        

        IEnumerator CR_MoveCrosshair(Transform crosshairTrans, Vector2 endPos)
        {
            Vector2 curPos = crosshairTrans.transform.position;
            float remaningDistance = Vector2.Distance(curPos, endPos);
            while (remaningDistance > float.Epsilon)
            {
                curPos = Vector2.MoveTowards(curPos, endPos, crosshairMoveSpeed * Time.deltaTime);
                crosshairTrans.transform.position = curPos;
                remaningDistance = Vector2.Distance(curPos, endPos);
                yield return null;
            }
        }

        void DisplayCrosshair(SpriteRenderer crosshair, bool isShowing)
        {
            crosshair.enabled = isShowing;
        }
    }
}

