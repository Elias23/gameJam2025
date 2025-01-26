using System.Linq;
using Player;
using UnityEngine;

namespace UI
{
    [System.Serializable]
    public class Background
    {
        public Sprite sprite;
        public int zIndex;
    }

    public class BackgroundManager : MonoBehaviour
    {
        [Header("Backgrounds")] [SerializeField]
        private Background[] backgrounds;


        [SerializeField, Range(0, 1)] public float topOffset;
        [SerializeField, Range(0, 1)] public float bottomOffset;

        [SerializeField, Range(0, 1)] private float parallaxStrength = 0.1f;
        [SerializeField] private float maxParallaxOffset = 0.1f;

        private GameObject[] backgroundObjects;
        private Vector3[] initialPositions;
        private float screenWidth;

        private void Start()
        {
            backgrounds = backgrounds.OrderBy(b => b.zIndex).ToArray();
            screenWidth = Core.GameBounds.Instance.GetWidth();

            var (top, bottom) = Core.GameBounds.Instance.GetGameBoundsWorldPos();
            var gameRect = new Rect(-screenWidth / 2f, bottom, screenWidth, top - bottom);

            backgroundObjects = new GameObject[backgrounds.Length];
            initialPositions = new Vector3[backgrounds.Length];

            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgroundObjects[i] = DrawBackgroundLayer(backgrounds[i], gameRect);
                initialPositions[i] = backgroundObjects[i].transform.position;
            }
        }

        private void Update()
        {
            var playerX = PlayerController.Instance.GetPlayerPosition().x;
            var maxOffset = screenWidth / 2;

            // Calculate normalized player position (-1 to 1)
            var normalizedPlayerPos = Mathf.Clamp(playerX / maxOffset, -1f, 1f);

            for (int i = 0; i < backgroundObjects.Length; i++)
            {
                var zFactor = Mathf.Abs(backgrounds[i].zIndex);
                var direction = Mathf.Sign(backgrounds[i].zIndex);

                // More movement for higher absolute z-index values
                var parallaxOffset = normalizedPlayerPos * parallaxStrength * zFactor * maxParallaxOffset * direction;

                var newPos = initialPositions[i];
                newPos.x += parallaxOffset;
                backgroundObjects[i].transform.position = newPos;
            }
        }

        private GameObject DrawBackgroundLayer(Background bg, Rect gameRect)
        {
            var spriteRect = bg.sprite.rect;
            var gameplayRegion = new Rect(
                0,
                spriteRect.height * bottomOffset,
                spriteRect.width,
                spriteRect.height * (1 - topOffset - bottomOffset)
            );

            var scaleY = gameRect.height / gameplayRegion.height * bg.sprite.pixelsPerUnit;
            var baseScaleX = scaleY;

            // Add small extra width for parallax (in world units)
            var extraWidth = maxParallaxOffset * 2;
            var totalDesiredWidth = gameRect.width + extraWidth;
            var currentWidth = (spriteRect.width / bg.sprite.pixelsPerUnit) * baseScaleX;
            var widthScale = totalDesiredWidth / currentWidth;

            var bgObject = new GameObject($"Background_{bg.zIndex}");
            var spriteRenderer = bgObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = bg.sprite;
            spriteRenderer.sortingOrder = bg.zIndex;

            if (bg.sprite.name.StartsWith("Overlay"))
            {
                // set alpha to 0.5 for overlay sprites
                spriteRenderer.color = new Color(1, 1, 1, 0.2f);
            }

            bgObject.transform.localScale = new Vector3(baseScaleX * widthScale, scaleY, 1);

            var centerOffsetY = (gameplayRegion.y + gameplayRegion.height / 2) - (spriteRect.y + spriteRect.height / 2);
            var yOffset = centerOffsetY / bg.sprite.pixelsPerUnit * scaleY;

            bgObject.transform.position = new Vector3(
                gameRect.x + gameRect.width / 2,
                gameRect.y + gameRect.height / 2 - yOffset,
                bg.zIndex
            );

            return bgObject;
        }
    }
}