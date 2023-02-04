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
        
        void Start()
        {
            for (int i = 0; i < 7; i++)
            {
                cardsUI.AddCard(SplineShapes[i % SplineShapes.Length]);
            }
            
            cardsUI.OnCardSelected += cs =>
            {
                selected = cs;
            };
            cardsUI.OnCardUnselected += cs =>
            {
                selected = null;
            };
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
            if (!Physics.Raycast(ray, out var hit)) return;
            
            var endPoint = hit.collider.gameObject.GetComponent<VineEndPoint>();
            if (endPoint == null) return;

            isLocked = true;
            endPoint.Append(selected.splineShapeData, () => { isLocked = false; });
            cardsUI.UseCard(selected.splineShapeData);
        }
    }
}
