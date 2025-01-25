using System.Runtime.CompilerServices;
using Assets.RequiredField.Scripts;
using Core;
using UnityEngine;

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
        float screenWidth = Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height;
        float screenHeight = Camera.main.orthographicSize * 2.0f;

        float worldWidth = screenWidth * (1 - 2 * horizontalBorderPercentage);
        float worldHeight = screenHeight * (1 - 2 * verticalBorderPercentage);

        GameObject borders = new GameObject("Borders");
        borders.transform.SetParent(gameWorld.transform);
       
        // Left border
        CreateBorder(new Vector2(-worldWidth / 2 - borderThickness / 2, 0), new Vector2(borderThickness, worldHeight), borders,"side");
    
        // Right border
        CreateBorder(new Vector2(worldWidth / 2 + borderThickness / 2, 0), new Vector2(borderThickness, worldHeight), borders,"side");

        // Top border
        CreateBorder(new Vector2(0, bounds.top + borderThickness / 2), new Vector2(worldWidth, borderThickness), borders, "top");

        // Bottom border
        CreateBorder(new Vector2(0, bounds.bottom - borderThickness / 2), new Vector2(worldWidth, borderThickness), borders,"bottom");
    }

    void CreateBorder(Vector2 position, Vector2 size,GameObject borders,string name)
    {
        GameObject border = new GameObject(name);
        border.transform.SetParent(border.transform);
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
