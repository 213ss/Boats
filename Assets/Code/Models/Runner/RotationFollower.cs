using System;
using UnityEngine;

namespace Code.Models.Runner
{
    public class RotationFollower : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            transform.rotation = target.rotation;
        }
    }
}