using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Services.IslandIndicator
{
    public class IndicatorSlot : MonoBehaviour
    {
        [SerializeField] private Image _target;


        public void SetSprite(Sprite sprite)
        {
            _target.sprite = sprite;
        }
    }
}