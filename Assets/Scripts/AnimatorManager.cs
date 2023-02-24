using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    // public
    
    // private
    private Animator _animator;
    private int _horizontal;
    private int _vertical;

    private int _moving;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _horizontal = Animator.StringToHash("Velocity X");
        _vertical = Animator.StringToHash("Velocity Z");
        _moving = Animator.StringToHash("Moving");
    }

    private void UpdateAnimatorValue(float horizontalMovement, float verticalMovement)
    {
        // round off values
        float newHorizontal;
        float newVertical;

       // #region horizontal
//
       // if (_horizontal > 0 && _horizontal < 0.55)
       // {
       //     newHorizontal = 0.5f;
       // }
       // else if (_horizontal > 0.55)
       // {
       //     newHorizontal = 1;
       // }
       // else if (_horizontal < 0 && _horizontal > -0.55)
       // {
       //     newHorizontal = -0.5f;
       // }
       // else if (_horizontal < -0.55)
       // {
       //     newHorizontal = -1;
       // }
       // else
       // {
       //     newHorizontal = 0;
       // }
//
       // #endregion
        
        //#region vertical
//
        //if (_vertical > 0 && _vertical < 0.55)
        //{
        //    newVertical = 0.5f;
        //}
        //else if (_vertical > 0.55)
        //{
        //    newVertical = 1;
        //}
        //else if (_vertical < 0 && _vertical > -0.55)
        //{
        //    newVertical = -0.5f;
        //}
        //else if (_vertical < -0.55)
        //{
        //    newVertical = -1;
        //}
        //else
        //{
        //    newVertical = 0;
        //}
//
        //#endregion
        
        _animator.SetFloat(_horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        _animator.SetFloat(_vertical, verticalMovement, 0.1f, Time.deltaTime);
    }

    public void HandleAnimator(Vector2 input)
    {
        if (input.x > 0.1 || input.x < -0.1 || input.y > 0.1 || input.y < -0.1)
        {
            _animator.SetBool(_moving, true);
        }
        else
        {
            _animator.SetBool(_moving, false);
        }
        
        UpdateAnimatorValue(input.x, input.y);
    }
}