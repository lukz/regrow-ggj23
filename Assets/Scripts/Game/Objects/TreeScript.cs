using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DG.Tweening;
using NaughtyAttributes;
using Roots.SObjects;
using UnityEditor;
using UnityEngine;

namespace Roots
{
    [SelectionBase]
    public class TreeScript : MonoBehaviour
    {
        [field: Header("Config")]
        [field: SerializeField] public bool StartAlive { get; private set; }

        [field: SerializeField] public List<CardData> CardsToReceive { get; private set; } = new List<CardData>();

        [Header("Internal references")]
        [SerializeField] private TreeGrower treeModel = new();
        [SerializeField, ReadOnly] private List<RootScript> roots = new();
        
        [Header("Prefabs")]
        [SerializeField] private RootScript rootPrefab;
        [SerializeField] private SpriteRenderer cardSprite;
        
        public event Action<TreeScript, RootScript, CardData> OnGrowthRequested;
        public bool IsAlive { get; private set; }

        private void Start()
        {
            roots.Clear();
            roots.AddRange(GetComponentsInChildren<RootScript>());

            foreach (var rootScript in roots)
            {
                rootScript.OnGrowthRequested += OnRootGrowthRequested;
            }

            SetNotAlive();

            if (StartAlive)
            {
                SetAlive(true);
            }
        }

        private void SetNotAlive()
        {
            IsAlive = false;
            
            treeModel.Shrink(0);

            HideRoots();
        }

        public void SetAlive(bool isStart)
        {
            StartCoroutine(CO_SetAlive(isStart));
        }

        private IEnumerator CO_SetAlive(bool isStart = false)
        {
            IsAlive = true;
            
            var growDuration = isStart ? 2 : 1;
            
            treeModel.Grow(growDuration);
            
            yield return new WaitForSeconds(growDuration);
            
            GrowInitialRoots(2);

            AnimateCardsReceive();
            
            yield return new WaitForSeconds(2);
        }

        private void OnRootGrowthRequested(RootScript root, CardData card) => OnGrowthRequested?.Invoke(this, root, card);

        public IEnumerator GrowRootWithCard(RootScript root, CardData card)
        {
            yield return root.GrowWithCard(card);
        }

        public void HideRoots() => roots.ForEach(r => r.ForceHidden());
        
        public void GrowInitialRoots(float duration) => roots.ForEach(r => r.GrowFull(duration, 0));

        public void ShowEndPoints()
        {
            if(!IsAlive)
                return;
            
            roots.ForEach(r => r.ShowEndPoint());
        }

        public void HideEndPoints() => roots.ForEach(r => r.HideEndPoint());

        public void AnimateCardsReceive()
        {
            for (var index = 0; index < CardsToReceive.Count; index++)
            {
                var cardData = CardsToReceive[index];
                
                var sprite = Instantiate(cardSprite, transform);
                sprite.transform.localPosition = Vector3.zero;
                sprite.transform.localScale = Vector3.zero;
                sprite.sprite = cardData.Icon;
                
                var targetPos = new Vector2(4, 0);
                targetPos = targetPos.SetAngle((360 / CardsToReceive.Count) * index);
                
                DOTween.Sequence()
                    .AppendInterval(0.1f * index)
                    .AppendCallback(() => SoundManager.Instance.PlaySound(SoundManager.SFXType.NewCard))
                    .Append(sprite.transform.DOLocalMove(new Vector3(targetPos.x, 2, targetPos.y), 1f).SetEase(Ease.OutBack))
                    .Join(sprite.DOFade(1, 1f).SetEase(Ease.OutBack))
                    .Join(sprite.transform.DOScale(0.7f, 1f).SetEase(Ease.OutBack))
                    .AppendInterval(0.5f)
                    .Append(sprite.DOFade(0, 0.2f).SetEase(Ease.OutSine))
                    .Join(sprite.transform.DOMoveZ(-5, 0.3f).SetRelative(true).SetEase(Ease.OutSine))
                    .OnComplete(() =>
                    {
                        // Destroy(sprite);
                        Debug.Log(sprite, gameObject);
                        App.instance.game.cardsManager.AddCard(cardData);
                    });
            }
        }

        public List<(VineEndPoint, Vector3)> PreviewEndPoints(CardData cardData) 
            => roots
                .Where(r => r.IsAvailableForGrowth)
                .Select(r => (r.EndPoint, r.PreviewEndPoint(cardData)))
                .ToList();

        public void StopPreviewEndPoints() => roots.ForEach(r => r.StopPreviewEndPoint());

#if UNITY_EDITOR
        [Button("Add root")]
        private void AddRootEditor()
        {
            PrefabUtility.InstantiatePrefab(rootPrefab, transform);
            
            OnValidate();
        }

        private void OnValidate()
        {
            roots.Clear();
            roots.AddRange(GetComponentsInChildren<RootScript>());
        }
#endif
    }
}
