using Cinemachine;
using Scripts.Infrastructure.Services.Camera;
using UnityEngine;

public class CameraFollow : MonoBehaviour, ICameraFollow
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    
    public void SetFollow(Transform followTransform)
    {
        _camera.m_Follow = followTransform;
        _camera.m_LookAt = followTransform;
    }
}
