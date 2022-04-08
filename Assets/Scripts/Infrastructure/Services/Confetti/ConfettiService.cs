using System.Collections;
using UnityEngine;

namespace Infrastructure.Services.Confetti
{
    public class ConfettiService : MonoBehaviour, IConfettiService
    {
        [SerializeField] private float _delay;
        [SerializeField] private ParticleSystem[] _particleSystems;



        public void PlayConfetti()
        {
            StartCoroutine(PlayDelay());
        }

        private IEnumerator PlayDelay()
        {
            float delay = _delay;
            
            while (delay >= 0.0f)
            {
                delay -= Time.deltaTime;
                yield return null;
            }

            PlayParticles();
        }

        private void PlayParticles()
        {
            for (int i = 0; i < _particleSystems.Length; ++i)
            {
                _particleSystems[i].Play();
            }
        }
    }
}
