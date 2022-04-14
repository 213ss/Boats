using Actors;
using Infrastructure.Data.ScriptableObjects;
using Infrastructure.Services.Game;

namespace Infrastructure.Services.LevelService
{
    public interface ILevelService
    {
        void RestartLevel();
        void GoToMainMenu();
        void QuitGame();
        void SetPlayerActor(Actor actor);
        Actor PlayerActor { get; }
        AiData AIParametersData { get; }
        int MaxLevelsCount { get; }
        int CurrentSceneIndex { get; }
        void SetGameService(GameService gameService);
    }
}