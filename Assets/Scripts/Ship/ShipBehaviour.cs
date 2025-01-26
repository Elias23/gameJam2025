using System.Collections.Generic;
using System.Linq;
using Core;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

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

        [SerializeField] private float weightIncreasePerMin = 100f;
        [SerializeField] private float minDropInterval = 1f;
        [SerializeField] private float heavyItemCooldown = 5f;
        [SerializeField] private int recentWeightsMemory = 15;
        [SerializeField] private float spawnYOffset = 15;
        [SerializeField] private float initialGarbageDelay = 2f;

        [Header("Sprites")] [SerializeField] private Sprite fullLifeSprite;
        [SerializeField] private Sprite lessThanTwoThirdsLifeSprite;
        [SerializeField] private Sprite lessThanOneThirdLifeSprite;
        [SerializeField] private Sprite sinkingSprite;

        [Header("Garbage Items")] [SerializeField]
        private List<GameObject> garbagePrefabs;


        private GarbageSpawner garbageSpawner;

        private Vector3 targetPosition;
        private float waitTimer;
        private bool isWaiting = true;
        private float topBounds;
        private float time;
        private SpriteRenderer spriteRenderer;
        private bool hasReachedInitialPosition = false;
        private float garbageDelayTimer;
        private bool canSpawnGarbage;

        private bool firstFrame = true;

        private void Awake()
        {
            var garbageItems = garbagePrefabs
                .Select(prefab =>
                    prefab.GetComponent<Garbage>() is { } g
                        ? new GarbageItem(g.GetWeight(), g.spawnProbability, prefab)
                        : null)
                .Where(gi => gi != null).ToList();

            Debug.LogWarning("Registered garbage items: " + garbageItems.Count);

            garbageSpawner = new GarbageSpawner(garbageItems, baseWeightPerMin, weightIncreasePerMin,
                minDropInterval, heavyItemCooldown, recentWeightsMemory);
        }

        private void Start()
        {
            var (top, bottom) = GameBounds.Instance.GetGameBoundsWorldPos();
            var screenBounds = GameBounds.Instance.GetWidth() / 2;

            // Start position outside screen
            transform.position = new Vector3(-screenBounds * 2, top - yOffSet, 0);
            topBounds = top - yOffSet;

            // Initial target in middle
            targetPosition = new Vector3(0, top - yOffSet, 0);
            isWaiting = false;

            // Cache the sprite renderer
            spriteRenderer = GetComponent<SpriteRenderer>();

            // Delay garbage spawning until ship reaches initial position
            garbageDelayTimer = initialGarbageDelay;
            canSpawnGarbage = false;
        }

        private void Update()
        {
            if (firstFrame)
            {
                SoundManager.Instance.PlayShipHornSound();
                firstFrame = false;
            }

            if (GameManager.Instance.GameState == GameState.GameCongratulation)
            {
                Sink();
                return;
            }

            UpdateSprite();

            MoveShip();

            TrySpawnGarbage();
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

        private void TrySpawnGarbage()
        {
            if (hasReachedInitialPosition)
            {
                if (!canSpawnGarbage)
                {
                    garbageDelayTimer -= Time.deltaTime;
                    if (garbageDelayTimer <= 0)
                    {
                        canSpawnGarbage = true;
                    }

                    return;
                }

                garbageSpawner.Update(Time.deltaTime);
                if (garbageSpawner.ShouldDropNewGarbage(Time.time))
                {
                    var garbage = garbageSpawner.SelectGarbageType(Time.time);
                    DropGarbage(garbage);
                }
            }
            else if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                hasReachedInitialPosition = true;
            }
        }

        private void DropGarbage(GarbageItem garbage)
        {
            // set spawn position
            Vector3 spawnPosition = transform.position - new Vector3(0, spawnYOffset, 0);
            var garbageObject = Instantiate(garbage.Prefab, spawnPosition, Quaternion.identity);
            garbageObject.transform.SetParent(transform.parent);

            // add initial velocity
            var body = garbageObject.GetComponent<Rigidbody2D>();
            var weightClass = garbage.WeightClass;
            setInitialVelocity(garbage, body, weightClass);

            SoundManager.Instance.PlayDropGarbageSound();
        }

        private static void setInitialVelocity(GarbageItem garbage, Rigidbody2D body, float weightClass)
        {
            body.linearVelocityY = getInitialVelocity(weightClass);
            if (garbage.Prefab.name.Contains("Container"))
                body.linearVelocityY = -2.5f;
        }

        private static float getInitialVelocity(float weightClass) =>
            weightClass switch
            {
                <= 5 => -2f,
                <= 10 => -1f,
                <= 15 => -0.8f,
                _ => -0.2f
            };

        private void Sink()
        {
            // Add sinking component and disable this one
            spriteRenderer.sprite = sinkingSprite;
            var sinkingComponent = gameObject.AddComponent<ShipSinkingBehaviour>();
            sinkingComponent.Initialize(spriteRenderer);
            enabled = false;
        }

        private void UpdateSprite()
        {
            if (!spriteRenderer) return;

            var health = GameManager.Instance.GetShipHealthPercentage();

            if (health > 0.66f)
            {
                spriteRenderer.sprite = fullLifeSprite;
            }
            else if (health > 0.33f)
            {
                spriteRenderer.sprite = lessThanTwoThirdsLifeSprite;
            }
            else
            {
                spriteRenderer.sprite = lessThanOneThirdLifeSprite;
            }
        }
    }
}