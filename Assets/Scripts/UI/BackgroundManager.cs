using System.Linq;
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

        private void Start()
        {
            backgrounds = backgrounds.OrderBy(b => b.zIndex).ToArray();

            var (top, bottom) = Core.GameBounds.Instance.GetGameBoundsWorldPos();
            var width = Core.GameBounds.Instance.GetWidth();

            // in world space
            var gameRect = new Rect(-width / 2f, bottom, width, top - bottom);

            foreach (var bg in backgrounds)
            {
                DrawBackgroundLayer(bg, gameRect);
            }
        }

        private void DrawBackgroundLayer(Background bg, Rect gameRect)
        {
            var spriteRect = bg.sprite.rect;
            var gameplayRegion = new Rect(
                0,
                spriteRect.height * bottomOffset,
                spriteRect.width,
                spriteRect.height * (1 - topOffset - bottomOffset)
            );

            var scaleY = gameRect.height / gameplayRegion.height * bg.sprite.pixelsPerUnit;
            var scaleX = scaleY;

            var bgObject = new GameObject($"Background_{bg.zIndex}");
            var spriteRenderer = bgObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = bg.sprite;
            spriteRenderer.sortingOrder = bg.zIndex;

            bgObject.transform.localScale = new Vector3(scaleX, scaleY, 1);

            var centerOffsetY = (gameplayRegion.y + gameplayRegion.height / 2) - (spriteRect.y + spriteRect.height / 2);
            var yOffset = centerOffsetY / bg.sprite.pixelsPerUnit * scaleY;

            bgObject.transform.position = new Vector3(
                gameRect.x + gameRect.width / 2,
                gameRect.y + gameRect.height / 2 - yOffset,
                bg.zIndex
            );
        }
    }
}