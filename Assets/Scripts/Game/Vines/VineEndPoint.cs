using System;
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
        private VineSplineExtruder previewExtruder;
        public Material previewMaterial;

        public event Action<CardData> OnGrowthRequested;

        private void Start()
        {
            transform.localScale = new Vector3();
            FindObjectOfType<CardsManager>().AddEndPoint(this);
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

        public void Preview(CardData shapeData)
        {
            StopPreview();
            var prevLength = Extruder.Spline.GetLength();
            
            previewExtruder = Instantiate(Extruder, transform.parent);
            previewExtruder.ScaleRange = new Vector2((previewExtruder.ScaleRange.x + previewExtruder.ScaleRange.y)/2, previewExtruder.ScaleRange.y);
            
            // delay so it doesnt change Extruder mesh as well :shrug:
            previewExtruder.transform.DOLocalMove(new Vector3(), 1/30f).OnComplete(() =>
            {
                previewExtruder.AppendRotatedPointsKeepSize(shapeData);
                // move start of preview spline to end of actual one
                var newLength = previewExtruder.Spline.GetLength();
                var alpha = prevLength / newLength;
                
                previewExtruder.Range = new Vector2(alpha, 1);
                
                previewExtruder.FullSize();
                previewExtruder.GetComponent<MeshRenderer>().material = previewMaterial;
            });
            
        }

        public void StopPreview()
        {
            if (previewExtruder == null) return;
            previewExtruder.transform.DOKill();
            Destroy(previewExtruder.gameObject);
        }
    }
}
