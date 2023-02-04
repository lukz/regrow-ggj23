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

        public SplineShapeData splineShapeData;
        public void Setup(SplineShapeData splineShapeData)
        {
            this.splineShapeData = splineShapeData;
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
            Text.SetText(splineShapeData.CardName + " " + Uses);
        }
    }
}
