using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Roots
{
    public class VineScript : MonoBehaviour
    {
        public GameObject EndPoint;
        public VineSplineExtruder Extruder;
        
        void Start()
        {
            UpdateEndPoint();
        }

        [ContextMenu("Update End Point")]
        void UpdateEndPoint()
        {
            EndPoint.transform.localPosition = Extruder.Spline.EvaluatePosition(1);
        }
    }
}
