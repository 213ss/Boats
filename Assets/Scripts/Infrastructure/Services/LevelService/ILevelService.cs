using Actors;

namespace Infrastructure.Services.LevelService
{
    public interface ILevelService
    {
        void RestartLevel();
        void GoToMainMenu();
        void QuitGame();
        void SetPlayerActor(Actor actor);
        Actor PlayerActor { get; }
    }
}