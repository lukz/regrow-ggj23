using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Roots.SObjects;
using UnityEngine;
using UnityEngine.Splines;

namespace Roots
{
    public class VineEndPoint : MonoBehaviour
    {
        public VineSplineExtruder Extruder;
        public VineSplineExtruder PreviewExtruder;
        public Material previewMaterial;

        public event Action<CardData> OnGrowthRequested;

        private void Start()
        {
            transform.localScale = new Vector3();
        }

        public void MoveToEnd()
        {
            transform.localPosition = Extruder.Spline.EvaluatePosition(1);
        }

        public void GrowthRequested(CardData cardData) => OnGrowthRequested?.Invoke(cardData);

        public IEnumerator Append(CardData shapeData)
        {
            Extruder.AppendRotatedPointsKeepSize(shapeData);
            
            yield return Extruder.AnimateAddition();
            
            MoveToEnd();
        }

        public void Hide()
        {
            transform.DOKill();
            transform.DOScale(new Vector3(), .2f);
        }

        public void Show()
        {
            transform.DOKill();
            transform.DOScale(new Vector3(1, 1, 1), .2f);
        }

        public Vector3 Preview(CardData shapeData)
        {
            StopPreview();
            var prevLength = Extruder.Spline.GetLength();
            
            PreviewExtruder.gameObject.SetActive(true);
            PreviewExtruder.SetPoints(Extruder.Points.ToList());
            
            PreviewExtruder.ScaleRange = new Vector2((PreviewExtruder.ScaleRange.x + PreviewExtruder.ScaleRange.y)/2, PreviewExtruder.ScaleRange.y);
            PreviewExtruder.AppendRotatedPointsKeepSize(shapeData);
            // move start of preview spline to end of actual one
            var newLength = PreviewExtruder.Spline.GetLength();
            var alpha = prevLength / newLength;
                
            PreviewExtruder.Range = new Vector2(alpha, 1);
                
            PreviewExtruder.FullSize();
            PreviewExtruder.GetComponent<MeshRenderer>().material = previewMaterial;

            return transform.parent.TransformPoint(PreviewExtruder.Spline.EvaluatePosition(1));
        }

        public void StopPreview()
        {
            PreviewExtruder.gameObject.SetActive(false);
        }
    }
}
