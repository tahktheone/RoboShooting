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
        private InputAction iaFire;
        private InputAction iaShowHP;

        public InputActionAsset inputActions;

        private void Awake()
        {
            iaSpawnObject = inputActions.FindAction("Spawn");
            iaSpawnObject.performed += ctx => SpawnObject();

            iaFire = inputActions.FindAction("Fire");
            iaFire.performed += ctx => Fire();

            iaShowHP = inputActions.FindAction("ToggleHPShow");
            iaShowHP.started += ctx => ShowHPOn();
            iaShowHP.canceled += ctx => ShowHPOff();
        }

        private void ShowHPOn()
        {
            RSGlobalData.Instance.setParameter("ShowHP", 1.0F);
        }

        private void ShowHPOff()
        {
            RSGlobalData.Instance.setParameter("ShowHP", 0);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Fire()
        {
            GameObject rocket = InstantiatePrefab("rocket",
                             GetComponentInChildren<Camera>().transform.position + 
                             GetComponentInChildren<Camera>().transform.forward * 0.5F +
                             GetComponentInChildren<Camera>().transform.right * 1.5F -
                             GetComponentInChildren<Camera>().transform.up * 0.3F,
                             Quaternion.LookRotation(GetComponentInChildren<Camera>().transform.forward +
                                                     GetComponentInChildren<Camera>().transform.up * 0.2F, 
                                                            GetComponentInChildren<Camera>().transform.up) );
            rocket.GetComponent<Rigidbody>().AddForceAtPosition(
                ( GetComponentInChildren<Camera>().transform.forward +
                  GetComponentInChildren<Camera>().transform.up * 0.2F ) * 8000.0F, 
                                                                rocket.transform.position);
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

        private void SpawnFromCode(Vector3 initialPosition,Transform iTransform, RSgenCode iCode)
        {
            bool WeContinue = true;
            int iteration = 0;
            List<Transform> TransformList = new List<Transform>();
            TransformList.Add(iTransform);
            Transform ct = TransformList[TransformList.Count - 1];

            while (WeContinue)
            {
                iteration++;

                string prefname = "connector";
                //Set prefname based on code
                switch (iCode.curGene(4))
                {
                    case 0:
                        prefname = "computer";
                        break;
                    case 1:
                        prefname = "connector";
                        break;
                    case 2:
                        prefname = "caterpillar";
                        break;
                    case 3:
                        prefname = "weapon";
                        break;
                    case 4:
                        prefname = "radar";
                        break;
                    default:
                        prefname = "connector";
                        break;
                }
                Vector3 direction = new Vector3(0, 0, 1);
                switch (iCode.curGene(5))
                {
                    case 0:
                        direction = ct.forward;
                        break;
                    case 1:
                        direction = -ct.forward;
                        break;
                    case 2:
                        direction = -ct.right;
                        break;
                    case 3:
                        direction = ct.right;
                        break;
                    case 4:
                        direction = ct.up;
                        break;
                    case 5:
                        direction = -ct.up;
                        break;
                    default:
                        direction = Vector3.zero;
                        break;
                }

                Vector3 rlook = new Vector3(0, 0, 1);
                switch (iCode.curGene(5))
                {
                    case 0:
                        rlook = ct.forward;
                        break;
                    case 1:
                        rlook = -ct.forward;
                        break;
                    case 2:
                        rlook = -ct.right;
                        break;
                    case 3:
                        rlook = ct.right;
                        break;
                    case 4:
                        rlook = ct.up;
                        break;
                    case 5:
                        rlook = -ct.up;
                        break;
                    default:
                        rlook = Vector3.zero;
                        break;
                }
                Vector3 rup = new Vector3(0, 0, 1);
                switch (iCode.curGene(5))
                {
                    case 0:
                        rup = ct.forward;
                        break;
                    case 1:
                        rup = -ct.forward;
                        break;
                    case 2:
                        rup = -ct.right;
                        break;
                    case 3:
                        rup = ct.right;
                        break;
                    case 4:
                        rup = ct.up;
                        break;
                    case 5:
                        rup = -ct.up;
                        break;
                    default:
                        rup = Vector3.zero;
                        break;
                }
                Quaternion rotation = Quaternion.LookRotation(rlook, rup);
                GameObject go_cmp;
                if (TransformList.Count > 1) // Для всех кроме первого
                {
                    go_cmp = InstantiatePrefab(prefname, ct.position + direction * 2, rotation);
                    FixedJoint fixedJoint = go_cmp.AddComponent<FixedJoint>();
                    fixedJoint.connectedBody = ct.gameObject.GetComponent<Rigidbody>();
                    RSUnit rsUnit1 = ct.gameObject.GetComponent<RSUnit>();
                    rsUnit1.AddJoint(fixedJoint);
                    RSUnit rsUnit2 = go_cmp.GetComponent<RSUnit>();
                    rsUnit2.AddJoint(fixedJoint);
                }
                else
                {   // Первый объект
                    go_cmp = InstantiatePrefab(prefname, initialPosition + direction * 2, rotation);
                }
                TransformList.Add(go_cmp.transform);
                try
                {
                    int lc = iCode.curGene();
                    ct = TransformList[TransformList.Count - 1 - lc];
                    if (ct == iTransform)
                    {
                        WeContinue = false;
                    }
                }
                catch
                {
                    WeContinue = false;
                }
                if (iteration > 100)
                {
                    WeContinue = false;
                }
            } // while (WeContinue)
        } // private void SpawnFromCode(Transform iTransform, RSGenCode iCode)

        private void SpawnObject()
        {
            // 0 - create computer
            // 0 - at forward
            // 0 - looking forward
            // 4 - upvector up
            // 0 - continue (to next block)
            // 1 - create connector
            // 1 - at backward
            // 4 - looking up
            // 1 - upvector back
            // 0 - continue
            // 4 - create radar
            // 0 - at forward
            // 0 - looking forward
            // 4 - upvector up
            // 1 - continue (minus one block radar)
            // 2 - create caterpillar
            // 2 - at left
            // 4 - looking down   - up
            // 1 - upvector forward   - back
            // 2 - continue (minus 2 blocks (cater and radar))
            // 2 - create caterpillar
            // 3 - at right
            // 4 - looking down   - up
            // 0 - upvector back - forward
            // 3 - continue (minus 3 blocks (cater cater and radar))
            // 3 - create weapon
            // 4 - at down
            // 0 - looking forward
            // 4 - upvector up
            // 10 - stop (exit cuz objects less than 10)
            //                               cmp       cnt       rdr       cp1       cp1       wpn
            RSgenCode lCode = new RSgenCode("0,0,0,4,0,1,1,4,1,0,4,0,0,4,1,2,2,4,1,2,2,3,4,0,3,3,4,0,4,10");
            SpawnFromCode(GetComponentInChildren<Camera>().transform.position + GetComponentInChildren<Camera>().transform.forward * spawnDistance,
                          GetComponentInChildren<Camera>().transform, 
                          lCode);            
        }
    }
}