using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Roots.SObjects
{
    [CreateAssetMenu(fileName = "SplineShape", menuName = "Data/SplineShape")]
    public class SplineShapeData : ScriptableObject
    {
        [field: SerializeField]
        public List<float3> Points { get; set; }

        [field: SerializeField] 
        public float StartingAngle { get; private set; } = 270f;

        [field: SerializeField]
        public string CardName  { get; private set; }
    }
}