using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledFX : MonoBehaviour
{
    ParticleSystem particle;
    private float elapsed;
    private float duration;

    private Transform objectPool;

    public Transform ObjectPool
    {
        set => objectPool = value;
    }


    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        duration = particle.main.duration;
    }

    private void OnEnable()
    {
        elapsed = 0f;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
        {
            this.gameObject.transform.SetParent(objectPool);
            this.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        gameObject.transform.position = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
    }
}
