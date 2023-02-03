using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSUnit : MonoBehaviour
    {
        private List<FixedJoint> allMyJoints = new List<FixedJoint>();
        public float _hp;
        public string _typeName;

        private float size = 2.0F;

        public float Hp
        {
            get { return _hp; }
            set { _hp = value; }
        }

        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        public void AddJoint(FixedJoint joint)
        {
            allMyJoints.Add(joint);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * size);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * size);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * size);
        }
    }
}