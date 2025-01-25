namespace Player
{
    using Core;
    using Unity.VisualScripting;
    using UnityEngine;
    using Update = UnityEngine.PlayerLoop.Update;

    public interface IInputHandler
    {
        void Update(Vector3 currentPosition);
        float GetMovementDirection();
        bool isShootingActionPressed();
    }

    public class DesktopInputHandler : IInputHandler
    {
        public void Update(Vector3 currentPosition)
        {
            // nop
        }
        public float GetMovementDirection()
        {
            var keyboardInput = Input.GetAxisRaw("Horizontal");
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
        private readonly (float top, float bottom) bounds;
        private readonly Camera camera;
        private readonly float touchMinDistanceThreshold;

        private float movementDirection;
        private bool isShootingPressed;

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
                    isShootingPressed = true;
                else
                    movementDirection += CalculateMovementDirection(touchPos, currentPlayerPos);
            }
        }

        private void ResetPerUpdate()
        {
            movementDirection = 0f;
            isShootingPressed = false;
        }

        public float GetMovementDirection() => movementDirection;

        public bool isShootingActionPressed() => isShootingPressed;
    }
}