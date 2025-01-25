using Assets.RequiredField.Scripts;
using UnityEngine;

namespace Core
{
    public class BorderManager : MonoBehaviour
    {
        [SerializeField, RequiredField] public GameObject gameWorld;
        private GameBounds gameBounds;
        private (float top, float bottom) bounds;
        public float borderThickness = 1.0f;
        public float horizontalBorderPercentage = 0.1f; // 10% of the screen width
        public float verticalBorderPercentage = 0.1f; // 10% of the screen height
        public bool drawColliders = true; // Flag to make drawing optional
        [SerializeField, RequiredField] public Sprite borderSprite; // Assign a sprite in the Inspector

        private Camera camera;

        void Start()
        {
            bounds = GameBounds.Instance.GetGameBoundsWorldPos();
            camera = Camera.main;

            if (Application.isPlaying)
            {
                CreateBorderColliders();
            }
        }

        void CreateBorderColliders()
        {
            var screenWidth = camera.orthographicSize * 2.0f * Screen.width / Screen.height;
            var screenHeight = camera.orthographicSize * 2.0f;

            var worldWidth = screenWidth * (1 - 2 * horizontalBorderPercentage);
            var worldHeight = screenHeight * (1 - 2 * verticalBorderPercentage);

            var borders = new GameObject("Borders");
            borders.transform.SetParent(gameWorld.transform);

            // Left border
            CreateBorder(new Vector2(-worldWidth / 2 - borderThickness / 2, 0),
                new Vector2(borderThickness, worldHeight),
                borders);

            // Right border
            CreateBorder(new Vector2(worldWidth / 2 + borderThickness / 2, 0),
                new Vector2(borderThickness, worldHeight),
                borders);

            // Top border
            CreateBorder(new Vector2(0, bounds.top + borderThickness / 2), new Vector2(worldWidth, borderThickness),
                borders);

            // Bottom border
            CreateBorder(new Vector2(0, bounds.bottom - borderThickness / 2), new Vector2(worldWidth, borderThickness),
                borders);
        }

        void CreateBorder(Vector2 position, Vector2 size, GameObject borders)
        {
            GameObject border = new GameObject("Border");
            border.transform.SetParent(transform);
            border.transform.position = position;
            BoxCollider2D collider = border.AddComponent<BoxCollider2D>();
            collider.size = size;

            if (drawColliders && borderSprite != null)
            {
                SpriteRenderer renderer = border.AddComponent<SpriteRenderer>();
                renderer.sprite = borderSprite;
                renderer.drawMode = SpriteDrawMode.Sliced;
                renderer.size = size;
            }
        }

        void Update()
        {
        }
    }
}