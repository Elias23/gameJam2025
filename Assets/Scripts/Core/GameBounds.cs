using UnityEngine;

namespace Core
{
    public class GameBounds : MonoBehaviour
    {
        [Header("Game Bounds")] [SerializeField]
        private float targetAspectRatio = 0.9f;

        [SerializeField] private float targetVerticalOffset = 0.1f;
        [SerializeField] private bool drawBounds = true;

        // Singleton
        public static GameBounds Instance { get; private set; }

        // cache camera reference
        private Camera mainCamera;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                mainCamera = Camera.main;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            if (drawBounds)
            {
                var bounds = GetGameBoundsWorldPos();
                Debug.DrawLine(new Vector3(-1000, bounds.top, 0), new Vector3(1000, bounds.top, 0), Color.red);
                Debug.DrawLine(new Vector3(-1000, bounds.bottom, 0), new Vector3(1000, bounds.bottom, 0), Color.red);
            }
        }

        public (float top, float bottom) GetGameBoundsWorldPos()
        {
            var width = mainCamera.orthographicSize * 2 * mainCamera.aspect;
            var targetHeight = width / targetAspectRatio;
            var offset = targetHeight * targetVerticalOffset;

            return (
                top: mainCamera.transform.position.y + (targetHeight / 2) + offset,
                bottom: mainCamera.transform.position.y - (targetHeight / 2) + offset
            );
        }

        public (float top, float bottom) GetGameBoundsScreenPos()
        {
            var worldBounds = GetGameBoundsWorldPos();
            var top = mainCamera.WorldToScreenPoint(Vector3.up * worldBounds.top).y;
            var bottom = mainCamera.WorldToScreenPoint(Vector3.up * worldBounds.bottom).y;
            return (top, bottom);
        }

        public float GetWidth()
        {
            return mainCamera.orthographicSize * 2 * mainCamera.aspect;
        }
    }
}
