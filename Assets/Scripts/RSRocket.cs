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
        GameObject pex = Instantiate(_explosion, transform.position, Quaternion.identity);
        GameObject ex = pex.transform.GetChild(0).gameObject;
        ex.transform.parent = null;
        Destroy(pex);
        var mn = _ps.main;
        mn.loop = false;
        var ems = _ps.emission;
        ems.enabled = false;
        _ps.transform.parent = null;
        Destroy(gameObject);
    }
}
