using System;
using System.Collections.Generic;
using System.Linq;
using Assets.RequiredField.Scripts;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    [SerializeField, RequiredField] public PlayerController Player;
    [SerializeField, RequiredField] public Transform PlayerTransform;

    [SerializeField] private float touchMinDistanceThreshold = 0.2f;
    private List<IInputHandler> inputHandlers;

    private void Start()
    {
        inputHandlers = new List<IInputHandler>
        {
            new DesktopInputHandler(),
            new MobileInputHandler(touchMinDistanceThreshold)
        };
    }

    private void Update()
    {
        inputHandlers.ForEach(it => it.Update(PlayerTransform.position));

        float horizontalInput = inputHandlers.Sum(it => it.GetMovementDirection());
        Player.MovePlayer(horizontalInput);

        if (inputHandlers.Any(input => input.isShootingActionPressed()))
            Player.ShootProjectile();
    }

    // Hook for UI interactions
    public void ShootingActionPressed()
    {
        Player.ShootProjectile();
    }
}