using UnityEngine;

namespace Ship
{
    public class ShipSinkingBehaviour : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private float sinkSpeed = 2f;
        private float rotationSpeed = 30f;
        private float currentRotation = 0f;

        public void Initialize(SpriteRenderer shipSprite)
        {
            spriteRenderer = shipSprite;
        }

        private void Update()
        {
            // Sink downward
            transform.position += Vector3.down * (sinkSpeed * Time.deltaTime);

            // Rotate as it sinks
            currentRotation += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);

            var color = spriteRenderer.color;
            color.a = Mathf.Max(0, color.a - (Time.deltaTime * 0.3f));
            spriteRenderer.color = color;

            // randomly spawn bubbles
            if (Random.value < 0.07f && color.a >= 0.25f)
            {
                var pos = transform.position + Random.Range(-0.1f, 0.2f) * Vector3.right;
                ProjectileManager.Instance.SpawnBubble(pos, Random.Range(0.5f, 0.8f));
            }

            // Destroy when fully sunk
            if (color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}