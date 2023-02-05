using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void PreviewEndPoints(CardData cardData)
        {
            foreach (var treeScript in trees)
            {
                var endPointTuples = treeScript.PreviewEndPoints(cardData);

                foreach (var (endPoint, endPointPosition) in endPointTuples)
                {
                    var nearbyTree = GetTreeNearby(endPointPosition, treeScript);
                    if (nearbyTree != null)
                    {
                        Debug.Log("Found tree nearby ", endPoint);
                    }
                }
            }
        }

        public TreeScript GetTreeNearby(Vector3 position, TreeScript excludeTree = null)
        {
            foreach (var treeScript in trees.Where(r => r != excludeTree))
            {
                var data = App.instance.appData.data;
                if (Vector3.Distance(treeScript.transform.position, position) < data.TreeSnappingDistance)
                {
                    return treeScript;
                }
            }

            return null;
        }

        public void ShowEndPoints() => trees.ForEach(r => r.ShowEndPoints());

        public void HideEndPoints() => trees.ForEach(r => r.HideEndPoints());

        public void StopPreviewEndPoints() => trees.ForEach(r => r.StopPreviewEndPoints());
    }
}
