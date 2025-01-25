using Assets.RequiredField.Scripts;
using JetBrains.Annotations;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField, RequiredField] private GameObject bubblePrefab;
    [SerializeField, RequiredField] private PlayerController player;

    [Header("Charge Shot")] [SerializeField]
    private float chargeSpeed;

    [SerializeField] private float maxCharge = 2.5f;

    [CanBeNull] private GameObject currentHeldBubble = null;

    private bool isCharging = false;
    private float currentChargeLevel = 1f;

    public static ProjectileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (currentHeldBubble == null)
            return;

        var bubble = currentHeldBubble.GetComponent<BubbleProjectile>();
        currentChargeLevel += (chargeSpeed * Time.deltaTime);
        currentChargeLevel = Mathf.Min(currentChargeLevel, maxCharge);
        bubble.setSize(currentChargeLevel);
    }

    public void ChargeProjectile()
    {
        // spawn bubble in front of player
        currentChargeLevel = 1;
        currentHeldBubble = Instantiate(bubblePrefab, player.transform);
        currentHeldBubble.transform.localPosition += new Vector3(-0.2f, 1.04f, 0);
    }

    public void FireProjectile()
    {
        if (currentHeldBubble == null)
        {
            Debug.LogWarning("Trying to fire a bubble even though none is charging");
            return;
        }

        // release bubble
        var bubble = currentHeldBubble.GetComponent<BubbleProjectile>();
        bubble.ReleaseCharge();
        currentChargeLevel = 1;
    }
}