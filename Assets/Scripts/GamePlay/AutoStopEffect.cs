using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStopEffect : MonoBehaviour
{
    public  System.Action <GameObject> OnEffectStoped;

    [SerializeField]
    private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        //particle = GetComponent<ParticleSystem>();
        
        Invoke("StopEffect", particle.main.duration);

    }

    void StopEffect()
    {
        Destroy(gameObject);
        if (OnEffectStoped != null)
            OnEffectStoped(gameObject);

    }
}
