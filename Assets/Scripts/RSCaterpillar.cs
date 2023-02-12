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
        private Vector3 tmpCollider;
        private Vector3 fixCollider;

        public float _pushForce;
        public Animator animator;

        public float EngineForce
        {
            set { 
                _engineForce = value;
                if (transform.up.y > 0)
                {
                    animator.SetFloat("AnimSpeed", 10.0F * _engineForce);
                }
                else
                {
                    animator.SetFloat("AnimSpeed", -10.0F * _engineForce);
                }                
            }
        }

        public RSCaterpillar() { TypeName = "caterpillar";}

        public override void Start()
        {
            base.Start();
            _collider = GetComponent<MeshCollider>();
            _collisionPoints = new List<Vector3>();
            _rb = GetComponent<Rigidbody>();
            _engineForce = 0.0F;
            tmpCollider = new Vector3(1.3F, 1.3F, 2.6F);
            fixCollider = new Vector3(0, 0.1F, 0);
        }

        private void FixedUpdate()
        {
            foreach (var collisionPoint in _collisionPoints)
            {
                Vector3 direction = transform.position - collisionPoint;
                direction.Normalize();
                Vector3 ForceVector = new Vector3(0, 0, 0);
                if ((direction.x < 0.9F) & (direction.x > -0.9F))
                {
                    if (direction.y > 0.9F)
                    {
                        ForceVector = transform.forward;
                        ForceVector.y = 0;
                        ForceVector.Normalize();
                        if (!((_rb.velocity.magnitude >= _maxVelocity) &  // —корость уже больше максимальной
                            (Vector3.Dot(_rb.velocity.normalized, ForceVector) >= 0.9F)))// “€нем в ту же сторону
                        {
                            _rb.AddForceAtPosition(ForceVector * _pushForce * _engineForce, collisionPoint);
                        }
                    }
                    else if (direction.y < -0.9F)
                    {
                        ForceVector = -transform.forward;
                        ForceVector.y = 0;
                        ForceVector.Normalize();
                        if (!((_rb.velocity.magnitude >= _maxVelocity) &  // —корость уже больше максимальной
                            (Vector3.Dot(_rb.velocity.normalized, ForceVector) >= 0.9F)))// “€нем в ту же сторону
                        {
                            _rb.AddForceAtPosition(ForceVector * _pushForce * _engineForce, collisionPoint);
                        }
                    }
                    else if (direction.z < -0.9F)
                    {
                        ForceVector = transform.up;
                        ForceVector.y = 0;
                        ForceVector.Normalize();
                        if (!((_rb.velocity.magnitude >= _maxVelocity) &  // —корость уже больше максимальной
                            (Vector3.Dot(_rb.velocity.normalized, ForceVector) >= 0.9F)))// “€нем в ту же сторону
                        {
                            _rb.AddForceAtPosition(ForceVector * _pushForce * 0.2F * _engineForce, collisionPoint);
                        }
                    }
                    else if (direction.z > 0.9F)
                    {
                        ForceVector = -transform.up;
                        ForceVector.y = 0;
                        ForceVector.Normalize();
                        if (!((_rb.velocity.magnitude >= _maxVelocity) &  // —корость уже больше максимальной
                            (Vector3.Dot(_rb.velocity.normalized, ForceVector) >= 0.9F) ) )// “€нем в ту же сторону
                        {
                            _rb.AddForceAtPosition(ForceVector * _pushForce * 0.2F * _engineForce, collisionPoint);
                        }
                        
                    }
                } // if((direction.x in bounds)
            } //foreach (var collisionPoint in _collisionPoints)
        } // private void FixedUpdate()

        private void Update()
        {
            _collisionPoints.Clear();
            Collider[] colliders = Physics.OverlapBox(transform.position, tmpCollider, transform.rotation);

            foreach (var otherCollider in colliders)
            {
                if (_collider.GetInstanceID() == otherCollider.GetInstanceID())
                    continue;

                Vector3 direction;
                float distance;
                if (Physics.ComputePenetration(_collider,
                                               _collider.transform.position - fixCollider,
                                               _collider.transform.rotation,
                                               otherCollider,
                                               otherCollider.transform.position,
                                               otherCollider.transform.rotation,
                                               out direction,
                                               out distance))
                {
                    Terrain terrain = otherCollider.GetComponent<Terrain>();
                    if (terrain != null)
                    {
                        // otherCollider is a Terrain Collider                        
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
