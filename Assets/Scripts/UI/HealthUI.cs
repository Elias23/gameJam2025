using System.Collections;
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
        [SerializeField] private float destroyAnimationDuration = 1f;
        [SerializeField] private float fallSpeed = 3f;

        private float screenHeight;
        private float screenWidth;
        private GameObject[] hearts;
        private int lastHealthCount;

        private void Start()
        {
            screenHeight = GameBounds.Instance.GetHeight();
            screenWidth = GameBounds.Instance.GetWidth();
            hearts = new GameObject[GameManager.Instance.GetMaxPlayerLife()];
            lastHealthCount = GameManager.Instance.GetMaxPlayerLife();

            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i] = Instantiate(heartPrefab);
                hearts[i].SetActive(false);
            }
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
                else if (!hearts[i].GetComponent<SpriteRenderer>().enabled)
                {
                    hearts[i].SetActive(false);
                }
            }
        }


        public void Update()
        {
            var currentHealth = GameManager.Instance.GetPlayerLife();
            if (currentHealth < lastHealthCount)
            {
                StartCoroutine(DestroyHeart(lastHealthCount - 1));
            }

            lastHealthCount = currentHealth;
            DrawHearts(currentHealth);
        }


        private IEnumerator DestroyHeart(int heartIndex)
        {
            GameObject heart = hearts[heartIndex];
            SpriteRenderer sprite = heart.GetComponent<SpriteRenderer>();
            Vector3 startPos = heart.transform.position;
            Color startColor = sprite.color;
            float elapsedTime = 0f;

            while (elapsedTime < destroyAnimationDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / destroyAnimationDuration;

                // Fall down
                heart.transform.position = startPos + Vector3.down * (fallSpeed * elapsedTime);

                // Fade out
                sprite.color = new Color(startColor.r, startColor.g, startColor.b, 1 - progress);

                yield return null;
            }

            sprite.enabled = false;
        }
    }
}