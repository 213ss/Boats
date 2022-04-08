using Actors.Actors;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.MovementFinalPoint
{
    public class NavMeshMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private PlayerActor _playerActor;

        public void GoTo(Vector3 position)
        {
            _playerActor.GoToFinalPoint();
            _agent.enabled = true;
            _agent.SetDestination(position);
        }
    }
}