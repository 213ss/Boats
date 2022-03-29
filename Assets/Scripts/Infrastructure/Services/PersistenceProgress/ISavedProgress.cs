using Infrastructure.Data;

public interface ISavedProgress : IProgressReader
{
    void UpdateProgress(PlayerProgress progress);
}

public interface IProgressReader
{
    void LoadProgress(PlayerProgress progress);
}
