using System;
using System.Collections.Generic;
using Actors;
using UnityEngine;

using AnimatorState = Actors.AnimatorState;
using Random = UnityEngine.Random;

public class StickmanAnimator : MonoBehaviour, IAnimationStateReader
{
    public event Action<AnimatorState> StateEntered; 
    public event Action<AnimatorState> StateExited;
    
    public bool IsHit { get; private set; }
    
    private readonly int _move = Animator.StringToHash("IsMoving");
    private readonly int _digging = Animator.StringToHash("IsDigging");
    private readonly int _jump = Animator.StringToHash("Jump");
    private readonly int _hit = Animator.StringToHash("Hit");
    private readonly int _winner = Animator.StringToHash("Winner");
    private readonly int _winnerState = Animator.StringToHash("WinnerState");

    public AnimatorState CurrentState { get; private set; }

    [Header("Component references")]
    [SerializeField] private Animator _animator;


    private Dictionary<int, AnimatorState> _animationHashes;

    private void Awake()
    {
        InitAnimationHashtable();
        _animator = GetComponent<Animator>();
    }

    public void HitEvent()
    {
        IsHit = true;
    }

    public void AttackEnded()
    {
        IsHit = false;
    }

    public void Move()
    {
        _animator.SetBool(_move, true);
    }
    

    public void StopMoving()
    {
        _animator.SetBool(_move, false);
    }

    public void OnHit()
    {
        _animator.SetTrigger(_hit);
    }

    public void StartDigging()
    {
        _animator.SetBool(_digging, true);
    }

    public void StopDigging()
    {
        _animator.SetBool(_digging, false);
    }

    public void PlayWinner()
    {
        _animator.SetInteger(_winnerState, Random.Range(0, 3));
        _animator.SetTrigger(_winner);
    }

    public void EnterState(int stateHash)
    {
        CurrentState = StateFor(stateHash);
        StateEntered?.Invoke(CurrentState);
    }

    public void ExitedState(int stateHash)
    {
        StateExited?.Invoke(CurrentState);
    }

    private void InitAnimationHashtable()
    {
        _animationHashes = new Dictionary<int, AnimatorState>();
        
        _animationHashes.Add(Animator.StringToHash("Run"), AnimatorState.Walking);
        _animationHashes.Add(Animator.StringToHash("Digging"), AnimatorState.Digging);
        _animationHashes.Add(Animator.StringToHash("Fight"), AnimatorState.Attack);
        _animationHashes.Add(Animator.StringToHash("Jump"), AnimatorState.Jump);
        _animationHashes.Add(Animator.StringToHash("Idle"), AnimatorState.Idle);
        _animationHashes.Add(Animator.StringToHash("SwingDancing"), AnimatorState.SwingDancing);
        _animationHashes.Add(Animator.StringToHash("ChickenDance"), AnimatorState.ChickenDance);
        _animationHashes.Add(Animator.StringToHash("SambaDancing"), AnimatorState.SambaDancing);
        _animationHashes.Add(Animator.StringToHash("ShoppingCartDance"), AnimatorState.ShoppingCartDance);
    }

    private AnimatorState StateFor(int stateHash)
    {
        return _animationHashes[stateHash];
    }
}
