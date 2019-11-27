using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectManager : MonoBehaviour
{

    public static event Action OnAllEffectCompleted;

    public static EffectManager Instance = null;

    [Header("Effects")]
    public GameObject waterSplash;
    public GameObject canonFiring;
    public GameObject explosion;

    private Dictionary<GameObject, Action<GameObject>> effectStopHandlers = new Dictionary<GameObject, Action<GameObject>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
            DestroyImmediate(gameObject);
    }

    public void SpawnEffect(GameObject effect, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (effect == null)
        {
            throw new UnityException("The effect you're trying to spawn has not been assigned!");
        }
        
        AutoStopEffect stopEffect = Instantiate(effect,position,rotation).GetComponent<AutoStopEffect>();
        stopEffect.OnEffectStoped += AutoStopEffect_OnEffectStoped;
        AddNewEffectHandler(stopEffect.gameObject,stopEffect.OnEffectStoped);

        
    }

    void AddNewEffectHandler(GameObject effect, Action<GameObject> callBack)
    {
        if (effectStopHandlers.ContainsKey(effect))
            return;

        effectStopHandlers.Add(effect, callBack);

    }

    void AutoStopEffect_OnEffectStoped(GameObject effect)
    {
        if (!effectStopHandlers.ContainsKey(effect))
            return;

        effectStopHandlers.Remove(effect);
        if (effectStopHandlers.Count == 0)
            OnAllEffectCompleted();
    }

}
