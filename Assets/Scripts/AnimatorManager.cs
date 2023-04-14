using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    // public
    
    // private
    private Animator _animator;
    private int _xControl; // float
    private int _yControl; // float

    private int _moving; // bool

    private int _jump; // trigger
    
    private int _yVelocity; // float

    private int _interact; // trigger

    private int _attack; // trigger

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _xControl = Animator.StringToHash("XControl");
        _yControl = Animator.StringToHash("YControl");
        _moving = Animator.StringToHash("Moving");
        _jump = Animator.StringToHash("Jump");
        _yVelocity = Animator.StringToHash("YVelocity");
        _interact = Animator.StringToHash("Interact");
        _attack = Animator.StringToHash("Attack");

    }

    private void UpdateMovementValue(float horizontalMovement, float verticalMovement)
    {
        _animator.SetFloat(_xControl, horizontalMovement, 0.1f, Time.deltaTime);
        _animator.SetFloat(_yControl, verticalMovement, 0.1f, Time.deltaTime);
    }

    public void HandleAnimator(Vector2 input, float velocityY)
    {
        // check animation state
        AnimatorStateInfo stateInfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Interact") && stateInfo.normalizedTime >= 1f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().interacting = false;
        }

        if (stateInfo.IsName("Attack") && stateInfo.normalizedTime >= 1f)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().attacking = false;
        }

        // movement
        if (input.x > 0.1 || input.x < -0.1 || input.y > 0.1 || input.y < -0.1)
        {
            _animator.SetBool(_moving, true);
        }
        else
        {
            _animator.SetBool(_moving, false);
        }
        
        UpdateMovementValue(input.x, input.y);
        
        // jump
        _animator.SetFloat(_yVelocity, velocityY);
    }

    public void Jump()
    {
        _animator.SetTrigger(_jump);
    }

    public void Interact()
    {
        _animator.SetTrigger(_interact);
    }

    public void Attack()
    {
        _animator.SetTrigger(_attack);
    }
}