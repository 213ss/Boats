using Actors;
using Logic.Island;

namespace Infrastructure.Services.Islands
{
    public interface IIslandService
    {
        void SetActorToStartIsland(Actor actor);
        void SetActorToNextIsland(Actor actor);
        void RemoveActorFromIsland(Actor actor);
        BaseIsland TryGetIslandForActor(Actor actor);
    }
}