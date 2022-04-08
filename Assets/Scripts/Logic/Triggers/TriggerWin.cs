using System.Collections;
using Actors;
using Infrastructure.Services.Confetti;
using UnityEngine;
using Zenject;

namespace Logic.Triggers
{
    public class TriggerWin : MonoBehaviour
    {
        private readonly int _finalCamera = Animator.StringToHash("FinalCamera");

        [SerializeField] private Animator _cinemachineAnimator;
        
        private IConfettiService _confettiService;

        [Inject]
        private void Construct(IConfettiService confettiService)
        {
            _confettiService = confettiService;
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            Actor actor = other.GetComponent<Actor>();
            
            actor.YouWin();
            actor.ActorTransform.rotation = transform.rotation;

            if (other.CompareTag("Player") == false)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Actor>().DroppedOutGame();
            }

            StartCoroutine(DelayCameraSwitch());
        }

        private IEnumerator DelayCameraSwitch()
        {
            _confettiService.PlayConfetti();
            
            float timer = 2.0f;
            while (timer >= 0.0f)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
            
            SwitchCameraToWinCamera();
        }

        private void SwitchCameraToWinCamera()
        {
            _cinemachineAnimator.Play(_finalCamera);
        }

#if UNITY_EDITOR

        [Header("Only editor mode")]
        public Color ColorBox;
        public Vector3 BoxSize;


        private void OnDrawGizmos()
        {
            Gizmos.color = ColorBox;
            Gizmos.DrawWireCube(transform.position, BoxSize);
        }

#endif
    }
}
