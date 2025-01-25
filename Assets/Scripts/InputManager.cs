using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float touchMinDistanceThreshold = 0.2f;
    private List<IInputHandler> inputHandlers;
    private ProjectileManager projectileManager;

    private void Start()
    {
        inputHandlers = new List<IInputHandler>
        {
            new DesktopInputHandler(),
            new MobileInputHandler(touchMinDistanceThreshold)
        };
        projectileManager = ProjectileManager.Instance;
    }

    private void Update()
    {
        inputHandlers.ForEach(it => it.Update(PlayerController.Instance.GetPlayerPosition()));

        float horizontalInput = inputHandlers.Sum(it => it.GetMovementDirection());
        PlayerController.Instance.MovePlayer(horizontalInput);

        if (inputHandlers.Any(input => input.isShootingActionPressed()))
            projectileManager.ChargeProjectile();
        if (inputHandlers.Any(input => input.isShootingActionReleased()))
            projectileManager.FireProjectile();
    }
}