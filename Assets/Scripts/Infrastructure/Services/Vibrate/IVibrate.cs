namespace Infrastructure.Services.Vibrate
{
    public interface IVibrate
    {
        bool IsEnableVibrate { get; }
        void EnableVibrate();
        void DisableVibrate();
        void PlayVibrate(float time);
    }
}