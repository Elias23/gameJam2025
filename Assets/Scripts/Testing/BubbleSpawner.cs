using UnityEngine;

namespace Testing
{
    public class BubbleSpawner : MonoBehaviour
    {
        [Header("Bubble Prefabs")] [SerializeField]
        private GameObject bubblePrefab;

        private Camera mainCamera;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnBubble();
            }
        }

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void SpawnBubble()
        {
            var mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Instantiate(bubblePrefab, mousePosition, Quaternion.identity);
        }
    }
}