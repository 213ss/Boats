using System;
using System.Collections;
using System.Collections.Generic;
using Code.Models;
using Code.Models.Runner;
using DG.Tweening;
using Runner;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code
{
    public class LevelComplete : MonoBehaviour
    {
        public static LevelComplete Instance { get; set; }

        [SerializeField] private GameObject window;
        [SerializeField] private Image back;
        [SerializeField] private GameObject holder;
        [SerializeField] private GameObject upgrateGarden;
        [SerializeField] private Button getButton;

        [SerializeField] private Image prototype;

        private List<Sprite> sprites;

        private void Awake()
        {
            Instance = this;
            sprites = new List<Sprite>();
        }


        public void AddObject(Sprite sprite)
        {
            var img = Instantiate(prototype.gameObject, holder.transform).GetComponentInChildren<Prototype>();
            img.Image.sprite = sprite;
            img.gameObject.SetActive(true);
            sprites.Add(sprite);
        }

        public void Show()
        {
            CollectedView.Instance.gameObject.SetActive(false);

            var fuck = sprites.Count > 0;

            getButton.interactable = fuck;

            if (!fuck)
            {
                StartCoroutine(LoadNextLevl());
            }

            window.SetActive(true);

            back.DOFade(0.5f, 1f)
                .OnComplete(() =>
                {
                    holder.SetActive(true);
                    getButton.gameObject.SetActive(true);
                    upgrateGarden.SetActive(true);
                });
        }

        public void GoToGarden()
        {
            SceneManager.LoadScene(1);
        }

        private IEnumerator LoadNextLevl()
        {
            yield return new WaitForSeconds(2f);

            int lvl = PlayerPrefs.GetInt("lvl", 2);

            if (lvl > SceneManager.sceneCountInBuildSettings - 1)
                lvl = 2;
            SceneManager.LoadScene(lvl);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<PlayerMovement>(out var player))
            {
                int curlvl = PlayerPrefs.GetInt("lvl", 2) + 1;

                if (curlvl > SceneManager.sceneCountInBuildSettings - 1)
                {
                    curlvl = 2;
                }

                PlayerPrefs.SetInt("lvl", curlvl);

                player.isStarted = false;
                player.Animator.SetTrigger("dance");
                Show();
            }
        }
    }
}