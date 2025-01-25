using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    using System;
    using UnityEngine.UIElements;

    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float screenEdgeBuffer = 0.5f;

        private float _minX;
        private float _maxX;
        private Camera _mainCamera;
        private Vector2 _screenBounds;

        private void Start()
        {
            _mainCamera = Camera.main;
            CalculateScreenBounds();
        }



        public void ShootProjectile()
        {
            Debug.Log("Pew");
        }

        private void CalculateScreenBounds()
        {
            _screenBounds =
                _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                    _mainCamera.transform.position.z));
            _minX = -_screenBounds.x + screenEdgeBuffer;
            _maxX = _screenBounds.x - screenEdgeBuffer;
        }

        public void MovePlayer(float horizontalInput)
        {
            var movement = new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);
            var newPosition = transform.position + movement;
            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
            transform.position = newPosition;
        }
    }
}