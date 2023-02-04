using System.Collections;
using System.Collections.Generic;
using Roots.SObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Roots
{
    public class CardsUser : MonoBehaviour
    {
        public CardsUI cardsUI;
        public SplineShapeData[] SplineShapes;
        private CardScript selected;
        private bool isLocked;
        
        private List<VineEndPoint> EndPoints = new();
        
        void Start()
        {
            // dummy cards
            for (int i = 0; i < 7; i++)
            {
                cardsUI.AddCard(SplineShapes[i % SplineShapes.Length]);
            }
            
            cardsUI.OnCardSelected += StartSelection;
            cardsUI.OnCardUnselected += CancelSelection;
            HideEndPoints();
        }
        
        void StartSelection(CardScript cs)
        {
            selected = cs;
            if (isLocked) return;
            ShowEndPoints();
            PreviewEndPoints(cs.splineShapeData);
        }

        void CancelSelection(CardScript cs)
        { 
            selected = null;
            StopPreviewEndPoints();
            HideEndPoints();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && selected != null && !isLocked)
            {
                UseCardAtMouse();
            }
        }

        private void UseCardAtMouse()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit))
            {
                CancelSelection(null);
                return;
            }
            
            var endPoint = hit.collider.gameObject.GetComponent<VineEndPoint>();
            if (endPoint == null)
            {
                CancelSelection(null);
                return;
            }

            isLocked = true;
            endPoint.Append(selected.splineShapeData, () =>
            {
                isLocked = false;
                if (selected == null) return;
                // selection changed during animation
                StartSelection(selected);
            });
            cardsUI.UseCard(selected.splineShapeData);
            CancelSelection(null);
        }

        private void HideEndPoints()
        {
            EndPoints.ForEach(point => point.Hide());
        }
        private void ShowEndPoints()
        {
            EndPoints.ForEach(point => point.Show());
        }
        private void PreviewEndPoints(SplineShapeData shapeData)
        {
            EndPoints.ForEach(point => point.Preview(shapeData));
        }
        
        private void StopPreviewEndPoints()
        {
            EndPoints.ForEach(point => point.StopPreview());
        }

        public void AddEndPoint(VineEndPoint vineEndPoint)
        {
            EndPoints.Add(vineEndPoint);
            if (selected != null)
            {
                vineEndPoint.Show();
                vineEndPoint.Preview(selected.splineShapeData);
            }
            else
            {
                vineEndPoint.Hide();
                vineEndPoint.StopPreview();
            }
        }
    }
}
