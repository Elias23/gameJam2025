using Core;
using UnityEngine;

namespace UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private GameObject heartPrefab;
        [SerializeField] private float bottomYOffset = 1f;
        [SerializeField] private float heartSpacing = 1f;
        [SerializeField] private float heartPadding = 1f;

        private float screenHeight;
        private float screenWidth;
        private GameObject[] hearts;

        private void Start()
        {
            screenHeight = GameBounds.Instance.GetHeight();
            screenWidth = GameBounds.Instance.GetWidth();
            hearts = new GameObject[GameManager.Instance.GetMaxPlayerLife()];

            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i] = Instantiate(heartPrefab);
                hearts[i].SetActive(false);
            }
        }

        public void Update()
        {
            var currentHealth = GameManager.Instance.GetPlayerLife();
            DrawHearts(currentHealth);
        }

        private void DrawHearts(int healthCount)
        {
            float heartWidth = heartPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
            float startX = -screenWidth / 2 + heartPadding + heartWidth;

            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < healthCount)
                {
                    hearts[i].SetActive(true);
                    float xPos = startX + (i * (heartWidth + heartSpacing));
                    hearts[i].transform.position = new Vector3(xPos, -screenHeight / 2 + heartPadding + heartWidth, 6f);
                }
                else
                {
                    hearts[i].SetActive(false);
                }
            }
        }
    }
}