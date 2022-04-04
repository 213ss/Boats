using System;
using DG.Tweening;
using UnityEngine;

namespace Code.Models.Runner
{
    public class TutorHandsAnim : MonoBehaviour
    {
        [SerializeField] private RectTransform point;
        [SerializeField] private RectTransform hand;
        [SerializeField] private float time = 1f;
        private void Start()
        {
            hand.DOMove(point.position, time)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}