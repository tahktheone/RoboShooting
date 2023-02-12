using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSRadar : RSUnit
    {
        private Vector3 _up;
        private Vector3 _ssize;

        public RSRadar()
        {
            TypeName = "radar";
        }

        public override void Start()
        {
            base.Start();
            _up = new Vector3(0,2.0F,0);
            _ssize = new Vector3(4.0F,  // wide
                                        3.5F,  // forward 
                                        3.0F); // up
        }

        public bool CheckUnitsForward(List<RSUnit> ichecklist)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position + transform.up * 4.0F, _ssize, transform.rotation);

            foreach (Collider col in colliders)
            {
                RSUnit u = col.gameObject.GetComponent<RSUnit>();
                if ((u != null) & (!ichecklist.Contains(u))) // all RSUnits that not in checklist
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckUnitsBackward(List<RSUnit> ichecklist)
        {
            Collider[] colliders = Physics.OverlapBox(transform.position - transform.up * 4.0F, _ssize, transform.rotation);

            foreach (Collider col in colliders)
            {
                RSUnit u = col.gameObject.GetComponent<RSUnit>();
                if ((u != null) & (!ichecklist.Contains(u))) // all RSUnits that not in checklist
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            
            Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.up * 4.0F, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _ssize * 2.0F);

            Gizmos.color = Color.yellow;

            Gizmos.matrix = Matrix4x4.TRS(transform.position - transform.up * 4.0F, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, _ssize * 2.0F);
        }

        public Vector3 FindPlayer()
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Vector3 direction = player.transform.position + _up - transform.position;
                return direction.normalized;
            }
            else
            {
                return new Vector3(0,1,0);
            }
        }

        public float DistanceToPlayer()
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                float distance = (player.transform.position - transform.position).magnitude;
                return distance;
            }
            else
            {
                return 0.0F;
            }
        }

    }


}