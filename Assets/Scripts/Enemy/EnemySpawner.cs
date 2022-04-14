using Actors;
using Infrastructure.AssetManagment;
using Infrastructure.Factory;
using Infrastructure.Services.Islands;
using Infrastructure.Services.SkinChanger;
using Scripts.Infrastructure.Data;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Team _team;
        [SerializeField] private Vector3 _offsetSpawn;

        [Header("Component references")] 
        [SerializeField] private Transform _spawnTransform;
        
        private ISkinChanger _skinChanger;
        private IIslandService _islandService;
        private IGameFactory _gameFactory;

        [Inject]
        private void Construct(ISkinChanger skinChanger, IIslandService islandService, IGameFactory gameFactory)
        {
            _skinChanger = skinChanger;
            _islandService = islandService;
            _gameFactory = gameFactory;
        }
        
        private void Start()
        {
            if (_spawnTransform == null)
                _spawnTransform = GetComponent<Transform>();
            
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            GameObject enemyGameObject = _gameFactory.CreateGameObject(AssetsPath.EnemyActorPath, _spawnTransform.position + _offsetSpawn);
            Actor enemyActor = enemyGameObject.GetComponent<Actor>();
            
            enemyActor.SetTeam(_team);
            _islandService.SetActorToStartIsland(enemyActor);
            _skinChanger.SetSkin((int)_team, enemyActor.GetComponent<ISkin>());
        }


#if UNITY_EDITOR

        [Header("Only editor mode")] 
        public Color PointColor;
        public float PointRadius;
        [Space]
        public bool IsDisplaying = true;

        private void OnDrawGizmos()
        {
            if (IsDisplaying)
            {
                Gizmos.color = PointColor;
                Gizmos.DrawSphere(transform.position, PointRadius);
            }
        }


#endif
    }
}