using Actors;

namespace Logic.AI.States
{
    public class AiDroppedOutGameState : IState
    {
        private readonly AiStateMachine _stateMachine;
        private readonly Actor _aiOwner;


        public AiDroppedOutGameState(AiStateMachine stateMachine, Actor aiOwner)
        {
            _stateMachine = stateMachine;
            _aiOwner = aiOwner;
        }
        
        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}