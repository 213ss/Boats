using Infrastructure.Data;
using Infrastructure.Factory;
using UnityEngine;

namespace Infrastructure.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string PROGRESS_KEY = "PlayerProgress";
        
        private readonly IPersistenceProgressServices _progressServices;
        private readonly IGameFactory _gameFactory;


        public SaveLoadService(IPersistenceProgressServices progressServices, IGameFactory gameFactory)
        {
            _progressServices = progressServices;
            _gameFactory = gameFactory;
        }
        
        public void SaveProgress()
        {
            if (_progressServices.progress == null)
                _progressServices.progress = new PlayerProgress();
            
            _gameFactory.ProgressWriters.ForEach(
                x => x.UpdateProgress(_progressServices.progress));
            

            Debug.Log("Saved progress: " + _progressServices.progress.ToJson());
            
            PlayerPrefs.SetString(PROGRESS_KEY, _progressServices.progress.ToJson());
            PlayerPrefs.Save();
        }

        public PlayerProgress LoadProgress()
        {
            var playerProgress = GetPalyerProgress();
            
            _gameFactory.ProgressReaders.ForEach(x => x.LoadProgress(playerProgress));

            return playerProgress;
        }

        private PlayerProgress GetPalyerProgress()
        {
            if (PlayerPrefs.HasKey(PROGRESS_KEY))
            {
                var progressJson = PlayerPrefs.GetString(PROGRESS_KEY);
                
                Debug.Log("Loaded progress: " + progressJson);
                
                return progressJson.ToDesirialize<PlayerProgress>();
            }

            return new PlayerProgress();
        }
    }
}