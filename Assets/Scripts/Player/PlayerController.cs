using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float screenEdgeBuffer = 0.5f;
        [SerializeField] private float touchThreshold = 0.2f;

        [Header("Position")] [SerializeField] private float bottomOffset = 1f;

        private float _minX;
        private float _maxX;
        private Camera _mainCamera;
        private Vector2 _screenBounds;

        private List<IInputHandler> _inputHandlers;

        private void Awake()
        {
            _inputHandlers = new List<IInputHandler>
            {
                new DesktopInputHandler(),
                new MobileInputHandler(touchThreshold)
            };
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            CalculateScreenBounds();
            PositionPlayerAtStart();
        }

        private void Update()
        {
            // Adding up all the horizontal inputs from all the input handlers
            float horizontalInput = _inputHandlers.Sum(handler => handler.GetHorizontalInput(transform.position));

            MovePlayer(horizontalInput);
        }

        private void CalculateScreenBounds()
        {
            _screenBounds =
                _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                    _mainCamera.transform.position.z));
            _minX = -_screenBounds.x + screenEdgeBuffer;
            _maxX = _screenBounds.x - screenEdgeBuffer;
        }

        private void PositionPlayerAtStart()
        {
            var startY = -_screenBounds.y + bottomOffset;
            transform.position = new Vector3(0f, startY, 0f);
        }

        private void MovePlayer(float horizontalInput)
        {
            var movement = new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);
            var newPosition = transform.position + movement;
            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
            transform.position = newPosition;
        }
    }
}