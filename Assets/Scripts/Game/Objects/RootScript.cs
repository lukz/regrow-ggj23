using System;
using System.Collections;
using System.Collections.Generic;
using Roots.SObjects;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Roots
{
    [SelectionBase]
    public class RootScript : MonoBehaviour
    {
        public VineEndPoint EndPoint;
        public VineSplineExtruder Extruder;
        
        public event Action<RootScript, CardData> OnGrowthRequested;
        
        void Start()
        {
            EndPoint.MoveToEnd();
            EndPoint.OnGrowthRequested += RequestGrowth;
        }

        public void RequestGrowth(CardData card) => OnGrowthRequested?.Invoke(this, card);
        
        public IEnumerator Grow(CardData card)
        {
            yield return EndPoint.Append(card);
        }

        [ContextMenu("Update End Point")]
        public void UpdateEndPoint()
        {
            EndPoint.MoveToEnd();
        }
        
        
    }
}
