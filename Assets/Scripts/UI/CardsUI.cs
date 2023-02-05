using System;
using System.Collections.Generic;
using Roots.SObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Roots
{
    public class CardsUI : MonoBehaviour
    {
        public CardScript Prefab;
        public HorizontalLayoutGroup CardsGroup;
        
        public event Action<CardScript> OnCardSelected;
        public event Action<CardScript> OnCardUnselected;
        public event Action<CardScript> OnCardPostCreate;
        public event Action<CardScript> OnCardPreDestroy;

        protected CardScript Selected;
        
        protected Dictionary<CardData, CardScript> CardScripts = new();
        
        public void AddCard(CardData cardData)
        {
            if (!CardScripts.TryGetValue(cardData, out var cardScript))
            {
                cardScript = Instantiate(Prefab, CardsGroup.gameObject.transform);
                CardScripts[cardData] = cardScript;
                cardScript.OnCardClicked += OnCardClicked;
            }
            cardScript.Setup(cardData);
        }

        private void OnCardClicked(CardScript obj)
        { 
            OnCardSelected?.Invoke(obj);
        }

        public void UseCard(CardData cardData)
        {
            if (!CardScripts.TryGetValue(cardData, out var cardScript)) return;
            if (!cardScript.Use()) return;
            if (cardScript.Uses != 0) return;
            
            cardScript.OnCardClicked -= OnCardClicked;
            CardScripts.Remove(cardData);
            OnCardUnselected?.Invoke(cardScript);
            Destroy(cardScript.gameObject);
        }
    }
}
