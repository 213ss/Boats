using Actors;
using Infrastructure.Data.ScriptableObjects;

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
    }
}