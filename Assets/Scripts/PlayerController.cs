using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // public
    public float movementSpeed = 10;
    public float rotationSpeed = 0.1f;
    
    public Vector2 look;

    public bool speedBoosted = false;

    // private
    private PlayerControls _controls;
    private Vector2 _move;
    private Vector3 _moveDirection;

    private float _moveAmount;
    
    private GameObject _cameraManager;
    private GameObject _camera;

    private GameObject[] _speedBoosts;
    private GameObject[] _doubleJumps;

    private float _speedBoostTime = 0;
    private float _speedBoostTimeDefault = 5;
    private float _speedBoostMultiplier = 1;

    private bool _jump;
    private float _jumpMultiplier = 6;

    private AnimatorManager _animatorManager;

    private void OnEnable()
    {
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    void Awake()
    {
        _controls = new PlayerControls();

        _controls.Player.Move.performed += ctx => _move = ctx.ReadValue<Vector2>();
        _controls.Player.Move.canceled += ctx => _move = Vector2.zero;

        _controls.Player.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        _controls.Player.Look.canceled += ctx => look = Vector2.zero;

        _controls.Player.Jump.performed += ctx => _jump = true;
        _controls.Player.Jump.canceled += ctx => _jump = false;

        _cameraManager = GameObject.FindGameObjectWithTag("Camera Manager");
        _camera = GameObject.FindGameObjectWithTag("MainCamera");

        _animatorManager = GetComponent<AnimatorManager>();

        _speedBoosts = GameObject.FindGameObjectsWithTag("PU Speed Boost");
        _doubleJumps = GameObject.FindGameObjectsWithTag("PU Double Jump");
    }

    private void FixedUpdate()
    {
        // movement
        HandleRotation();
        HandleMovement();
        HandleJump();
        
        // animation
        HandleAnimation();
        
        // power up
        HandlePowerUp();
    }

    private void HandleMovement()
    {
        _moveDirection = _camera.transform.forward * _move.y;
        _moveDirection += _camera.transform.right * _move.x;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        Vector3 movementVelocity = _moveDirection * movementSpeed * _speedBoostMultiplier;
        transform.Translate(movementVelocity * Time.deltaTime, Space.World);
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = _camera.transform.forward * _move.y;
        targetDirection += _camera.transform.right * _move.x;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void LateUpdate()
    {
        // camera
        _cameraManager.GetComponent<CameraScript>().HandleCamera(look);
    }

    private void HandlePowerUp()
    {
        // detect power ups
        for (int i = 0; i < _speedBoosts.Length; i++)
        {
            if (_speedBoosts[i].GetComponent<PowerUpScript>().collided)
            {
                StartSpeedBoostPowerUp();
            }
        }

        for (int i = 0; i < _doubleJumps.Length; i++)
        {
            if (_doubleJumps[i].GetComponent<PowerUpScript>().collided)
            {
                StartDoubleJumpPowerUp();
            }
        }
        
        // speed boost
        if (speedBoosted)
        {
            _speedBoostTime -= 1 * Time.deltaTime;

            if (_speedBoostTime <= 0)
            {
                speedBoosted = false;
                _speedBoostMultiplier = 1;
            }
        }
    }

    public void StartSpeedBoostPowerUp()
    {
        speedBoosted = true;
        _speedBoostTime = _speedBoostTimeDefault;
        _speedBoostMultiplier = 2;
    }

    private void StartDoubleJumpPowerUp()
    {
        
    }

    private void HandleJump()
    {
        if (_jump)
        {
            Jumping();
            _jump = false;
        }
    }

    private void Jumping()
    {
        Vector3 jumpVelocity = new Vector3(0, 1, 0);

        jumpVelocity.y = jumpVelocity.y * _jumpMultiplier;
        GetComponent<Rigidbody>().velocity += jumpVelocity;
    }

    private void HandleAnimation()
    {
        //_moveAmount = Mathf.Clamp01(Mathf.Abs(_move.x) + Mathf.Abs(_move.y));
        _animatorManager.HandleAnimator(_move);
    }
}
