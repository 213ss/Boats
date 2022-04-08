using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Infrastructure.Services.Vibrate
{
    public class VibrateSettings : MonoBehaviour
    {
        [SerializeField] private Image _targetImage;
        [SerializeField] private Sprite _onVibrate;
        [SerializeField] private Sprite _offVibrate;


        private IVibrate _vibrate;
        
        [Inject]
        private void Construct(IVibrate vibrate)
        {
            _vibrate = vibrate;
        }

        public void SwitchParameters()
        {
            if (_vibrate.IsEnableVibrate)
            {
                _vibrate.DisableVibrate();
            }
            else
            {
                _vibrate.EnableVibrate();
            }

            SwitchSprites();
        }
        
        public void SwitchSprites()
        {
            if (_vibrate.IsEnableVibrate)
            {
                _targetImage.sprite = _onVibrate;
            }
            else
            {
                _targetImage.sprite = _offVibrate;
            }
        }
    }
}