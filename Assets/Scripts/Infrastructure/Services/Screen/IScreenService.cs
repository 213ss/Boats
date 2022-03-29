namespace Infrastructure.Services.Screen
{
    public interface IScreenService
    {
        void ShowPressTapScreen();
        void ShowWinScreen();
        void ShowLoseScreen();
        void HideAllScreens();
    }
}