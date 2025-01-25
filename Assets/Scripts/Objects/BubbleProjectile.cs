using Assets.RequiredField.Scripts;
using Core;
using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    [Header("Properties")] [SerializeField, RequiredField]
    private float baseSpeed = 5f;

    [SerializeField, RequiredField] private float sideMovementAmplitude = 0.7f;
    [SerializeField, RequiredField] private float sideMovementFrequency = 2f;
    [SerializeField, RequiredField] private float wobbleStrength = 0.4f;
    [SerializeField, RequiredField] private float wobbleRampUpTime = 1f;

    [Header("Sprites")] [SerializeField] private Sprite[] sprites;

    private float topBounds;
    private float timeOffset;
    private Vector3 startPosition;
    private bool isCharging = true;
    private float startTime;
    private float lastHorizontalOffset;
    private float size;

    void Start()
    {
        var (top, _) = GameBounds.Instance.GetGameBoundsWorldPos();
        topBounds = top;

        // Random starting phase for varied movement
        timeOffset = Random.Range(0f, 2f * Mathf.PI);

        // Choose random sprite
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    public void SetSize(float size)
    {
        this.size = size;
        transform.localScale = Vector3.one * size;
    }

    public void ReleaseCharge()
    {
        isCharging = false;
        startPosition = transform.position;
        startTime = Time.time;
        lastHorizontalOffset = 0f;
    }

    void Update()
    {
        // do not move yet while charging
        if (isCharging)
            return;

        var time = Time.time + timeOffset;
        var timeSinceRelease = Time.time - startTime;
        var effectStrength = Mathf.Min(timeSinceRelease / wobbleRampUpTime, 1f) / transform.localScale.magnitude;

        var scaledSpeed = baseSpeed * ((transform.localScale.x - 1f) / 2f + 1);
        var newPosition = transform.position + Vector3.up * (scaledSpeed * Time.deltaTime);

        var targetHorizontalOffset = Mathf.Sin(time * sideMovementFrequency) * sideMovementAmplitude;
        targetHorizontalOffset += (Mathf.PerlinNoise(time, timeOffset) * 2f - 1f) * wobbleStrength;

        lastHorizontalOffset = Mathf.Lerp(lastHorizontalOffset, targetHorizontalOffset, effectStrength);
        newPosition.x = startPosition.x + lastHorizontalOffset;
        transform.position = newPosition;

        if (transform.position.y > topBounds)
        {
            HandleTopCollision();
        }
    }

    public float GetSize()
    {
        return size;
    }

    private void HandleTopCollision()
    {
        Destroy(gameObject);
    }
}