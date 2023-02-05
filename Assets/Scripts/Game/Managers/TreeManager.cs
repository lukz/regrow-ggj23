using System.Collections;
using System.Collections.Generic;
using Roots.SObjects;
using UnityEngine;

namespace Roots
{
    public class TreeManager : MonoBehaviour
    {
        private List<TreeScript> trees = new List<TreeScript>();
        
        public bool IsWorking { get; private set; }
        
        void Start()
        {
            trees.AddRange(App.instance.field.GetComponentsInChildren<TreeScript>());

            foreach (var treeScript in trees)
            {
                treeScript.OnGrowthRequested += GrowTree;
            }
        }

        // TODO: Probably we should schedule growth through GameManager and handle it in trees turn 
        private void GrowTree(TreeScript tree, RootScript root, CardData card)
        {
            StartCoroutine(CO_GrowTree(tree, root, card));
        }

        private IEnumerator CO_GrowTree(TreeScript tree, RootScript root, CardData card)
        {
            IsWorking = true;

            yield return tree.Grow(root, card);
            
            // Check connected trees?
            
            IsWorking = false;
        }
        
    }
}
