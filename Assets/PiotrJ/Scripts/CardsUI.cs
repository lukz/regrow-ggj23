using System;
using System.Collections;
using System.Collections.Generic;
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


        protected Dictionary<CardSO, CardScript> CardScripts = new();


        public void AddCard(CardSO cardSo)
        {
            CardScript cardScript;
            if (!CardScripts.TryGetValue(cardSo, out cardScript))
            {
                cardScript = Instantiate(Prefab, CardsGroup.gameObject.transform);
                CardScripts[cardSo] = cardScript;
                cardScript.OnCardClicked += OnCardClicked;
            }
            cardScript.Setup(cardSo);
        }

        private void OnCardClicked(CardScript obj)
        { 
            OnCardSelected?.Invoke(obj);
        }

        public void UseCard(CardSO cardSo)
        {
            CardScript cardScript;
            if (!CardScripts.TryGetValue(cardSo, out cardScript)) return;
            
            if (cardScript.Use())
            {
                if (cardScript.Uses == 0)
                {
                    cardScript.OnCardClicked -= OnCardClicked;
                    CardScripts.Remove(cardSo);
                    OnCardUnselected?.Invoke(cardScript);
                    Destroy(cardScript.gameObject);
                }
            }
        }
    }
}
