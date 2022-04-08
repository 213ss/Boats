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
            Vibration.Init();
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
            Vibration.VibratePop();
        }
    }
}
