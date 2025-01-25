using Core;
using UnityEngine;

namespace Ship
{
    public class ShipBehaviour : MonoBehaviour
    {
        [Header("Movement")] [SerializeField] private float horizontalSpeed = 5f;
        [SerializeField] private float waveAmplitude = 0.1f;
        [SerializeField] private float waveFrequency = 1f;
        [SerializeField] private float minWaitTime = 0.5f;
        [SerializeField] private float maxWaitTime = 2f;
        [SerializeField] private float screenEdgeBuffer = 0.5f;
        [SerializeField] private float yOffSet = 0.2f;

        [Header("Garbage Spawner")] [SerializeField]
        private float baseWeightPerMin = 10f;

        [SerializeField] private float weightIncreasePerMin = 10f;
        [SerializeField] private float minDropInterval = 1f;
        [SerializeField] private float heavyItemCooldown = 5f;
        [SerializeField] private int recentWeightsMemory = 15;


        private GarbageSpawner garbageSpawner;

        private Vector3 targetPosition;
        private float waitTimer;
        private bool isWaiting = true;
        private float topBounds;
        private float time;
        private SpriteRenderer spriteRenderer;

        private void Start()
        {
            var (top, bottom) = GameBounds.Instance.GetGameBoundsWorldPos();
            transform.position = Vector3.up * top;
            topBounds = transform.position.y - yOffSet;

            // Initialize the garbage spawner
            garbageSpawner = new GarbageSpawner(baseWeightPerMin, weightIncreasePerMin, minDropInterval, 5f, 5);

            // Cache the sprite renderer
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            MoveShip();

            if (garbageSpawner.ShouldDropNewGarbage(Time.time))
            {
                var garbage = garbageSpawner.SelectGarbageType(Time.time);
                DropGarbage(garbage);
            }
        }

        private void MoveShip()
        {
            time += Time.deltaTime;

            // Calculate vertical wave motion
            var yOffset = Mathf.Sin(time * waveFrequency) * waveAmplitude;

            if (isWaiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    isWaiting = false;

                    // calculate new target position
                    var screenBounds = GameBounds.Instance.GetWidth() / 2;
                    var targetX = Random.Range(-screenBounds + screenEdgeBuffer, screenBounds - screenEdgeBuffer);

                    targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
                }
            }
            else
            {
                // Move
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    horizontalSpeed * Time.deltaTime
                );

                // Set sprite direction based on movement

                if (spriteRenderer)
                {
                    spriteRenderer.flipX = transform.position.x < targetPosition.x;
                }

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    isWaiting = true;
                    waitTimer = Random.Range(minWaitTime, maxWaitTime);
                }
            }

            // Apply vertical wave motion
            var currentPos = transform.position;
            currentPos.y = topBounds + yOffset;
            transform.position = currentPos;
        }

        private void DropGarbage(GarbageItem garbage)
        {
            // Ignore for now
        }
    }
}