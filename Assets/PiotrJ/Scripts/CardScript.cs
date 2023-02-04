using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

        public CardSO CardSo;
        public void Setup(CardSO cardSo)
        {
            CardSo = cardSo;
            Uses += 1;
            UpdateText();
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
            Text.SetText(CardSo.cardName + " " + Uses);
        }
    }
}
