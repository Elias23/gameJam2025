using UnityEngine;

namespace Player
{
    using System;
    using NUnit.Framework.Internal;

    public interface IInputHandler
    {
        float GetHorizontalInput(Vector3 currentPosition);
    }

    public class DesktopInputHandler : IInputHandler
    {
        public float GetHorizontalInput(Vector3 currentPosition)
        {
            float keyboardInput = Input.GetAxisRaw("Horizontal");
            Debug.Log($"Keyboard input: {keyboardInput}");
            if (keyboardInput != 0) return keyboardInput;

            return (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        }

        public bool isShootingActionPressed()
        {
            return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        }
    }

    public class MobileInputHandler : IInputHandler
    {
        private Camera _camera;
        private float _touchThreshold;

        public MobileInputHandler(float touchThreshold)
        {
            _touchThreshold = touchThreshold;
        }

        public float GetHorizontalInput(Vector3 currentPosition)
        {
            // cache camera reference
            if (!_camera) _camera = Camera.main;

            if (Input.touchCount <= 0) return 0;

            var touch = Input.GetTouch(0);
            var screenPosition = _camera.WorldToScreenPoint(currentPosition);
            var distanceX = touch.position.x - screenPosition.x;

            // Slowing down the movement when the touch is closer to the center of the screen
            var threshold = Screen.width * _touchThreshold;
            var normalizedDistance = Mathf.Clamp01(Mathf.Abs(distanceX) / threshold);

            return Mathf.Sign(distanceX) * normalizedDistance;
        }
    }
}