using UnityEngine;
using System.Collections;

namespace SevenSeas
{
    public class CanonballController : MonoBehaviour
    {




        [SerializeField]
        private float moveSpeed = 3f;

        public bool isHited { get; set; }

        private bool launched = false;
        private Vector2 endPosition;

        private void Start()
        {
            isHited = false;
        }

        private void Update()
        {
            if (launched)
            {
                MoveToTarget();
            }

        }

        private bool isDestroyed = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag != "Arrow")
            {
                if (other.CompareTag("Enemy"))
                {
                    Destroy(gameObject);
                    isDestroyed = true;
                }
            }
        }

        public void Launch(Vector2 endPos)
        {
            launched = true;
            endPosition = endPos;
        }

        void MoveToTarget()
        {
            transform.position = Vector2.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);

            if (((Vector2)transform.position - endPosition).sqrMagnitude < float.Epsilon)
            {
                //Debug.Log("Reach far");
                if (!isDestroyed)
                {
                    var waterSplash = EffectManager.Instance.waterSplash;
                    EffectManager.Instance.SpawnEffect(waterSplash, endPosition, waterSplash.transform.rotation);
                }
                Destroy(gameObject);
            }

        }
    }
}


