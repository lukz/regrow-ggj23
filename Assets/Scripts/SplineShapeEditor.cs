using System;
using System.Linq;
using Roots.SObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

namespace Roots
{
    public class SplineShapeEditor : MonoBehaviour
    {
        [SerializeField] private CardData _data;

        [ContextMenu("Save")]
        private void Save()
        {
            if (_data == null)
                return;

            var splineComp = GetComponent<SplineContainer>();
            var points = splineComp.Spline.Select(knot => knot.Position).ToList();

            _data.Points = points;

            EditorUtility.SetDirty(_data);
        }

        [ContextMenu("Load")]
        private void Load()
        {
            if (_data == null)
                return;

            var splineComp = GetComponent<SplineContainer>();
            splineComp.Spline.Clear();

            foreach (var point in _data.Points)
            {
                splineComp.Spline.Add(new BezierKnot(point), TangentMode.AutoSmooth);
            }
        }
    }
}