using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Roots.SObjects;
using UnityEditor;
using UnityEngine;

namespace Roots
{
    public class TreeScript : MonoBehaviour
    {
        [field: Header("Config")]
        [field: SerializeField] public bool IsAlive { get; private set; }

        [Header("Internal references")]
        [SerializeField] private List<RootScript> roots = new();
        
        [Header("Prefabs")]
        [SerializeField] private RootScript rootPrefab;
        
        public event Action<TreeScript, RootScript, CardData> OnGrowthRequested;

        private void Start()
        {
            roots.Clear();
            roots.AddRange(GetComponentsInChildren<RootScript>());

            foreach (var rootScript in roots)
            {
                rootScript.OnGrowthRequested += OnRootGrowthRequested;
            }
        }

        private void OnRootGrowthRequested(RootScript root, CardData card) => OnGrowthRequested?.Invoke(this, root, card);

        public IEnumerator Grow(RootScript root, CardData card)
        {
            yield return root.Grow(card);
        }

#if UNITY_EDITOR
        [Button("Add root")]
        private void AddRootEditor()
        {
            PrefabUtility.InstantiatePrefab(rootPrefab, transform);
            
            OnValidate();
        }

        private void OnValidate()
        {
            roots.Clear();
            roots.AddRange(GetComponentsInChildren<RootScript>());
        }
#endif

    }
}
