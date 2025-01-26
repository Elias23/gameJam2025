using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private List<AudioClip> bubbleSounds;
    [SerializeField] private List<AudioClip> hitGroundSounds;
    [SerializeField] private List<AudioClip> hitShipSounds;
    [SerializeField] private List<AudioClip> dropGarbageSounds;
    [SerializeField] private AudioClip menuButtonSound;
    [SerializeField] private AudioClip shipHornSound;

    [SerializeField] private AudioClip mainThemeSound;

    private Camera mainCamera;

    private AudioSource mainTheme;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            mainTheme = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMainTheme();
    }

    private void PlayMainTheme()
    {
        mainTheme.volume = 0.25f;
        mainTheme.clip = mainThemeSound;
        mainTheme.loop = true;

        mainTheme.Play();
    }

    private Camera GetMainCamera()
    {
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }

        return mainCamera;
    }

    public void PlayBubbleSound()
    {
        var randomIndex = Random.Range(0, bubbleSounds.Count);
        AudioSource.PlayClipAtPoint(bubbleSounds[randomIndex], GetMainCamera().transform.position);
    }

    public void PlayHitGroundSound()
    {
        var randomIndex = Random.Range(0, hitGroundSounds.Count);
        AudioSource.PlayClipAtPoint(hitGroundSounds[randomIndex], GetMainCamera().transform.position);
    }

    public void PlayHitShipSound()
    {
        var randomIndex = Random.Range(0, hitShipSounds.Count);
        AudioSource.PlayClipAtPoint(hitShipSounds[randomIndex], GetMainCamera().transform.position);
    }

    public void PlayMenuButtonSound()
    {
        AudioSource.PlayClipAtPoint(menuButtonSound, GetMainCamera().transform.position);
    }

    public void PlayShipHornSound()
    {
        AudioSource.PlayClipAtPoint(shipHornSound, GetMainCamera().transform.position);
    }

    public void PlayDropGarbageSound()
    {
        var randomIndex = Random.Range(0, dropGarbageSounds.Count);
        AudioSource.PlayClipAtPoint(dropGarbageSounds[randomIndex], GetMainCamera().transform.position);
    }
}