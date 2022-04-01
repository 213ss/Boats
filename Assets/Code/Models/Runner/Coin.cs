using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Code.Models.Runner
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private float speed;
        public Transform target;
        public bool isCollected = false;

        private void Update()
        {
            if (isCollected)
            {
                var pos = transform.position;
                pos.x = target.position.x;
                transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
                //transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,
                    //target.rotation.eulerAngles,
                    //speed * Time.deltaTime));
            }
        }
    }
}