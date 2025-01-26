using System.Collections.Generic;
using System.Linq;
using Core;
using Player;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private List<IInputHandler> inputHandlers;
    private ProjectileManager projectileManager;

    private void Start()
    {
        inputHandlers = new List<IInputHandler>
        {
            new DesktopInputHandler(),
            new MobileInputHandler()
        };
        projectileManager = ProjectileManager.Instance;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing)
            return;

        inputHandlers.ForEach(it => it.Update(PlayerController.Instance.GetPlayerPosition()));

        var horizontalInput = inputHandlers.Sum(it => it.GetMovementDirection());
        PlayerController.Instance.MovePlayer(horizontalInput);

        if (inputHandlers.Any(input => input.isShootingActionPressed()))
            projectileManager.ChargeProjectile();
        if (inputHandlers.Any(input => input.isShootingActionReleased()))
            projectileManager.FireProjectile();
    }
}