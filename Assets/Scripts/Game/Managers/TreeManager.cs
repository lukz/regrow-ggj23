using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots
{
    public class TreeManager : MonoBehaviour
    {
        private List<TreeScript> trees = new List<TreeScript>();
        
        void Start()
        {
            trees.AddRange(App.instance.field.GetComponentsInChildren<TreeScript>());
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
