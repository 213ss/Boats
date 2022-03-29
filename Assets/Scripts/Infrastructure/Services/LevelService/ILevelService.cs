namespace Infrastructure.Services.LevelService
{
    public interface ILevelService
    {
        void RestartLevel();
        void GoToMainMenu();
        void QuitGame();
    }
}