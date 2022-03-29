using Actors;
using Infrastructure.AssetManagment;
using Infrastructure.Data.ScriptableObjects;
using UnityEngine;

namespace Logic.AI.AC_BaseAI
{
    public class PatrolState : StateMachineBehaviour
    {
        public Actor _ownerActor;
        private AiData _aiData;

        private float _timerScan;

        private void Awake()
        {
            _aiData = Resources.Load<AiData>(AssetsPath.AiTriggerSearchData);
            _timerScan = _aiData.GetTimeScanGoldTrigger() + Random.Range(0, 3.0f);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_ownerActor.CurrentIsland.IsLast)
            {
                animator.SetTrigger("MoveToFinalPoint");
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_timerScan >= 0.0f)
            {
                _timerScan -= Time.deltaTime;
            }

            if (CheckGoldToTravel())
            {
                animator.SetTrigger("Travel");
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        private bool CheckGoldToTravel()
        {
            return _ownerActor.GoldService.CurrentCount >= _ownerActor.CurrentIsland.CostDelivery;
        }
    }
}
