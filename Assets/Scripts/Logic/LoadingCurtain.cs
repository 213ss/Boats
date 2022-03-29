using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Logic
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private float _fadeSpeed = 0.03f;
        [SerializeField] private CanvasGroup _curtain;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _curtain.alpha = 1;
        }

        public void Hide() => 
            StartCoroutine(FadeIn());


        private IEnumerator FadeIn()
        {
            while(_curtain.alpha > 0.0f)
            {
                _curtain.alpha -= _fadeSpeed * Time.deltaTime;
                yield return null;
            }

            gameObject.SetActive(false);
        }
    }
}