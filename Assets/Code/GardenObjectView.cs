using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Code.Models;
using DG.Tweening;
using UnityEngine;

namespace Code
{
    public class GardenObjectView : MonoBehaviour
    {
        [SerializeField] private GurdenObjectEnum type;
        [SerializeField] private int cost;


        [SerializeField] private ParticleSystem[] effects;

        [SerializeField] private SpriteRenderer[] _renderers;
        
        private GardenObjectModel _model;

        public GurdenObjectEnum Type => type;

        public int Cost => cost;

        public void Init(GardenObjectModel model)
        {
            gameObject.SetActive(false);
          
            
            if (model.IsCollected)
            {
                if (model.IsNew)
                {
                    model.IsNew = false;
                    ShowNewObject(model);
                }
                else
                {
                    gameObject.SetActive(true);
                }
            }
        }

        private async Task ShowNewObject(GardenObjectModel model)
        {
            await Task.Delay(1000);
            
            gameObject.SetActive(true);
            foreach (var effect in effects)
            {
                effect.gameObject.SetActive(true);
            }

            foreach (var spriteRenderer in _renderers)
            {
                var clr = spriteRenderer.color;
                clr.a = 0f;
                spriteRenderer.color = clr;
                
                spriteRenderer.DOFade(1,0.5f);
            }
        }

        [ContextMenu("GetEffects")]
        private void GetEffects()
        {
            List<ParticleSystem> ps = new List<ParticleSystem>();
            _GetEffects(transform, ps);

            GetSpriteRenderers();

            effects = ps.ToArray();
        }

        private void _GetEffects(Transform rootTransform, List<ParticleSystem> resultList)
        {
            foreach (Transform child in rootTransform)
            {
                if (child.TryGetComponent<ParticleSystem>(out var ps))
                {
                    resultList.Add(ps);
                    continue;
                }

                _GetEffects(child, resultList);
            }
        }
        
        [ContextMenu("GetSpriteRenderers")]
        private void GetSpriteRenderers()
        {
            _renderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }
}