using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RSGeneral
{
    public class RSGlobalData : MonoBehaviour
    {
        public static RSGlobalData Instance { get; private set; }
        public Dictionary<string, float> parameters = new Dictionary<string, float>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            setParameter("ShowHP", 0);
        }

        public void setParameter(string name, float value)
        {
            parameters[name] = value;
        }

        public float getParameter(string name)
        {
            return parameters.ContainsKey(name) ? parameters[name] : 0.0f;
        }
    }
}
