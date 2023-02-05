using System.Collections.Generic;
using NaughtyAttributes;
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
        
        
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
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
