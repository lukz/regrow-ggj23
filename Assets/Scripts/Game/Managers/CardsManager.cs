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
        
        public event Action<CardData> OnCardUsed;
        public event Action<CardData> OnCardAdded;
        public event Action OnHideEndPointsRequested;
        public event Action OnShowEndPointsRequested;
        public event Action<CardData> OnPreviewEndPointsRequested;
        public event Action OnStopPreviewEndPointsRequested;
        public event Func<bool> IsInputLocked;
        
        void Start()
        {
            var cards = App.instance.appData.assets.Cards;
            for (var i = 0; i < 20; i++)
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

        private void HideEndPoints() => OnHideEndPointsRequested?.Invoke();

        private void ShowEndPoints() => OnShowEndPointsRequested?.Invoke();

        private void PreviewEndPoints(CardData cardData) => OnPreviewEndPointsRequested?.Invoke(cardData);

        private void StopPreviewEndPoints() => OnStopPreviewEndPointsRequested?.Invoke();
        
    }
}
