using Core;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float screenEdgeBuffer = 0.5f;

        [Header("Bubble")] [SerializeField] private GameObject bubblePrefab;

        public static PlayerController Instance { get; private set; }

        private float _minX;
        private float _maxX;
        private Camera _mainCamera;
        private Vector2 _screenBounds;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            CalculateScreenBounds();

            var (top, bottom) = GameBounds.Instance.GetGameBoundsWorldPos();
            transform.position = Vector3.up * bottom;
        }

        public Vector3 GetPlayerPosition() => transform.position;

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