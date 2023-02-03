using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RSGeneral
{
    public class MainBehaviour : MonoBehaviour
    {
        public List<GameObject> prefabs;
        public float spawnDistance = 5f;
        private InputAction iaSpawnObject;

        public InputActionAsset inputActions;

        private void Awake()
        {
            iaSpawnObject = inputActions.FindAction("Spawn");
            iaSpawnObject.performed += ctx => SpawnObject();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnEnable()
        {
            iaSpawnObject.Enable();
        }

        private void OnDisable()
        {
            iaSpawnObject.Disable();
        }

        public GameObject InstantiatePrefab(string prefabName, Vector3 position, Quaternion rotation)
        {
            GameObject prefabToInstantiate = prefabs.Find(x => x.name == ("pr_" + prefabName));

            if (prefabToInstantiate != null)
            {
                GameObject CreatedPrefab = Instantiate(prefabToInstantiate, position, rotation);
                GameObject NewObject = CreatedPrefab.transform.GetChild(0).gameObject;
                NewObject.transform.parent = null;
                Destroy(CreatedPrefab);
                return NewObject;
            }
            else
            {
                Debug.LogError("Prefab not found: " + prefabName);
                return null;
            }
        }

        private void SpawnObject()
        {
            Transform _cameraTransform = GetComponentInChildren<Camera>().transform;
            Vector3 spawnPosition = _cameraTransform.position + _cameraTransform.forward * spawnDistance;
            GameObject go_cmp = InstantiatePrefab("computer", spawnPosition, Quaternion.identity);
            Vector3 connectorPosition = spawnPosition - go_cmp.transform.forward * 2;
            GameObject go_conn = InstantiatePrefab("connector", connectorPosition, Quaternion.identity);
            FixedJoint fixedJoint = go_cmp.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = go_conn.GetComponent<Rigidbody>();
            RSUnit rsUnit1 = go_conn.GetComponent<RSUnit>();
            rsUnit1.AddJoint(fixedJoint);
            RSUnit rsUnit2 = go_cmp.GetComponent<RSUnit>();
            rsUnit2.AddJoint(fixedJoint);
        }
    }
}