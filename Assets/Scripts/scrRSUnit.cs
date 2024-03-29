using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSgenCode
    {
        string genCodeString = "0";
        List<int> genCodeList = new List<int>();
        int curIndex = 0;

        public RSgenCode(string input) {
            SetGenCodeString(input);
            curIndex = 0;
        }
        ~RSgenCode() { }

        public void incIndex()
        {
            curIndex++;
            if(curIndex >= genCodeList.Count)
            {
                curIndex = 0;
            }
        }

        public int curGene(int limit = 0)
        {
            int code = genCodeList[curIndex];
            if(limit > 0)
            {
                while (code > limit)
                {
                    code -= limit;
                }
            }
            incIndex();
            return code;
        }

        private void _decode()
        {
            //Check if genCodeString is valid
            if (genCodeString.Length > 0)
            {
                //Split the string variable on the character ','
                string[] genCodeArray = genCodeString.Split(',');

                //Iterate through the string array and add each item to the list of integers 
                foreach (string code in genCodeArray)
                {
                    if (int.TryParse(code, out int result))
                    {
                        genCodeList.Add(result);
                    }
                }
            }
        }

        public void SetGenCodeString(string inputString)
        {
            //Check if input string is valid
            if (inputString.Length > 0)
            {
                genCodeString = inputString;
                _decode();
                curIndex = 0;
            }
        }

    }

    public class RSUnit : MonoBehaviour
    {
        private List<FixedJoint> allMyJoints = new List<FixedJoint>();
        public float _hp;
        public string _typeName;
        public GameObject prefabDestroyed;
        public GameObject prefabExplosion;
        private Renderer _renderer;

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

        public void DeleteJoint(FixedJoint joint)
        {
            allMyJoints.Remove(joint);
        }

        public virtual void Start()
        {
            _renderer = GetComponent<Renderer>();
            if(_renderer == null)
            {
                _renderer = transform.GetChild(0).GetComponent<Renderer>();
            }
        }

        public virtual void Update()
        {
            if(_hp <= 0)
            {
                DestroyUnit(true);
            }
            else
            {
                if (allMyJoints.Count == 0)
                {
                    DestroyUnit(false);
                }
            }
        }

        public virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * size);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * size);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * size);

            /*
            //get parent position
            if (transform.parent != null)
            {
                var parentPosition = transform.parent.position;

                Gizmos.color = Color.cyan;
                //draw line from current position to parent position
                Gizmos.DrawLine(transform.position, parentPosition);
                //draw big arrow from current position to parent position
                Gizmos.DrawLine(transform.position, parentPosition);
                Gizmos.DrawLine(parentPosition, parentPosition + Vector3.up * 0.5f);
                Gizmos.DrawLine(parentPosition, parentPosition + Vector3.right * 0.5f);
                Gizmos.DrawLine(parentPosition, parentPosition + Vector3.back * 0.5f);
                Gizmos.DrawLine(parentPosition, parentPosition + Vector3.left * 0.5f);
                Gizmos.DrawLine(parentPosition, parentPosition + Vector3.down * 0.5f);
            } */
        }

        public void DestroyUnit(bool withExplosion)
        {
            foreach (Transform child in transform)
            {
                // Detach children from parent
                if (child.gameObject.GetComponent<RSUnit>() != null)
                    child.parent = null;
            }
            transform.parent = null;

            GameObject prefabInstance = Instantiate(prefabDestroyed, gameObject.transform.position, gameObject.transform.rotation);
            Transform[] children = prefabInstance.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                child.parent = null;
            }

            foreach (FixedJoint joint in allMyJoints)
            {
                RSUnit connectedUnit;
                connectedUnit  = joint.connectedBody.gameObject.GetComponent<RSUnit>();
                if ((connectedUnit != null)&(connectedUnit!=this))
                {
                    connectedUnit.DeleteJoint(joint);
                }
                connectedUnit = joint.gameObject.GetComponent<RSUnit>();
                if ((connectedUnit != null)&(connectedUnit != this))
                {
                    connectedUnit.DeleteJoint(joint);
                }
                Destroy(joint);
            }
            allMyJoints.Clear();

            if (withExplosion)
            {
                GameObject ex = Instantiate(prefabExplosion, gameObject.transform.position, gameObject.transform.rotation);
                ex.transform.GetChild(0).parent = null;                
                Destroy(ex);
            }
            Destroy(gameObject);
        }

        private void OnGUI()
        {
            if (_renderer.isVisible & RSGlobalData.Instance.getParameter("ShowHP") > 0.0F)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
                float dist = (Camera.main.transform.position - transform.position).magnitude;
                float screenSize = 1.0F / (Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad) * (dist / Screen.height));
                float rectangleWidth = screenSize * (_hp / 100.0F);
                
                Rect maxRect = new Rect(screenPos.x - screenSize / 2.0F - 1, Screen.height - screenPos.y - screenSize / 2.0F - 1, screenSize + 2, 6);
                GUI.color = Color.grey;
                GUI.DrawTexture(maxRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true);

                Rect rect = new Rect(screenPos.x - screenSize / 2.0F, Screen.height - screenPos.y - screenSize / 2.0F, rectangleWidth, 4);
                GUI.color = Color.red;
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
            }
        }

        public List<RSUnit> GetConnectedRSUnits()
        {
            List<RSUnit> connectedUnits = new List<RSUnit>();
            GetConnectedRSUnits(connectedUnits);
            return connectedUnits;
        }

        private void GetConnectedRSUnits(List<RSUnit> connectedUnits)
        {
            foreach (FixedJoint joint in allMyJoints)
            {
                RSUnit connectedUnit = joint.GetComponent<RSUnit>();
                if (connectedUnit != null && !connectedUnits.Contains(connectedUnit))
                {
                    connectedUnits.Add(connectedUnit);
                    connectedUnit.GetConnectedRSUnits(connectedUnits);
                }
            }
        }
    }
}