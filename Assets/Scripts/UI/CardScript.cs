using System;
using System.Collections;
using System.Collections.Generic;
using Roots.SObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Roots
{
    public class CardScript : MonoBehaviour
    {
        public Image Image;
        public Button Button;

        public TextMeshProUGUI Text;
        
        public event Action<CardScript> OnCardClicked;

        public int Uses = 0;

        [FormerlySerializedAs("splineShapeData")] public CardData cardData;
        public void Setup(CardData cardData)
        {
            this.cardData = cardData;
            Uses += 1;
            UpdateText();
            Image.sprite = Sprite.Create(cardData.Icon, new Rect(new Vector2(0, 0), new Vector2(512, 512)), new Vector2(256, 256));
        }

        public void OnClicked()
        {
            OnCardClicked?.Invoke(this);
        }

        public bool Use()
        {
            if (Uses >= 1)
            {
                Uses--;
                UpdateText();
                return true;
            }
            return false;
        }

        private void UpdateText()
        {
            // Text.SetText(cardData.CardName + " " + Uses);
            Text.SetText("x " + Uses);
        }
    }
}
