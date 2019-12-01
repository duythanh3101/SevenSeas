using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    public class EnemyController : BoatController
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Projectile"))
            {
                GetDestroy();
            }
        }
    }

}
