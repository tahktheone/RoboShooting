using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSRadar : RSUnit
    {
        public RSRadar()
        {
            TypeName = "radar";
        }

        public bool CheckUnitsInArea(Vector3 icenter, Vector3 isize, Quaternion irotation)
        {
            List<GameObject> objects = new List<GameObject>();
            Collider[] colliders = Physics.OverlapBox(icenter, isize, irotation);

            foreach (Collider col in colliders)
            {
                objects.Add(col.gameObject);
            }

            foreach (GameObject obj in objects)
            {
                if (obj.GetComponent<RSUnit>() != null)
                {
                    return true;
                }
            }

            return false;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            Vector3 lsize = new Vector3(10.0F,  // wide
                                        14.0F,  // forward 
                                        6.0F); // up
            //Gizmos.DrawWireCube(transform.position, lsize);
            Gizmos.matrix = Matrix4x4.TRS(transform.position - transform.up * 1.0F, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, lsize);
        }

        public Vector3 FindPlayer()
        {
            GameObject player = GameObject.Find("Player");
            if (player != null)
            {
                Vector3 direction = player.transform.position - transform.position;
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