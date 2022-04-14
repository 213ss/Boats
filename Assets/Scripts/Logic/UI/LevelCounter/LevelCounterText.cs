using Infrastructure.Services.LevelService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Logic.UI.LevelCounter
{
    public class LevelCounterText : MonoBehaviour
    {
        [SerializeField] private Text _countText;
        
        private ILevelService _levelService;


        [Inject]
        public void Construct(ILevelService levelService)
        {
            _levelService = levelService;
        }

        private void Start()
        {
            if (_countText == null)
                _countText = GetComponent<Text>();

            _countText.text = "LEVELS: " + (_levelService.CurrentSceneIndex + 1) + "/" +
                              _levelService.MaxLevelsCount.ToString();
        }
    }
}