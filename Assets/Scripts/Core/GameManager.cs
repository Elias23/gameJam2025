using UnityEngine;

namespace Core
{
    using System.Collections;
    using System.Collections.Generic;

    public enum GameState
    {
        Playing,
        GameOver,
        GameCongratulation
    }

    public class GameStateInfo
    {
        public GameState GameState { get; private set; }
        public float ShipHealth { get; private set; }
        public int PlayerLife { get; private set; }
        public float TimeScale { get; private set; }

        public GameStateInfo(GameState gameState, float shipHealth, int playerLife, float timeScale)
        {
            GameState = gameState;
            ShipHealth = shipHealth;
            PlayerLife = playerLife;
            TimeScale = timeScale;
        }
    }

    public class GameManager : MonoBehaviour
    {
        public GameObject gameOverUI;
        public GameObject gameCongratulationUI;

        [Header("Game Settings")] [SerializeField]
        private float shipMaxHealth = 100;

        [SerializeField] private int playerMaxLife = 3;

        [Header("Game Prefabs")] [SerializeField]
        GameObject TutorialManagerPrefab;

        public static GameManager Instance { get; private set; }

        public GameState GameState { get; private set; }

        private float shipHealth;
        private int playerLife;
        private Camera mainCamera;

        public GameStateInfo GetGameDebugInfo()
        {
            return new GameStateInfo(GameState, shipHealth, playerLife, Time.timeScale);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                GameState = GameState.Playing;
                InitializeTutorial();

                shipHealth = shipMaxHealth;
                playerLife = playerMaxLife;
                mainCamera = Camera.main;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (GameState != GameState.Playing)
            {
                // slow motion
                Time.timeScale = 0.5f;
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }

        public void HandleShipDamage(float damage)
        {
            if (GameState != GameState.Playing)
            {
                return;
            }

            StartCoroutine(ShakeScreen(0.2f, 0.05f));
            SoundManager.Instance.PlayHitShipSound();

            TutorialManager.Instance?.HandleTutorialEvent(EventCountStep.TutorialEvent.GarbageRepelled);

            shipHealth -= damage;
            if (shipHealth <= 0)
            {
                ShowGameCongratulationScreen();
            }
        }

        public void HandleGarbageDropped()
        {
            if (GameState != GameState.Playing)
            {
                return;
            }

            SoundManager.Instance.PlayHitGroundSound();
            StartCoroutine(ShakeScreen(0.2f, 0.3f));

            playerLife--;
            if (playerLife <= 0)
            {
                ShowGameOverScreen();
            }
        }

        private void InitializeTutorial()
        {
            if (!GameStore.IsTutorialCompleted)
            {
                Instantiate(TutorialManagerPrefab);
                GameStore.IsTutorialCompleted = true;
            }
        }

        private void ShowGameOverScreen()
        {
            GameState = GameState.GameOver;
            gameOverUI.SetActive(true);
        }

        private void ShowGameCongratulationScreen()
        {
            GameState = GameState.GameCongratulation;
            gameCongratulationUI.SetActive(true);
            PlayerState.Instance.isWin();
        }

        public float GetShipHealthPercentage()
        {
            return shipHealth / shipMaxHealth;
        }

        public IEnumerator ShakeScreen(float duration, float magnitude)
        {
            var originalPos = mainCamera.transform.localPosition;

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
                float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

                mainCamera.transform.localPosition = new Vector3(xOffset, yOffset, originalPos.z);

                elapsedTime += Time.deltaTime;
                // Wait for one frame
                yield return null;
            }
            mainCamera.transform.localPosition = originalPos;
        }

    }
}