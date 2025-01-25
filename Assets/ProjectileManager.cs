using Assets.RequiredField.Scripts;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField, RequiredField] private GameObject bubblePrefab;

    [Header("Charge Shot")]
    [SerializeField, RequiredField] private float chargeSpeed;
    [SerializeField, RequiredField] private float maxCharge;

    public static ProjectileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private bool isCharging = false;
    private float currentChargeLevel = 1f;

    public void ChargeProjectile()
    {
        // TODO
    }

    public void FireProjectile()
    {

    }
}
