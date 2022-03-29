using Actors;
using UnityEngine;

namespace Logic.Triggers
{
    public class TriggerLayerSwitch : MonoBehaviour
    {
        [SerializeField] private int _layerIndexTo;

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.layer = _layerIndexTo;
            other.GetComponent<Actor>().IsHoleStay = true;
        }
    }
}
