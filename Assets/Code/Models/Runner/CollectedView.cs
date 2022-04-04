using System;
using UnityEngine;

namespace Code.Models.Runner
{
    public class CollectedView : MonoBehaviour
    {
        [SerializeField] private CollectedIconView prefab;
        public static CollectedView Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public void AddObject(Sprite sprite)
        {
            var newView = Instantiate(prefab.gameObject,prefab.transform.parent).GetComponent<CollectedIconView>();
            newView.Image.sprite = sprite;
            newView.gameObject.SetActive(true);
        }
    }
}