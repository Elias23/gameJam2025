using System;
using System.Collections.Generic;
using System.Linq;
using Assets.RequiredField.Scripts;
using Player;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    [RequiredField]
    [SerializeField] public PlayerController Player;

    [SerializeField] private float touchThreshold = 0.2f;
    private List<IInputHandler> inputHandlers;

    private void Awake()
    {
        inputHandlers = new List<IInputHandler>
        {
            new DesktopInputHandler(),
            new MobileInputHandler(touchThreshold)
        };
    }

    private void Update()
    {
        // Adding up all the horizontal inputs from all the input handlers
        float horizontalInput = inputHandlers.Sum(handler => handler.GetHorizontalInput(transform.position));
        Player.MovePlayer(horizontalInput);

        if (inputHandlers.Any(input => input.isShootingActionPressed()))
        {
           Player.ShootProjectile();
        }
    }

    // Hook for UI interactions
    public void ShootingActionPressed()
    {
        Player.ShootProjectile();
    }
}
