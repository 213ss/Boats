using System.Collections.Generic;
using Infrastructure.Data.ScriptableObjects;
using Logic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Services.Game
{

    public class GameService : MonoBehaviour
    {
        public int ScenesCount => _sceneNames.Count;
        public int CurrentSceneIndex => _currentSceneIndex;
        
        [SerializeField] private LoadingCurtain _curtain;

        private List<string> _sceneNames = new List<string>();

        private int _currentSceneIndex = -1;

        private LevelReferences _levelReferences;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            _levelReferences = Resources.Load<LevelReferences>("Data/LevelReferences");
            _sceneNames.AddRange(_levelReferences.GetSceneNames());
            
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            LoadNextLevel();
        }

        public void LoadNextLevel()
        {
            _curtain.Show();

            int nextSceneIndex = ++_currentSceneIndex;
            if (nextSceneIndex < _sceneNames.Count)
            {
                SceneManager.LoadScene(_sceneNames[nextSceneIndex]);
            }
            else
            {
                _curtain.Hide();
            }
        }

        public void RestartLevel()
        {
            _curtain.Show();
            SceneManager.LoadScene(_sceneNames[_currentSceneIndex]);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            _curtain.Hide();
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}