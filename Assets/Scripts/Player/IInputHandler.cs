﻿using Core;
using UnityEngine;

namespace Player
{
    public interface IInputHandler
    {
        void Update(Vector3 currentPosition);
        float GetMovementDirection();
        bool isShootingActionPressed();
        bool isShootingActionReleased();
    }

    public class DesktopInputHandler : IInputHandler
    {
        public void Update(Vector3 currentPosition)
        {
            // nop
        }

        public float GetMovementDirection()
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.RightArrow))
            {
                TutorialManager.Instance?.HandleTutorialEvent(EventCountStep.TutorialEvent.MovementKeysPressed);
            }

            var keyboardInput = Input.GetAxisRaw("Horizontal");
            if (keyboardInput != 0)
                return keyboardInput;

            return (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        }

        public bool isShootingActionPressed() =>
            Input.GetKeyDown(KeyCode.Space);

        public bool isShootingActionReleased() =>
            Input.GetKeyUp(KeyCode.Space);
    }

    public class MobileInputHandler : IInputHandler
    {
        private readonly (float top, float bottom) bounds;
        private readonly Camera camera;
        private readonly float touchMinDistanceThreshold;

        private float movementDirection;
        private bool isShootingPressed;
        private bool isShootingReleased;

        public MobileInputHandler(float touchMinDistanceThreshold)
        {
            this.touchMinDistanceThreshold = touchMinDistanceThreshold;
            bounds = GameBounds.Instance.GetGameBoundsScreenPos();
            camera = Camera.main;
        }

        private float CalculateMovementDirection(Vector3 touchPosScreen, Vector3 currentPosition)
        {
            var touchPosWorld = camera.ScreenToWorldPoint(touchPosScreen);

            var distanceX = touchPosWorld.x - currentPosition.x;
            if (Mathf.Abs(distanceX) <= touchMinDistanceThreshold)
                return 0;

            return Mathf.Sign(distanceX);
        }

        public void Update(Vector3 currentPlayerPos)
        {
            ResetPerUpdate();

            if (Input.touchCount <= 0)
                return;

            foreach (var touch in Input.touches)
            {
                var touchPos = touch.position;
                if (touchPos.y > bounds.bottom)
                {
                    HandleShootingAction(touch);
                }
                else
                {
                    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        TutorialManager.Instance?.HandleTutorialEvent(EventCountStep.TutorialEvent.MovementKeysPressed);
                    }

                    movementDirection += CalculateMovementDirection(touchPos, currentPlayerPos);
                }
            }
        }

        private void HandleShootingAction(Touch touch)
        {
            var touchPhase = touch.phase;
            isShootingPressed |= touchPhase == TouchPhase.Began;
            isShootingReleased |= touchPhase == TouchPhase.Ended;
        }

        private void ResetPerUpdate()
        {
            movementDirection = 0f;
            isShootingPressed = false;
            isShootingReleased = false;
        }

        public float GetMovementDirection() =>
            movementDirection;

        public bool isShootingActionPressed() =>
            isShootingPressed;

        public bool isShootingActionReleased() =>
            isShootingReleased;
    }
}