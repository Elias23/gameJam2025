using Assets.RequiredField.Scripts;
using Core;
using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    [Header("Properties")] [SerializeField, RequiredField]
    private float moveSpeed = 5f;

    [SerializeField, RequiredField] private float sizeModifier = 1f;

    private float topBounds;

    void Start()
    {
        transform.localScale *= sizeModifier;

        var (top, bottom) = GameBounds.Instance.GetGameBounds();
        topBounds = top;
    }

    void Update()
    {
        transform.position += Vector3.up * (moveSpeed * Time.deltaTime);

        // check out of bounds
        if (transform.position.y > topBounds)
        {
            // TODO: Handle Reached Top Bounds

            Destroy(gameObject);
        }
    }
}