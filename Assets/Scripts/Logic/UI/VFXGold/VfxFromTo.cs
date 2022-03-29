using System.Collections;
using UnityEngine;

public class VfxFromTo : MonoBehaviour, IVfxFromTo
{
    [SerializeField] private int _amount;
    [SerializeField] private float _lifeTime;
    [SerializeField] private float _intervalRate;
    [SerializeField] private iTween.EaseType _easeType;
    [SerializeField] private RectTransform _originPoint;
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private Vector3 _offset;

    [SerializeField] private GameObject _uiGoldVFXPrefab;

    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
    }

    public void StartAnimFromTo(Vector3 startPoint)
    {
        StartCoroutine(AnimFromTo(startPoint));
    }

    public void StartAnimFromTo(Transform originPoint)
    {
        StartCoroutine(AnimFromTo(originPoint.position, originPoint));
    }

    private IEnumerator AnimFromTo(Vector3 startPoint, Transform originPoint = null)
    {
        if (originPoint == null)
        {
            SetOriginPositionAt(startPoint);
        }
        
        for (int i = 0; i < _amount; ++i)
        {
            if (originPoint != null)
            {
                SetOriginPositionAt(originPoint.position);
            }
            
            var goldVFX = Instantiate(_uiGoldVFXPrefab, _originPoint);
            iTween.MoveTo(goldVFX, iTween.Hash("position", 
                _destinationPoint.position + _offset, 
                "easetype", _easeType, 
                "ignoretimescale", true, "time", _lifeTime));
            

            Destroy(goldVFX, _lifeTime + 1);
            yield return new WaitForSeconds(_intervalRate);

        }
    }
    
    private void SetOriginPositionAt(Vector3 startPoint)
    {
        _originPoint.localPosition = Vector3.zero;
        
        Vector2 positionOnScreen = RectTransformUtility.WorldToScreenPoint(_camera, startPoint);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_originPoint, positionOnScreen, null,
            out var anchoredPosition);

        _originPoint.anchoredPosition = anchoredPosition;   
    }
}
