using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Roots
{
    public class TreeGrower : MonoBehaviour
    {
        public GameObject[] leaves;
        private Vector3[] leafScales;

        void Awake()
        {
            leafScales = new Vector3[leaves.Length];
            for (int i = 0; i < leaves.Length; i++)
            {
                leafScales[i] = leaves[i].transform.localScale;
            }
        }
        
        [Button("Grow")]
        public void Grow(float duration = 1)
        {
            for (int i = 0; i < leaves.Length; i++)
            {
                leaves[i].transform.DOKill();
                leaves[i].transform.DOScale(leafScales[i], duration).SetDelay(Random.Range(0, duration/2));
            }
        }

        [Button("Shrink")]
        public void Shrink(float duration = 1)
        {
            if (duration == 0)
            {
                foreach (var leaf in leaves)
                {
                    leaf.transform.localScale = Vector3.zero;
                }
                return;
            }
            
            foreach (var leaf in leaves)
            {
                leaf.transform.DOKill();
                leaf.transform.DOScale(new Vector3(0, 0, 0), duration).SetDelay(Random.Range(0, duration/2));
            }
        }
    }
}
