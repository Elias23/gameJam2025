using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DebugUI : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private TextMeshProUGUI debugText;
        [SerializeField] private float updateInterval = 0.5f;

        private float timer;

        private void Start()
        {
            if (debugText == null)
            {
                Debug.LogError("Debug Text reference not set!");
                enabled = false;
                return;
            }
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                timer = 0f;
                UpdateDebugInfo();
            }
        }

        private void UpdateDebugInfo()
        {
            var info = GameManager.Instance.GetGameDebugInfo();
            debugText.text = $"Game State: {info.GameState}\n" +
                             $"Ship Health: {info.ShipHealth:F1}\n" +
                             $"Lives: {info.PlayerLife}\n" +
                             $"Time Scale: {info.TimeScale:F2}x";
        }
#endif
    }
}