using System;
using UnityEngine;

namespace Runner
{
    public class TargetFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float speedZ;
        [SerializeField] private float speedX;
        private void Update()
        {
            var followerNewPosition = target.position + offset;
            followerNewPosition.x = transform.position.x;
            transform.position = Vector3.Lerp(transform.position, followerNewPosition, speedZ);
            
            followerNewPosition.x = (target.position + offset).x;
            transform.position = Vector3.Lerp(transform.position, followerNewPosition, speedX);
        }
    }
}