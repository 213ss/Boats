using UnityEngine;
using UnityEngine.UI;

namespace Code
{
    public class Prototype : MonoBehaviour
    {
        [SerializeField] private Image image;

        public Image Image => image;
    }
}