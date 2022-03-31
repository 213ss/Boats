using Infrastructure.Services.Islands;
using TMPro;
using UnityEngine;
using Zenject;

namespace Logic.UI.IslandText
{
    public class IslandsTextUpdate : MonoBehaviour
    {
        [SerializeField] private TMP_Text _islandsText;
        
        private IIslandService _islandService;

        [Inject]
        private void Construct(IIslandService islandService)
        {
            _islandService = islandService;
        }
        
        private void Start()
        {
            _islandService.EventActorNewIsland += OnActorNewIsland;
            UpdateIslandText(1);
        }

        private void OnActorNewIsland(int indexIsland)
        {
            UpdateIslandText(indexIsland + 1);
        }

        private void UpdateIslandText(int islandIndex)
        {
            _islandsText.text = "Islands " + islandIndex + "/" + _islandService.GetCountIslands();
        }

        private void OnDestroy()
        {
            _islandService.EventActorNewIsland -= OnActorNewIsland;
        }
    }
}
