using System.Collections;
using Infrastructure.Services.SkinChanger;
using UnityEngine;
using Zenject;

namespace Logic.ImmortalAnim
{
    public class ImmortalAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _meshSocket;
        [SerializeField] private Material _transparentMaterial;

        [Header("Anim parameters")]
        [SerializeField] private float _interval;
        [SerializeField] private float _alphaPower;

        private SkinnedMeshRenderer _skinnedMesh;
        private ISkin _skin;
        private Coroutine _flickerAnimCoroutine;

        [Inject]
        private void Construct(ISkin skin)
        {
            _skin = skin;
        }

        private void Start()
        {
            _skinnedMesh = _skin.GetMeshRenderer();
        }

        public void RunAnim(float duration)
        {
            if(_flickerAnimCoroutine == null)
                _flickerAnimCoroutine = StartCoroutine(FlickerAnim(duration));
        }

        private IEnumerator FlickerAnim(float duration)
        {
            Material originMaterial = _skinnedMesh.sharedMaterial;
            
            _skinnedMesh.sharedMaterial = _transparentMaterial;
            Color color = _transparentMaterial.color;
            
            float timer = duration;
            float timerInterval = _interval;
            float timerInterval2 = _interval;
            
            while (timer >= 0.0f)
            {
                if (timerInterval > 0.0f)
                {
                    color.a = _alphaPower;
                    timerInterval -= Time.deltaTime;
                }
                else
                {
                    if (timerInterval2 > 0.0f)
                    {
                        color.a = 0.1f;
                        timerInterval2 -= Time.deltaTime;
                    }
                    else
                    {
                        timerInterval = _interval;
                        timerInterval2 = _interval;
                    }
                }

                _skinnedMesh.sharedMaterial.color = color;
                timer -= Time.deltaTime;
                yield return null;
            }

            
            _skinnedMesh.sharedMaterial = originMaterial;
            _flickerAnimCoroutine = null;
        }

        public void StopAnim()
        {
            if(_flickerAnimCoroutine != null)
                StopCoroutine(_flickerAnimCoroutine);
        }
    }
}