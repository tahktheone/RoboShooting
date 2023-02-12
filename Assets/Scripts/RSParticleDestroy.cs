using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSParticleDestroy : MonoBehaviour
{
    private ParticleSystem _ps;

    private void Start()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!_ps.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
