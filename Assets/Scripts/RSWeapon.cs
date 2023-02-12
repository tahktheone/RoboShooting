using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSWeapon : RSUnit
    {
        public Transform stoika;
        public Transform dulo;
        public float rotationSpeed = 5.0f;

        private Quaternion stoikaTargetRotation;
        private Quaternion duloTargetRotation;

        public RSWeapon()
        {
            TypeName = "weapon";
        }

        public override void Start()
        {
            base.Start();
            SetDirection(transform.position + transform.up * 3.0F);
        }

        public void SetDirection(Vector3 direction)
        {
            // Calculate yaw and pitch from the direction vector
            Vector3 localDirection = transform.InverseTransformDirection(direction);
            float yaw = Mathf.Atan2(localDirection.x, localDirection.z);
            float pitch = -Mathf.Atan2(localDirection.y, Mathf.Sqrt(localDirection.x * localDirection.x + localDirection.z * localDirection.z));

            // Set target rotations for stoika and dulo
            stoikaTargetRotation = Quaternion.Euler(-90, yaw * Mathf.Rad2Deg, 0);
            duloTargetRotation = Quaternion.Euler(pitch * Mathf.Rad2Deg, yaw * Mathf.Rad2Deg, 0);
        }

        public override void Update()
        {
            base.Update();
            // Interpolate the rotation of stoika towards the target rotation
            stoika.transform.localRotation = Quaternion.Lerp(stoika.transform.localRotation, stoikaTargetRotation, Time.deltaTime * rotationSpeed);

            // Interpolate the rotation of dulo towards the target rotation
            dulo.transform.localRotation = Quaternion.Lerp(dulo.transform.localRotation, duloTargetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
