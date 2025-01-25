using UnityEngine;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public GameObject gameOverUI;    
        [Header("Game Settings")] [SerializeField]
        private float shipHealth = 100f;

        [SerializeField] private int playerLife = 3;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void HandleShipDamage(float damage)
        {
            shipHealth -= damage;
            if (shipHealth <= 0)
            {
                // Game Over
            }
        }

        public void HandleGarbageDropped()
        {
            playerLife--;
            if (playerLife <= 0)
            {
                gameOver();
            }
        }

        public void gameOver()
        {
            gameOverUI.SetActive(true);
        }
    }
}