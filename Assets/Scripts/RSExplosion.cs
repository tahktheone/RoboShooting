using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSExplosion : MonoBehaviour
    {
        public float explosionRadius = 2f;
        public float explosionForce = 10f;
        public float duration = 2f;
        public float animationDuration = 2f;
        public float elapsedTime = 0f;
        public float explosiondamage = 1.0F;
        private Collider[] colliders;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime <= duration)
            { // (elapsedTime < duration)
                colliders = Physics.OverlapSphere(transform.position, explosionRadius);
                foreach (Collider collider in colliders)
                {
                    RSUnit unit = collider.GetComponent<RSUnit>();
                    if (unit == null)
                    {
                        Transform tp = collider.transform.parent;
                        if(tp!=null)
                            unit = tp.GetComponent<RSUnit>();
                    }
                    if (unit != null)
                    {
                        unit.Hp -= explosiondamage;
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            Vector3 explosionDirection = collider.transform.position - transform.position;
                            rb.AddForce(explosionDirection.normalized * explosionForce, ForceMode.Impulse);
                        }
                    }
                }
            } // (elapsedTime < duration)
            if (elapsedTime >= animationDuration)
            {
                Destroy(gameObject);
            }
        } // void Update()
    } // public class RSExplosion : MonoBehaviour
} // namespace RSGeneral