using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runner
{
    public class InputCatcher : MonoBehaviour, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private Transform turnTarget;
        [SerializeField] private float border = 4f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private GameObject tapText;
        [SerializeField] private GameObject tutorAnim;
        [SerializeField] private GameObject gardenButton;
        [SerializeField] private float sensitivity;

        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void OnDrag(PointerEventData eventData)
        {
            /*var ray = _camera.ScreenPointToRay(eventData.position);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
            {
                var pos = hit.point;
                var newTargetPos = turnTarget.position;
                newTargetPos.x += Mathf.Clamp(hit.point.x, -border, border);
                turnTarget.position = newTargetPos;
            }*/
            
            
            var newTargetPos = turnTarget.position;
            newTargetPos.x += eventData.delta.x * sensitivity;
            newTargetPos.x = Mathf.Clamp(newTargetPos.x, -border, border);
            turnTarget.position = newTargetPos;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            tapText.SetActive(false);
            gardenButton.SetActive(false);
            tutorAnim.SetActive(false);

            playerMovement.Go();
        }
    }
}