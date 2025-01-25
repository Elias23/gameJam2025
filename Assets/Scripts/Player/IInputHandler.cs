using UnityEngine;

namespace Player
{
    using System;
    using Core;
    using Unity.Mathematics.Geometry;

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
        private readonly Camera camera;
        private readonly float touchMinDistanceThreshold;
        private readonly (float top, float bottom) bounds;

        public MobileInputHandler(float touchMinDistanceThreshold)
        {
            this.touchMinDistanceThreshold = touchMinDistanceThreshold;
            bounds = GameBounds.Instance.GetGameBoundsScreenPos();
            camera = Camera.main;
        }

        public float GetHorizontalInput(Vector3 currentPosition)
        {
            if (Input.touchCount <= 0)
                return 0;

            var touchPosScreen = Input.GetTouch(0).position;
            var touchPosWorld = camera.ScreenToWorldPoint(touchPosScreen);

            // check if bottom area of screen was touched
            if (!(touchPosScreen.y <= bounds.bottom))
                return 0;

            var distanceX = touchPosWorld.x - currentPosition.x;
            if (Mathf.Abs(distanceX) <= touchMinDistanceThreshold)
                return 0;

            return Mathf.Sign(distanceX);
        }

        public bool isShootingActionPressed()
        {
            // handled by UI button
            return false;
        }
    }
}