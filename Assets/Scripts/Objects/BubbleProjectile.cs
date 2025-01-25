using Assets.RequiredField.Scripts;
using Core;
using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    [Header("Properties")] [SerializeField, RequiredField]
    private float baseSpeed = 5f;

    [SerializeField, RequiredField] private float sideMovementAmplitude = 1f;
    [SerializeField, RequiredField] private float sideMovementFrequency = 2f;
    [SerializeField, RequiredField] private float wobbleStrength = 0.2f;
    [SerializeField, RequiredField] private float sizeModifier = 1f;

    private float topBounds;
    private float timeOffset;
    private Vector3 startPosition;

    void Start()
    {
        transform.localScale *= sizeModifier;
        var (top, _) = GameBounds.Instance.GetGameBoundsWorldPos();
        topBounds = top;

        // Random starting phase for varied movement
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
        startPosition = transform.position;
    }

    void Update()
    {
        var time = Time.time + timeOffset;

        // Basic upward movement
        var newPosition = transform.position + Vector3.up * (baseSpeed * Time.deltaTime);

        // Sine wave horizontal movement
        var horizontalOffset = Mathf.Sin(time * sideMovementFrequency) * sideMovementAmplitude;

        // Add some random wobble
        var randomWobble = Mathf.PerlinNoise(time, timeOffset) * 2f - 1f;
        horizontalOffset += randomWobble * wobbleStrength;

        // Apply horizontal movement
        newPosition.x = startPosition.x + horizontalOffset;
        transform.position = newPosition;

        if (transform.position.y > topBounds)
        {
            HandleTopCollision();
        }
    }

    private void HandleTopCollision()
    {
        Destroy(gameObject);
    }
}