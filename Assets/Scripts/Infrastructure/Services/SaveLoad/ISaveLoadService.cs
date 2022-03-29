using Infrastructure.Data;

namespace Infrastructure.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        public void SaveProgress();
        public PlayerProgress LoadProgress();
    }
}