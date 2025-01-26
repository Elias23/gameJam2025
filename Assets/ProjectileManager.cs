using Assets.RequiredField.Scripts;
using Core;
using JetBrains.Annotations;
using Player;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField, RequiredField] private GameObject bubblePrefab;

    [Header("Charge Shot")] [SerializeField]
    private float chargeSpeed;

    [SerializeField] private float maxCharge = 2.5f;

    [CanBeNull] private BubbleProjectile currentBubbleScript = null;
    private float currentChargeLevel = 1f;

    public static ProjectileManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            ResetCharge();
        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (!currentBubbleScript)
            return;

        var bubble = currentBubbleScript;
        currentChargeLevel += (chargeSpeed * Time.deltaTime);
        currentChargeLevel = Mathf.Min(currentChargeLevel, maxCharge);
        bubble.SetSize(currentChargeLevel);
    }

    public void ChargeProjectile()
    {
        // spawn bubble in front of player
        currentChargeLevel = 1;
        var bubbleObject = Instantiate(bubblePrefab, PlayerController.Instance.transform);
        currentBubbleScript = bubbleObject.GetComponent<BubbleProjectile>();

        bubbleObject.transform.localPosition += new Vector3(-0.2f, 1.04f, 0);
    }

    public void FireProjectile()
    {
        if (!currentBubbleScript)
        {
            Debug.LogWarning("Trying to fire a bubble even though none is charging");
            return;
        }

        // release bubble
        currentBubbleScript.ReleaseCharge();


        // Tutorial event: normal shot
        TutorialManager.Instance?.HandleTutorialEvent(EventCountStep.TutorialEvent.BubbleShot);


        // Tutorial event: charged shot
        if (maxCharge - currentChargeLevel < 0.1f)
        {
            TutorialManager.Instance?.HandleTutorialEvent(EventCountStep.TutorialEvent.ChargedBubbleShot);
        }

        SoundManager.Instance.PlayBubbleSound();

        ResetCharge();
    }

    private void ResetCharge()
    {
        currentBubbleScript = null;
        currentChargeLevel = 1;
    }

    public void SpawnBubble(Vector3 position, float scale)
    {
        var bubble = Instantiate(bubblePrefab, position, Quaternion.identity);
        var bubbleScript = bubble.GetComponent<BubbleProjectile>();
        bubbleScript.SetSize(scale);

        SoundManager.Instance.PlayBubbleSound();
        bubbleScript.ReleaseCharge();
    }
}