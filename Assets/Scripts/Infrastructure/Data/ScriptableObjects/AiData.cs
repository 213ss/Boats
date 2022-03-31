using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.Data.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Tools/AI/AI data", fileName = "Ai Data")]
    public class AiData : ScriptableObject
    {
        public AiDifficulty Difficulty => _aiDifficulty;

        [SerializeField] private AiDifficulty _aiDifficulty;
        [SerializeField] private float _baseTimeScanGoldTrigger;
        [SerializeField] private float _baseTimePatrol;
        
        [Header("Attack parameters")]
        [SerializeField] private float _chanceAttack;
        [SerializeField] private float _timeReloadsAttack;

        private int _tryChange = 0;

        public float GetTimeScanGoldTrigger()
        {
            float timeToSearch = _baseTimeScanGoldTrigger;
            
            switch (_aiDifficulty)
            {
                case AiDifficulty.Lite:
                    timeToSearch = _baseTimeScanGoldTrigger;
                    break;
                case AiDifficulty.Medium:
                    timeToSearch -= _baseTimeScanGoldTrigger / 4.0f;
                    break;
                case AiDifficulty.Hard:
                    timeToSearch -= _baseTimeScanGoldTrigger / 3.0f;
                    break;
                case AiDifficulty.Madman:
                    timeToSearch -= _baseTimeScanGoldTrigger / 1.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return timeToSearch;
        }
        
        public bool IsSuccessfullyChanceDigGold()
        {
            float chance = 0.0f;
            float delta = 0.0f;
            
            switch (_aiDifficulty)
            {
                case AiDifficulty.Lite:
                    delta = 16.0f;
                    break;
                case AiDifficulty.Medium:
                    delta = 32.0f;
                    break;
                case AiDifficulty.Hard:
                    delta = 64.0f;
                    break;
                case AiDifficulty.Madman:
                    delta = 128.0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            chance = Random.Range(0.0f, 128.0f);
            
            ++_tryChange;
            delta += _tryChange;
            
            
            if (chance <= delta)
            {
                _tryChange = 0;
                return true;
            }
            
            return false;
        }

        public float GetTimePatrol()
        {
            float delta = _baseTimePatrol;
            
            switch (_aiDifficulty)
            {
                case AiDifficulty.Lite:
                    delta = _baseTimePatrol;
                    break;
                case AiDifficulty.Medium:
                    delta -= _baseTimePatrol / 4.0f;
                    break;
                case AiDifficulty.Hard:
                    delta -= _baseTimePatrol / 3.0f;
                    break;
                case AiDifficulty.Madman:
                    delta -= _baseTimePatrol / 1.5f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return delta;
        }

        public bool IsChanceAttack()
        {
            float random = Random.Range(0, 99.0f);
            if (random <= _chanceAttack)
                return true;

            return false;
        }

        public float GetTimeReloadsAttack()
        {
            return  _timeReloadsAttack;
        }
    }
}