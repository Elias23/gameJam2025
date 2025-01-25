using UnityEngine;

namespace Testing
{
    public class BubbleSpawner : MonoBehaviour
    {
        [Header("Bubble Prefabs")] [SerializeField]
        private GameObject bubblePrefab;

        private Camera camera;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SpawnBubble();
            }
        }

        private void Awake()
        {
            camera = Camera.main;
        }

        private void SpawnBubble()
        {
            var mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            Instantiate(bubblePrefab, mousePosition, Quaternion.identity);
        }
    }
}