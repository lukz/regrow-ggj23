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
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CardTarget"))
                    {
                        isLocked = true;
                        cardsUI.UseCard(selected.splineShapeData);
                        var extruder = hit.collider.gameObject.GetComponentInParent<VineSplineExtruder>();
                        extruder.AppendRotatedPointsKeepSize(selected.splineShapeData);
                        extruder.StartAnimateAddition(() =>
                        {
                            // animate, onSplineChange?
                            var vine = hit.collider.gameObject.GetComponentInParent<VineScript>();
                            vine.UpdateEndPoint();
                            isLocked = false;
                        });
                        
                    }
                }
            }
        }
        
    }
}
