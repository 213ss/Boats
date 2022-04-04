using System;
using Code.Models;
using Code.Models.Runner;
using Runner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Runner
{
    public class Gate : MonoBehaviour
    {
        [SerializeField, Tooltip("require if GateManager.GenerateLevel is false")] private int cost;
        [SerializeField, Tooltip("require if GateManager.GenerateLevel is false")] private GurdenObjectEnum type;
        [SerializeField, Tooltip("require if GateManager.GenerateLevel is false")] private TMP_Text costText;

        [SerializeField, Tooltip("require if GateManager.GenerateLevel is false")] private Image img;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform target;
        [SerializeField] private Image icon;
        private GateManager _gateManager;
        private GardenObjectModel _model;

        /// <summary>
        /// Invoked if used level generation
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Init(GateManager gateManager, GardenObjectModel model)
        {
            _gateManager = gateManager;
            if (!_gateManager.GenerateLevel)
                return;

            _model = model;

            costText.text = model.Cost.ToString();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<CoinsCollector>(out var collector))
            {
                var cost = _gateManager.GenerateLevel ? _model.Cost : this.cost;
                var type = _gateManager.GenerateLevel ? _model.Type : this.type;
                
                if (collector.CollectedCoins >= cost)
                {
                    LevelComplete.Instance.AddObject(img.sprite);
                    CollectedView.Instance.AddObject(icon.sprite);
                    collector.CollectedCoins -= cost;
                    collector.Buy(cost, false, target);
                    _gateManager.Collect(type);
                    animator.SetTrigger("sale");
                }
                else
                {
                    animator.SetTrigger("faild");
                    collector.NoMoneyPing();
                }
            }
        }
    }
}