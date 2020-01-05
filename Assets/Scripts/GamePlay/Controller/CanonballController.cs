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
        private bool reachoutFiredFrom;
        private Vector2 endPosition;
        private CircleCollider2D collider2D;
        private Vector2 originPosition;

        [HideInInspector]
        public GameObject firedFrom;

        private float firedFromColliderX;
        private float circleColliderX;

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
                //Debug.Log("Other: " + other.tag + " fired from: " + firedFrom.tag);
                //if (other.tag != firedFrom.tag)
                {
                    if (other.CompareTag("Enemy"))
                    {
                        Destroy(gameObject);
                        isDestroyed = true;
                    }
                }
              
            }
        }

        public void Launch(GameObject firedFrom, Vector2 endPos)
        {
            launched = true;
            endPosition = endPos;
            this.firedFrom = firedFrom;
            firedFromColliderX = firedFrom.GetComponent<BoxCollider2D>().size.x / 2;

            collider2D = GetComponent<CircleCollider2D>();
            circleColliderX = collider2D.radius;

            //Debug.Log("Fired: " + firedFrom.GetComponent<BoxCollider2D>().size +  " collider: " + circleColliderX);
            collider2D.enabled = false;

            originPosition = transform.position;
        }

        void MoveToTarget()
        {
           
            transform.position = Vector2.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position,firedFrom.transform.position) >= (MapConstantProvider.Instance.TileSize.x - 0.2f)  && !reachoutFiredFrom )
            //if (Mathf.Abs(transform.position.x - firedFrom.transform.position.x + circleColliderX) > firedFromColliderX && !reachoutFiredFrom)
            {
                collider2D.enabled = true;
                reachoutFiredFrom = true;
            }
            
            //Debug.Log("canonbal: " + (((Vector2)transform.position - endPosition).sqrMagnitude < float.Epsilon));

            if (((Vector2)transform.position - endPosition).sqrMagnitude < float.Epsilon)
            
            {
                
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


