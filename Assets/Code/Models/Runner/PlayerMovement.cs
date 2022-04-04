using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Runner
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float turnSpeed = 5f;
        [SerializeField] private float viewTurnTime = 0.2f;
        [SerializeField] private float angle = 40f;
        [SerializeField] private float delta;
        [SerializeField] private Transform turnTarget;
        [SerializeField] private Transform viewTransform;

        private Animator _animator;
        private Rigidbody _rigidbody;

        public bool isStarted = false;

        public Animator Animator => _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Go()
        {
            if(!isStarted)
            _animator.SetTrigger("walk");
            isStarted = true;
        }

        private void Update()
        {
            if (!isStarted)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }

            var pos = transform.position;
            pos.x = turnTarget.position.x;

            Debug.Log(_rigidbody.velocity);

            //var myPos = viewTransform.position;
            //var fixedTargetPos = turnTarget.position;
            //fixedTargetPos.x = transform.position.x;
            //var angle = Vector3.Angle(myPos, fixedTargetPos);
            //viewTransform.DORotate(viewTransform.rotation.eulerAngles + Vector3.up * angle, viewTurnTime);

            /*var myX = transform.position.x;
            var targetX = pos.x;
            if (myX - delta > targetX)
            {
                viewTransform.DORotate(Vector3.up * -angle, viewTurnTime);
            }
            else if(myX  < targetX - delta)
            {
                viewTransform.DORotate(Vector3.up * angle, viewTurnTime);
            }
            else
            {
                viewTransform.DORotate(Vector3.up , viewTurnTime);
            }
*/
            transform.position = Vector3.Lerp(transform.position, pos, turnSpeed * Time.deltaTime);

            _rigidbody.velocity = transform.forward * speed;
        }
    }
}