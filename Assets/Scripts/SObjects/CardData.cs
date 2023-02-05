using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Roots.SObjects
{
    [CreateAssetMenu(fileName = "Card", menuName = "Data/CardData")]
    public class CardData : ScriptableObject
    {
        [field: SerializeField]
        public List<float3> Points { get; set; }

        [field: SerializeField] 
        public float StartingAngle { get; private set; } = 270f;

        [field: SerializeField]
        public string CardName  { get; private set; }

        [field: SerializeField]
        public Sprite Icon  { get; private set; }

        private void OnValidate()
        {
            for (var index = 0; index < Points.Count; index++)
            {
                Points[index] = new float3(Points[index].x, 0, Points[index].z);
            }
        }
    }
}