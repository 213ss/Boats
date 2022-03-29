using Actors;
using Logic.GoldLoot;
using UnityEngine;

public class TriggerHole : MonoBehaviour
{
    [SerializeField] private GameObject goldLootsPrefab;
    [SerializeField] private float _shardCount;
    [SerializeField] private Vector3 _spawnOffset;

    private Transform _transform;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var actor = other.GetComponentInParent<Actor>();
        if (actor != null)
        {
            float gold = actor.GoldService.CurrentCount;
            
            if(gold <= 0.0f) return;

            actor.GoldService.SubstractionGold(gold);
            float goldPerShard = gold / _shardCount;
            
            for (int i = 0; i < _shardCount; ++i)
            {
                GameObject loots = Instantiate(goldLootsPrefab, _transform.position + _spawnOffset, Quaternion.identity);
                GoldLoots goldLoot = loots.GetComponentInChildren<GoldLoots>();
                goldLoot.SetGold(goldPerShard);
                goldLoot.AddForce();
            }
            
            enabled = false;
        }
    }
}
