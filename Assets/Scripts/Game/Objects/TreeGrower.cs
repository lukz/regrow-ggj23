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
        public GameObject trunk;
        public GameObject[] leaves;
        private Vector3[] leafScales;
        public bool startGrown = false;

        void Awake()
        {
            leafScales = new Vector3[leaves.Length];
            for (int i = 0; i < leaves.Length; i++)
            {
                leafScales[i] = leaves[i].transform.localScale;
            }
            if (!startGrown)
            {
                Shrink(0);
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
            foreach (var leaf in leaves)
            {
                leaf.transform.DOKill();
                leaf.transform.DOScale(new Vector3(0, 0, 0), duration).SetDelay(Random.Range(0, duration/2));
            }
        }
    }
}
