using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Logic.UI.UISpriteAnimation
{
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private float _timeToSwitch;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _startOnPlay;

        [SerializeField] private Image _targetImage;

        private Coroutine _coroutine;
    
        private void Start()
        {
            if (_targetImage == null)
                _targetImage = GetComponent<Image>();

            if (_startOnPlay)
                StartAnimn();
        }

        public void StartAnimn()
        {
            if(_coroutine == null)
                _coroutine = StartCoroutine(AnimSpriteSwap());
        }

        private IEnumerator AnimSpriteSwap()
        {
            do
            {
                for (int i = 0; i < _sprites.Length; ++i)
                {
                    _targetImage.sprite = _sprites[i];
                    yield return new WaitForSeconds(_timeToSwitch);
                }
            } while (_loop);
        }
    
    }
}
