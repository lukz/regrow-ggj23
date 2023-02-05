using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NaughtyAttributes;
using Roots.SObjects;
using TreeEditor;
using UnityEditor;
using UnityEngine;

namespace Roots
{
    public class TreeScript : MonoBehaviour
    {
        [field: Header("Config")]
        [field: SerializeField] public bool StartAlive { get; private set; }

        [Header("Internal references")]
        [SerializeField] private TreeGrower treeModel = new();
        [SerializeField, ReadOnly] private List<RootScript> roots = new();
        
        [Header("Prefabs")]
        [SerializeField] private RootScript rootPrefab;
        
        public event Action<TreeScript, RootScript, CardData> OnGrowthRequested;
        public bool IsAlive { get; private set; }

        private void Start()
        {
            roots.Clear();
            roots.AddRange(GetComponentsInChildren<RootScript>());

            foreach (var rootScript in roots)
            {
                rootScript.OnGrowthRequested += OnRootGrowthRequested;
            }

            SetNotAlive();

            if (StartAlive)
            {
                SetAlive(true);
            }
        }

        private void SetNotAlive()
        {
            IsAlive = false;
            
            treeModel.Shrink(0);

            HideRoots();
        }

        public void SetAlive(bool isStart)
        {
            StartCoroutine(CO_SetAlive(isStart));
        }

        private IEnumerator CO_SetAlive(bool isStart = false)
        {
            IsAlive = true;
            
            var growDuration = isStart ? 2 : 1;
            
            treeModel.Grow(growDuration);
            
            yield return new WaitForSeconds(growDuration);
            
            GrowInitialRoots(2);
            
            yield return new WaitForSeconds(2);
        }

        private void OnRootGrowthRequested(RootScript root, CardData card) => OnGrowthRequested?.Invoke(this, root, card);

        public IEnumerator GrowRootWithCard(RootScript root, CardData card)
        {
            yield return root.GrowWithCard(card);
        }

        public void HideRoots() => roots.ForEach(r => r.ForceHidden());
        
        public void GrowInitialRoots(float duration) => roots.ForEach(r => r.GrowFull(duration, 0));

        public void ShowEndPoints()
        {
            if(!IsAlive)
                return;
            
            roots.ForEach(r => r.ShowEndPoint());
        }

        public void HideEndPoints() => roots.ForEach(r => r.HideEndPoint());

        public List<(VineEndPoint, Vector3)> PreviewEndPoints(CardData cardData) 
            => roots
                .Where(r => r.IsAvailableForGrowth)
                .Select(r => (r.EndPoint, r.PreviewEndPoint(cardData)))
                .ToList();

        public void StopPreviewEndPoints() => roots.ForEach(r => r.StopPreviewEndPoint());

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
