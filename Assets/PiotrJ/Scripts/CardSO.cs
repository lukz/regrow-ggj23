using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots
{
    [CreateAssetMenu(fileName = "Card", menuName = "Roots/CardSO", order = 1)]
    public class CardSO : ScriptableObject
    {
        public string cardName;
    }
}
