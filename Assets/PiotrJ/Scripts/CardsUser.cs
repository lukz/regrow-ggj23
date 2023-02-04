using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Roots
{
    public class CardsUser : MonoBehaviour
    {
        public CardsUI cardsUI;
        public CardSO[] CardSos;
        private CardScript selected;
        
        void Start()
        {
            for (int i = 0; i < 7; i++)
            {
                cardsUI.AddCard(CardSos[i % CardSos.Length]);
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
            if (Input.GetMouseButtonDown(0) && selected != null)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("CardTarget"))
                    {
                        cardsUI.UseCard(selected.CardSo);        
                    }
                }
            }
        }
        
    }
}
