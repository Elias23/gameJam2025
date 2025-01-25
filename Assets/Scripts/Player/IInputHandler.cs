﻿using UnityEngine;

namespace Player
{
    public interface IInputHandler
    {
        float GetHorizontalInput(Vector3 currentPosition);
        bool isShootingActionPressed();
    }

    public class DesktopInputHandler : IInputHandler
    {
        public float GetHorizontalInput(Vector3 currentPosition)
        {
            float keyboardInput = Input.GetAxisRaw("Horizontal");
            if (keyboardInput != 0)
                return keyboardInput;

            return (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        }

        public bool isShootingActionPressed()
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
    }

    public class MobileInputHandler : IInputHandler
    {
        private Camera camera;
        private readonly float touchThreshold;

        public MobileInputHandler(float touchThreshold)
        {
            this.touchThreshold = touchThreshold;
        }

        public float GetHorizontalInput(Vector3 currentPosition)
        {
            // cache camera reference
            if (!camera) camera = Camera.main;

            if (Input.touchCount <= 0) return 0;

            var touch = Input.GetTouch(0);
            var screenPosition = camera.WorldToScreenPoint(currentPosition);
            var distanceX = touch.position.x - screenPosition.x;

            // Slowing down the movement when the touch is closer to the center of the screen
            var threshold = Screen.width * touchThreshold;
            var normalizedDistance = Mathf.Clamp01(Mathf.Abs(distanceX) / threshold);

            return Mathf.Sign(distanceX) * normalizedDistance;
        }

        public bool isShootingActionPressed()
        {
            // handled by UI button
            return false;
        }
    }
}