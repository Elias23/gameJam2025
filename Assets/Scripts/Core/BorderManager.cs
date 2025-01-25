using Assets.RequiredField.Scripts;
using UnityEngine;

namespace Core
{
    public class BorderManager : MonoBehaviour
    {
        public const string Top = "Top Border";
        public const string Bottom = "Bottom Border";
        [SerializeField, RequiredField] public GameObject gameWorld;
        private GameBounds gameBounds;
        private (float top, float bottom) bounds;
        public float borderThickness = 1.0f;
        public float horizontalBorderPercentage = 0.1f; // 10% of the screen width
        public float verticalBorderPercentage = 0.1f; // 10% of the screen height
        public bool drawColliders = true; // Flag to make drawing optional
        [SerializeField, RequiredField] public Sprite borderSprite; // Assign a sprite in the Inspector

        private Camera mainCamera;

        void Awake()
        {
            mainCamera = Camera.main;
        }

        void Start()
        {
            bounds = GameBounds.Instance.GetGameBoundsWorldPos();

            if (Application.isPlaying)
            {
                CreateBorderColliders();
            }
        }

        void CreateBorderColliders()
        {
            var screenWidth = mainCamera.orthographicSize * 2.0f * Screen.width / Screen.height;
            var screenHeight = mainCamera.orthographicSize * 2.0f;

            var worldWidth = screenWidth * (1 - 2 * horizontalBorderPercentage);
            var worldHeight = screenHeight * (1 - 2 * verticalBorderPercentage);

            GameObject borders = new GameObject("Borders");
            borders.transform.SetParent(gameWorld.transform);

            // Left border
            CreateBorder(new Vector2(-worldWidth / 2 - borderThickness / 2, 0),
                new Vector2(borderThickness, worldHeight), borders, "side");

            // Right border
            CreateBorder(new Vector2(worldWidth / 2 + borderThickness / 2, 0),
                new Vector2(borderThickness, worldHeight), borders, "side");

            // Top border
            CreateBorder(new Vector2(0, bounds.top + borderThickness / 2), new Vector2(worldWidth, borderThickness),
                borders, Top);

            // Bottom border
            CreateBorder(new Vector2(0, bounds.bottom - borderThickness / 2), new Vector2(worldWidth, borderThickness),
                borders, Bottom);
        }

        void CreateBorder(Vector2 position, Vector2 size, GameObject borders, string name)
        {
            GameObject border = new GameObject(name);
            border.tag = name;
            border.transform.SetParent(borders.transform);
            border.transform.position = position;
            BoxCollider2D newCollider = border.AddComponent<BoxCollider2D>();
            Rigidbody2D newRigidbody = border.AddComponent<Rigidbody2D>();
            newRigidbody.bodyType = RigidbodyType2D.Static;
            newCollider.size = size;

            if (drawColliders && borderSprite != null)
            {
                SpriteRenderer renderer = border.AddComponent<SpriteRenderer>();
                renderer.sprite = borderSprite;
                renderer.drawMode = SpriteDrawMode.Sliced;
                renderer.size = size;
            }
        }
    }
}