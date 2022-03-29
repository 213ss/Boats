using Enemy;
using Logic.GoldLoot;
using UnityEngine;

public class SearchCoins : MonoBehaviour
{
    [SerializeField] private LayerMask _layerCoins;
    [SerializeField] private Vector3 _radiusSearch;

    private Collider[] _overlapColliders =new Collider[10];
    private Transform _thisTransform;

    private Transform _target;
    private IAIMovement _movement;

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _movement = GetComponent<IAIMovement>();
    }

    private void Update()
    {
        int overlapCount =
            Physics.OverlapBoxNonAlloc(_thisTransform.position,
                _radiusSearch, 
                _overlapColliders, 
                Quaternion.identity, 
                _layerCoins);

        if (overlapCount > 0 && _target == null)
        {
            for (int i = 0; i < overlapCount; ++i)
            {
                if (_overlapColliders[i].TryGetComponent<GoldLoots>(out var loots))
                {
                    _target = loots.transform;
                    _movement.SetDestination(_target.position);
                }
            }
        }
    }
    
    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _radiusSearch.x / 2);
    }

#endif
}
