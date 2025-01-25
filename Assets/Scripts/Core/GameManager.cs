using UnityEngine;

namespace Core
{
    public enum GameState
    {
        Playing,
        GameOver,
        GameCongratulation
    }

    public class GameManager : MonoBehaviour
    {
        public GameObject gameOverUI;
        public GameObject gameCongratulationUI;

        [Header("Game Settings")] [SerializeField]
        private float shipHealth = 1;

        [SerializeField] private int playerLife = 3;

        public static GameManager Instance { get; private set; }

        public GameState GameState { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                GameState = GameState.Playing;
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

            //TODO reactivate playerLife--;
            if (playerLife <= 0)
            {
                ShowGameOverScreen();
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
        }
    }
}