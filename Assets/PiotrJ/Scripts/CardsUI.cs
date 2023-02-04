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


        protected Dictionary<SplineShapeData, CardScript> CardScripts = new();


        public void AddCard(SplineShapeData splineShapeData)
        {
            CardScript cardScript;
            if (!CardScripts.TryGetValue(splineShapeData, out cardScript))
            {
                cardScript = Instantiate(Prefab, CardsGroup.gameObject.transform);
                CardScripts[splineShapeData] = cardScript;
                cardScript.OnCardClicked += OnCardClicked;
            }
            cardScript.Setup(splineShapeData);
        }

        private void OnCardClicked(CardScript obj)
        { 
            OnCardSelected?.Invoke(obj);
        }

        public void UseCard(SplineShapeData splineShapeData)
        {
            CardScript cardScript;
            if (!CardScripts.TryGetValue(splineShapeData, out cardScript)) return;
            
            if (cardScript.Use())
            {
                if (cardScript.Uses == 0)
                {
                    cardScript.OnCardClicked -= OnCardClicked;
                    CardScripts.Remove(splineShapeData);
                    OnCardUnselected?.Invoke(cardScript);
                    Destroy(cardScript.gameObject);
                }
            }
        }
    }
}
