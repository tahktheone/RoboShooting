using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSCaterpillar : RSUnit
    {
        private MeshCollider _collider;
        private List<Vector3> _collisionPoints;
        private Rigidbody _rb;
        private float _maxVelocity = 20.0f;
        private float _engineForce;

        public float _pushForce;

        public float EngineForce
        {
            set { _engineForce = value; }
        }

        public RSCaterpillar() { TypeName = "caterpillar";}

        private void Start()
        {
            _collider = GetComponent<MeshCollider>();
            _collisionPoints = new List<Vector3>();
            _rb = GetComponent<Rigidbody>();
            _engineForce = 1.0F;
        }

        private void FixedUpdate()
        {
            foreach (var collisionPoint in _collisionPoints)
            {
                Vector3 direction = transform.position - collisionPoint;
                direction.Normalize();
                if ((direction.x < 0.9F) & (direction.x > -0.9F))
                {
                    if (direction.y > -0.5F)
                    {
                        _rb.AddForceAtPosition(transform.forward * _pushForce * _engineForce, collisionPoint); 
                    }
                    else if (direction.y < 0.5F)
                    {
                        _rb.AddForceAtPosition(-transform.forward * _pushForce * _engineForce, collisionPoint);
                    }
                    else if (direction.z < 0.8F)
                    {
                        _rb.AddForceAtPosition(transform.up * _pushForce * 0.2F * _engineForce, collisionPoint);
                    }
                    else if (direction.z > -0.8F)
                    {
                        _rb.AddForceAtPosition(-transform.up * _pushForce * 0.2F * _engineForce, collisionPoint);
                    }
                } // if((direction.x < 0.9F) &(direction.x > -0.9F))
            } //foreach (var collisionPoint in _collisionPoints)

            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);

        } // private void FixedUpdate()

        private void Update()
        {
            _collisionPoints.Clear();
            Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(1.3F, 1.3F, 2.6F), transform.rotation);

            foreach (var otherCollider in colliders)
            {
                if (_collider.GetInstanceID() == otherCollider.GetInstanceID())
                    continue;

                Vector3 direction;
                float distance;
                if (Physics.ComputePenetration(_collider,
                                               _collider.transform.position - new Vector3(0, 0.1F, 0),
                                               _collider.transform.rotation,
                                               otherCollider,
                                               otherCollider.transform.position,
                                               otherCollider.transform.rotation,
                                               out direction,
                                               out distance))
                {
                    if (otherCollider.GetComponent<Terrain>() != null)
                    {
                        // otherCollider is a Terrain Collider                        
                        Terrain terrain = otherCollider.GetComponent<Terrain>();
                        float terrainHeight = terrain.SampleHeight(transform.position);
                        Vector3 collisionPointB = _collider.transform.position;
                        collisionPointB.y = terrainHeight + otherCollider.transform.position.y;
                        Vector3 closestPointA = _collider.ClosestPoint(collisionPointB);
                        Vector3 collisionPoint = (closestPointA + collisionPointB) / 2f;
                        _collisionPoints.Add(collisionPoint);
                    }
                    else
                    {
                        // otherCollider is not a Terrain Collider
                        Vector3 closestPointA = _collider.ClosestPoint(otherCollider.transform.position);
                        Vector3 closestPointB = otherCollider.ClosestPoint(_collider.transform.position);
                        Vector3 collisionPoint = (closestPointA + closestPointB) / 2f;
                        _collisionPoints.Add(collisionPoint);
                    }
                } // if (Physics.ComputePenetration(_collider, _collider.transfo .....
            } // foreach (var otherCollider in colliders)
        } //private void Update()

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            Gizmos.color = Color.white;
            foreach (var collisionPoint in _collisionPoints)
            {
                Gizmos.DrawSphere(collisionPoint, 0.1f);
            }

            Gizmos.color = Color.yellow;
            Vector3 endPoint = transform.position + transform.up * 2.0F + transform.forward * _engineForce * 2.0F;
            Gizmos.DrawLine(transform.position + transform.up * 2.0F, endPoint);
            Gizmos.DrawSphere(endPoint, 0.1f);
        }
    }



}
