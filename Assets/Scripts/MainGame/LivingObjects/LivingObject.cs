using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class LivingObject : MonoBehaviour
    {
        protected bool isDead = false;

        public event Action OnDeath;

        protected virtual void Die()
        {
            isDead = true;
            if (OnDeath != null)
            {
                OnDeath();
            }
            Destroy(gameObject);
        }
    }
}


