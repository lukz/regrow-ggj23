using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Roots.SObjects;
using UnityEngine;
using UnityEngine.Splines;

namespace Roots
{
    public class VineEndPoint : MonoBehaviour
    {
        public VineSplineExtruder Extruder;
        
        public void MoveToEnd()
        {
            transform.localPosition = Extruder.Spline.EvaluatePosition(1);
        }

        public void Append(SplineShapeData shapeData, TweenCallback completed)
        {
            transform.DOScale(new Vector3(), .2f);
            Extruder.AppendRotatedPointsKeepSize(shapeData);
            Extruder.StartAnimateAddition(
                () => {
                    MoveToEnd();
                    transform.DOScale(new Vector3(1, 1, 1), .2f);
                    completed.Invoke();
                }
            );
        }
    }
}
