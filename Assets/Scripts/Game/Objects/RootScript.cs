using System;
using System.Collections;
using Roots.SObjects;
using UnityEngine;

namespace Roots
{
    [SelectionBase]
    public class RootScript : MonoBehaviour
    {
        public bool IsAvailableForGrowth { get; private set; } = true;
        
        public VineEndPoint EndPoint;
        public VineSplineExtruder Extruder;
        
        public event Action<RootScript, CardData> OnGrowthRequested;
        
        void Start()
        {
            EndPoint.MoveToEnd();
            EndPoint.OnGrowthRequested += RequestGrowth;
        }

        public void RequestGrowth(CardData card) => OnGrowthRequested?.Invoke(this, card);
        
        public IEnumerator GrowWithCard(CardData card)
        {
            yield return EndPoint.Append(card);
        }

        public void ForceHidden() => Extruder.ForceSize(0);

        public void GrowFull(float duration, float delay) => Extruder.StartAnimateFull(duration, delay);

        [ContextMenu("Update End Point")]
        public void UpdateEndPoint() => EndPoint.MoveToEnd();
        
        public void ShowEndPoint()
        {
            if(!IsAvailableForGrowth)
                return;
            
            EndPoint.Show();
        }

        public void HideEndPoint() => EndPoint.Hide();

        public Vector3 PreviewEndPoint(CardData cardData) => EndPoint.Preview(cardData);

        public void StopPreviewEndPoint() => EndPoint.StopPreview();
        
        public void SetConnected()
        {
            IsAvailableForGrowth = false;
            
            Extruder.StartAnimateFullWidth();
        }
    }
}
