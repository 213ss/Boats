using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IDragHandler
{
    [SerializeField] private float maxZoom = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float zoomSensitivity = 1f;
    [SerializeField] private float dragSensitivity;
    [SerializeField] private float xBorder;
    [SerializeField] private float yBorder;
    [SerializeField] private GameObject tutor;

    private float _currentZoom;
    private float _fixedXBorder;
    private float _fixedYBorder;
    private Camera _camera;

    private bool isDOubleTouch = false;

    private float lastDistance = 0;

    private void Awake()
    {
        _camera = Camera.main;

        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
        _currentZoom = _camera.orthographicSize;
        
        var bordersZoomOffset = maxZoom - _currentZoom;

        _fixedXBorder = xBorder + bordersZoomOffset * 0.875f;
        _fixedYBorder = yBorder + bordersZoomOffset * 0.875f;
        //0.875
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            tutor.SetActive(false);
        }
        
        if (Input.touchCount > 1)
        {
            isDOubleTouch = true;

            var pos1 = Input.touches[0].position;
            var pos2 = Input.touches[1].position;
            var currentDistance = Vector2.Distance(pos1, pos2);

            _currentZoom += currentDistance < lastDistance ? zoomSensitivity : -zoomSensitivity;
            _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);
            _camera.orthographicSize = _currentZoom;

            var bordersZoomOffset = maxZoom - _currentZoom;

            _fixedXBorder = xBorder + bordersZoomOffset * 0.875f;
            _fixedYBorder = yBorder + bordersZoomOffset * 0.875f;
            
            var pos = _camera.transform.position ;

            pos.x = Mathf.Clamp(pos.x, -_fixedXBorder, _fixedXBorder);
            pos.y = Mathf.Clamp(pos.y, -_fixedYBorder, _fixedYBorder);
            _camera.transform.position = pos;
            
            lastDistance = currentDistance;
        }
        else
        {
            isDOubleTouch = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDOubleTouch)
            return;

        var pos = _camera.transform.position + -(Vector3)eventData.delta * dragSensitivity;

        pos.x = Mathf.Clamp(pos.x, -_fixedXBorder, _fixedXBorder);
        pos.y = Mathf.Clamp(pos.y, -_fixedYBorder, _fixedYBorder);

        _camera.transform.position = pos;
    }
}