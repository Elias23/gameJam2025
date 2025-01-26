using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerState : MonoBehaviour
{
    public State state;
    public State oldState;
    private Animator animator;
    public static PlayerState Instance { get; private set; }
    public enum State
    {
        Idle,
        Walk,
        ShootIdle,
        ShootWalk,
        Win
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.Idle;
        oldState = State.Idle;
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (state != oldState)
        {
            animator.CrossFade(state.ToString(), 0.1f);
        }

        oldState = state;
    }
    
    public void isShooting()
    {
        if (state == State.Idle)
        {
            state = State.ShootIdle;
        }
        else if (state == State.Walk)
        {
            state = State.ShootWalk;
        }
    }
    
    public void isWalking()
    {
        if (state == State.Idle)
        {
            state = State.Walk;
        }
        else if (state == State.ShootIdle)
        {
            state = State.ShootWalk;
        }
    }
    
    public void stopShooting()
    {
        if (state == State.ShootIdle)
        {
            state = State.Idle;
        }
        else if (state == State.ShootWalk)
        {
            state = State.Walk;
        }
    }
    
    public void stopWalking()
    {
        if (state == State.Walk)
        {
            state = State.Idle;
        }
        else if (state == State.ShootWalk)
        {
            state = State.ShootIdle;
        }
    }
    
    public void isWin()
    {
        state = State.Win;
    }
}
