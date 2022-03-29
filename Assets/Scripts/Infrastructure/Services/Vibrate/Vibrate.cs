using System.Collections;
using UnityEngine;

namespace Infrastructure.Services.Vibrate
{
    public class Vibrate : MonoBehaviour, IVibrate
    {
        public bool IsEnableVibrate { get; private set; }
        
        private Coroutine _virate;
        private bool _isVibrate;


        private void Start()
        {
            IsEnableVibrate = true;
        }

        private void Update()
        {
            if(_isVibrate && IsEnableVibrate) Handheld.Vibrate();
        }

        public void EnableVibrate()
        {
            IsEnableVibrate = true;
        }

        public void DisableVibrate()
        {
            IsEnableVibrate = false;
        }

        public void PlayVibrate(float time)
        {
            if(_virate == null)
                _virate = StartCoroutine(VibratePlay(time));
        }

        private IEnumerator VibratePlay(float time)
        {
            _isVibrate = true;
            yield return new WaitForSeconds(time);
            _isVibrate = false;
            _virate = null;
        }
    }
}
