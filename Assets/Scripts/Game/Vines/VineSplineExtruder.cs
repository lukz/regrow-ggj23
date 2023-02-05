using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Roots.SObjects;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Roots
{
    public class VineSplineExtruder : MonoBehaviour
    {
        [SerializeField, Tooltip("Start an end scale")]
        Vector2 m_ScaleRange = new Vector2(1f, .5f);
        
        [SerializeField, Tooltip("The Spline to extrude.")]
        SplineContainer m_Container;

        [SerializeField, Tooltip("The maximum number of times per-second that the mesh will be rebuilt.")]
        int m_RebuildFrequency = 30;

        [SerializeField, Tooltip("The number of sides that comprise the radius of the mesh.")]
        int m_Sides = 8;

        [SerializeField, Tooltip("The number of edge loops that comprise the length of one unit of the mesh. The " +
             "total number of sections is equal to \"Spline.GetLength() * segmentsPerUnit\".")]
        float m_SegmentsPerUnit = 4;

        [SerializeField, Tooltip("Indicates if the start and end of the mesh are filled. When the Spline is closed this setting is ignored.")]
        bool m_Capped = true;

        [SerializeField, Tooltip("The radius of the extruded mesh.")]
        float m_Radius = .25f;

        [SerializeField, Tooltip("The section of the Spline to extrude.")]
        Vector2 m_Range = new Vector2(0f, 1f);
        
        Mesh m_Mesh;
        bool m_RebuildRequested;

        /// <summary>The SplineContainer of the <see cref="Spline"/> to extrude.</summary>
        public SplineContainer Container
        {
            get => m_Container;
            set => m_Container = value;
        }

        /// <summary>How many sides make up the radius of the mesh.</summary>
        [Obsolete("Use Sides instead.", false)]
        public int sides => Sides;
        /// <summary>How many sides make up the radius of the mesh.</summary>
        public int Sides
        {
            get => m_Sides;
            set => m_Sides = Mathf.Max(value, 3);
        }

        /// <summary>How many edge loops comprise the one unit length of the mesh.</summary>
        public float SegmentsPerUnit
        {
            get => m_SegmentsPerUnit;
            set => m_SegmentsPerUnit = Mathf.Max(value, .0001f);
        }

        /// <summary>Whether the start and end of the mesh is filled. This setting is ignored when spline is closed.</summary>
        [Obsolete("Use Capped instead.", false)]
        public bool capped => Capped;

        /// <summary>Whether the start and end of the mesh is filled. This setting is ignored when spline is closed.</summary>
        public bool Capped
        {
            get => m_Capped;
            set => m_Capped = value;
        }

        /// <summary>The radius of the extruded mesh.</summary>
        [Obsolete("Use Radius instead.", false)]
        public float radius => Radius;

        /// <summary>The radius of the extruded mesh.</summary>
        public float Radius
        {
            get => m_Radius;
            set => m_Radius = Mathf.Max(value, .00001f);
        }

        /// <summary>
        /// The section of the Spline to extrude.
        /// </summary>
        [Obsolete("Use Range instead.", false)]
        public Vector2 range => Range;

        /// <summary>
        /// The section of the Spline to extrude.
        /// </summary>
        public Vector2 Range
        {
            get => m_Range;
            set => m_Range = new Vector2(Mathf.Min(value.x, value.y), Mathf.Max(value.x, value.y));
        }
        
        public Vector2 ScaleRange
        {
            get => m_ScaleRange;
            set => m_ScaleRange = value;
        }

        public Spline Spline => m_Container.Spline;

        public IReadOnlyList<Spline> Splines => m_Container.Splines;
        
        public List<float3> Points => _points;

        private List<float3> _points;

        private void Awake()
        {
            if (_points == null ||_points.Count == 0)
            {
                LoadFromSpline();
            }

            DuplicateMesh();
        }

        private void DuplicateMesh()
        {
            var mf = GetComponent<MeshFilter>();
            mf.mesh = Instantiate(mf.mesh);
        }

        public void LoadFromSpline()
        {
            if (m_Container.Spline == null) 
                return;

            SetPoints(Spline.Select(knot => knot.Position).ToList());
        }

        void Reset()
        {
            TryGetComponent(out m_Container);

            if (TryGetComponent<MeshFilter>(out var filter) && filter.sharedMesh != null)
                m_Mesh = filter.sharedMesh;
            else
                filter.sharedMesh = m_Mesh = CreateMeshAsset();

            if (TryGetComponent<MeshRenderer>(out var renderer) && renderer.sharedMaterial == null)
            {
                // todo Make Material.GetDefaultMaterial() public
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                var mat = cube.GetComponent<MeshRenderer>().sharedMaterial;
                DestroyImmediate(cube);
                renderer.sharedMaterial = mat;
            }

            RebuildMesh();
        }

        void Start()
        {
            if (m_Container == null || m_Container.Spline == null)
            {
                Debug.LogError("Spline Extrude does not have a valid SplineContainer set.");
                return;
            }

            if((m_Mesh = GetComponent<MeshFilter>().sharedMesh) == null)
                Debug.LogError("SplineExtrude.createMeshInstance is disabled, but there is no valid mesh assigned. " +
                    "Please create or assign a writable mesh asset.");

            RebuildMesh();
        }

        private void RebuildMesh()
        {
            if(m_Mesh == null && (m_Mesh = GetComponent<MeshFilter>().sharedMesh) == null)
                return;

            RootMesh.Extrude(Splines, m_Mesh, m_Radius, m_Sides, m_SegmentsPerUnit, m_Capped, m_Range, m_ScaleRange);
        }

        private void RebuildSpline()
        {
            var newSpline = SplineFactory.CreateCatmullRom(_points, false);
            m_Container.Spline = newSpline;
        }

        public void SetPoints(List<float3> points)
        { 
            _points = points;
            
            for (var index = 0; index < _points.Count; index++)
            {
                _points[index] = new float3(_points[index].x, 0, _points[index].z);
            }

            RebuildSpline();
            RebuildMesh();
        }

        public void AppendRotatedPointsKeepSize(CardData data) 
            => AppendRotatedPointsKeepSize(data.Points, data.StartingAngle);

        public void AppendRotatedPointsKeepSize(List<float3> points, float angle)
        {
            var diff = Spline[^1].Position - Spline[^2].Position;
            var endDirection = new Vector2(diff.x, diff.z);
            
            var pointsDirection = new Vector2(0, 1);
            pointsDirection = pointsDirection.SetAngle(angle + 180);
            var angleDiff = Vector2.SignedAngle(endDirection, pointsDirection);

            var rotatedPoints = new List<float3>();
            foreach (var point in points)
            {
                var rotatedPoint = new Vector2(point.x, point.z);
                rotatedPoint = rotatedPoint.RotateRad(-angleDiff * Mathf.Deg2Rad);
                rotatedPoints.Add(new float3(rotatedPoint.x, point.y, rotatedPoint.y));
            }

            AppendPointsKeepSize(rotatedPoints);
        }

        public void AppendPointsKeepSize(List<float3> points)
        {
            var prevLength = Spline.GetLength();
            var lastPoint = _points[^1];

            // we get an ugly loop when there are two in same spot
            if (Approximately(points[0], float3.zero, .1f))
            {
                points = points.GetRange(1, points.Count - 1);
            }

            points.ForEach(r => _points.Add(lastPoint + r));

            RebuildSpline();
            
            var newLength = Spline.GetLength();
            var alpha = prevLength / newLength;

            m_Range.y = alpha;

            RebuildMesh();
        }

        // extension? 
        private static bool Approximately(float3 a, float3 b, float tolerance)
        {
            return Math.Abs(a.x - b.x) < tolerance && Math.Abs(a.y - b.y) < tolerance && Math.Abs(a.z - b.z) < tolerance;
        }

        public void StartAnimateFull(float duration = 2, float delay = 0) => StartCoroutine(AnimateFull(duration, delay));

        public void StartAnimateFullWidth() => StartCoroutine(CO_AnimateFullWidth(2));

        private IEnumerator AnimateFull(float duration, float delay)
        {
            yield return DOVirtual.Float(0, 1, duration, value =>
            {
                m_Range.y = value;
                RebuildMesh();
            }).SetDelay(delay).SetEase(Ease.OutSine).WaitForCompletion();
        }
        
        private IEnumerator CO_AnimateFullWidth(float duration)
        {
            yield return DOVirtual.Float(0, 1, duration, value =>
            {
                m_ScaleRange.y = value;
                RebuildMesh();
            }).SetEase(Ease.OutSine).WaitForCompletion();
        }

        public void StartAnimateAddition(TweenCallback OnDone)
        {
            StartCoroutine(AnimateAddition());
        }

        public IEnumerator AnimateAddition()
        {
            yield return DOVirtual.Float(m_Range.y, 1, 2, value =>
            {
                m_Range.y = value;
                RebuildMesh();
            }).SetEase(Ease.OutSine).WaitForCompletion();
        }

        public void ForceSize(float sizeScale)
        {
            m_Range.y = sizeScale;
            RebuildMesh();
        }

        void OnValidate()
        {
            RebuildMesh();
        }

        internal Mesh CreateMeshAsset()
        {
            var mesh = new Mesh();
            mesh.name = name;
            
            return mesh;
        }
    }
}