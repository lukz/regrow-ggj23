
using Roots.SObjects;
using UnityEngine;
using UnityEngine.Splines;

namespace Roots
{
    public class LukaszSceneTest : MonoBehaviour
    {
        [SerializeField] private SplineContainer _spline;
        [SerializeField] private VineSplineExtruder _extruder;
        
        [SerializeField] private SplineShapeData _shapeData;
        
        [ContextMenu("Add shape")]
        void AddShape()
        {
            _extruder.AppendRotatedPointsKeepSize(_shapeData.Points, _shapeData.StartingAngle);
            _extruder.StartAnimateAddition();
        }
    }
}
