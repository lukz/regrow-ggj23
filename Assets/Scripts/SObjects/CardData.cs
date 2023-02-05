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
    }
}