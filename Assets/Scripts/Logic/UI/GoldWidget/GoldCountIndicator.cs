using System;
using Actors;
using Scripts.Infrastructure.Services.Gold;
using UnityEngine;
using Zenject;

namespace Logic.UI.GoldWidget
{
    public class GoldCountIndicator : MonoBehaviour
    {
        [SerializeField] private Transform _goldWidgetParent;
        [SerializeField] private GoldWidget _goldWidgetPrefab;
        [SerializeField] private Vector2 _offsetWidget;

        [SerializeField] private Transform _ownerTransform;

        private Actor _owner;
        private GoldWidget _goldWidget;
        private IUIFollow _uiFollow;
        private bool _isActor;
        
        [Inject]
        private void Construct(IUIFollow uiFollow)
        {
            _uiFollow = uiFollow;
        }
        
        private void Start()
        {
            if (_ownerTransform == null)
                _ownerTransform = GetComponent<Transform>();

            _owner = _ownerTransform.GetComponent<Actor>();
            if (_owner != null) _isActor = true;

            if (_goldWidgetParent == null)
                _goldWidgetParent = GameObject.FindGameObjectWithTag("UIFollowArea").transform;
            
            
            _goldWidget = Instantiate(_goldWidgetPrefab, _goldWidgetParent);
            _goldWidget.transform.SetAsFirstSibling();
            
            _goldWidget.Construct(_ownerTransform.GetComponentInParent<IGold>());

            EnableIndicator();
        }

        private void Update()
        {
            if(_isActor && _owner.IsDroppedOutGame)
                DisableIndicator();
        }

        public void EnableIndicator()
        {
            _uiFollow.TryAddUIFollowObject(_goldWidget.GetComponent<RectTransform>(), _ownerTransform, _offsetWidget);
            _goldWidget.gameObject.SetActive(true);
        }

        public void DisableIndicator()
        {
            _uiFollow.DisableFollowAndReturn(_ownerTransform);
            _goldWidget.gameObject.SetActive(false);
        }
    }
}
