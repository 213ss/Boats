

namespace Actors
{
    public interface IAnimationStateReader
    {
        void EnterState(int stateHash);
        void ExitedState(int stateHash);
        
        AnimatorState CurrentState { get; }
    }
}