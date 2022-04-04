using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{

    public class GardenHandsAnim : MonoBehaviour
    {
        [SerializeField] private Image hand1;
        [SerializeField] private Image hand2;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float time = 1f;

        private void Start()
        {
            var h1StartPos = hand1.transform.position + new Vector3(-Mathf.Abs(offset.x), Mathf.Abs(offset.x), Mathf.Abs(offset.x)) ;
            var h2StartPos = hand2.transform.position + new Vector3(Mathf.Abs(offset.x), -Mathf.Abs(offset.x), -Mathf.Abs(offset.x));

            hand1.transform.DOMove(h1StartPos, time).SetLoops(-1);
            hand2.transform.DOMove(h2StartPos + new Vector3(1, -1, -1), time).SetLoops(-1);
            hand1.DOFade(0, time).SetLoops(-1);
            hand2.DOFade(0, time).SetLoops(-1);
        }
    }
}