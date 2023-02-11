using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RSRocket : MonoBehaviour
{
    private Rigidbody rb;

    public GameObject _explosion;
    public ParticleSystem _ps;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude > 0)
        {
            transform.forward = rb.velocity.normalized;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(_explosion, transform.position, Quaternion.identity);
        var ems = _ps.emission;
        ems.enabled = false;
        _ps.transform.parent = null;
        Destroy(gameObject);
    }
}
