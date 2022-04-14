using System.Collections.Generic;
using Logic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.Game
{

    public class GameService : MonoBehaviour
    {
        public int ScenesCount => _scenes.Count;
        public int CurrentSceneIndex => _currentSceneIndex;
        
        [SerializeField] private List<SceneAsset> _scenes = new List<SceneAsset>();
        [SerializeField] private LoadingCurtain _curtain;
        
        private int _currentSceneIndex;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            LoadNextLevel();
        }

        public void LoadNextLevel()
        {
            _curtain.Show();

            if (_currentSceneIndex == 0)
            {
                SceneManager.LoadScene(_scenes[_currentSceneIndex].name);
            }
            else
            {
                int nextSceneIndex = ++_currentSceneIndex;
                if (nextSceneIndex < _scenes.Count)
                {
                    SceneManager.LoadScene(_scenes[nextSceneIndex].name);
                }
                else
                {
                    _curtain.Hide();
                }
            }
        }

        public void RestartLevel()
        {
            _curtain.Show();
            SceneManager.LoadScene(_scenes[_currentSceneIndex].name);
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode sceneMode)
        {
            _curtain.Hide();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}