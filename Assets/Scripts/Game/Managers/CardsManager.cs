using System;
using System.Collections.Generic;
using Roots.SObjects;
using UnityEditor;
using UnityEngine;

namespace Roots
{
    public class CardsManager : MonoBehaviour
    {
        private CardScript selected;
        private bool isLocked;
        
        private List<VineEndPoint> EndPoints = new();

        public event Action<CardData> OnCardUsed;
        public event Action<CardData> OnCardAdded;
        
        public event Func<bool> IsInputLocked;
        
        void Start()
        {
            var cards = App.instance.appData.assets.Cards;
            for (var i = 0; i < 7; i++)
            {
                AddCard(cards[i % cards.Count]);
            }

            HideEndPoints();
        }
        
        public void StartSelection(CardScript card)
        {
            selected = card;
            
            if (IsInputLocked!.Invoke()) return;
            
            ShowEndPoints();
            PreviewEndPoints(card.cardData);
        }

        public void CancelSelection(CardScript card)
        { 
            selected = null;
            StopPreviewEndPoints();
            HideEndPoints();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && selected != null)
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
            
            if(IsInputLocked!.Invoke())
                return;
            
            endPoint.GrowthRequested(selected.cardData);
            // endPoint.Append(selected.cardData, () => {});
            
            OnCardUsed?.Invoke(selected.cardData);
            
            CancelSelection(null);
        }

        public void PreviewActualSelection()
        {
            if (selected == null) return;
            
            StartSelection(selected);
        }

        public void AddCard(CardData card)
        {
            OnCardAdded?.Invoke(card);
        }

        private void HideEndPoints() => EndPoints.ForEach(point => point.Hide());

        private void ShowEndPoints() => EndPoints.ForEach(point => point.Show());

        private void PreviewEndPoints(CardData shapeData) => EndPoints.ForEach(point => point.Preview(shapeData));

        private void StopPreviewEndPoints() => EndPoints.ForEach(point => point.StopPreview());

        public void AddEndPoint(VineEndPoint vineEndPoint)
        {
            EndPoints.Add(vineEndPoint);
            if (selected != null)
            {
                vineEndPoint.Show();
                vineEndPoint.Preview(selected.cardData);
            }
            else
            {
                vineEndPoint.Hide();
                vineEndPoint.StopPreview();
            }
        }
    }
}
