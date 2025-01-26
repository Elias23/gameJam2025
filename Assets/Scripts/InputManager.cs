using System.Collections.Generic;
using System.Linq;
using Core;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] private float touchMinDistanceThreshold = 0.2f;
    private List<IInputHandler> inputHandlers;
    private ProjectileManager projectileManager;
    private PlayerState playerState;

    private void Start()
    {
        inputHandlers = new List<IInputHandler>
        {
            new DesktopInputHandler(),
            new MobileInputHandler(touchMinDistanceThreshold)
        };
        projectileManager = ProjectileManager.Instance;
        playerState = PlayerState.Instance;
    }

    private void Update()
    {
        if (GameManager.Instance.GameState != GameState.Playing)
            return;

        inputHandlers.ForEach(it => it.Update(PlayerController.Instance.GetPlayerPosition()));

        var horizontalInput = inputHandlers.Sum(it => it.GetMovementDirection());
        if (horizontalInput != 0)
        {
            playerState.isWalking();
        }
        else
        {
            playerState.stopWalking();
        }

        PlayerController.Instance.MovePlayer(horizontalInput);

        if (inputHandlers.Any(input => input.isShootingActionPressed()))
        {
            projectileManager.ChargeProjectile();
            playerState.isShooting();
        }

        if (inputHandlers.Any(input => input.isShootingActionReleased()))
        {
            projectileManager.FireProjectile();
            playerState.stopShooting();
        }
    }
}