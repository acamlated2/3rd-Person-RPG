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

    public bool poweredUp = false;

    // private
    private PlayerControls _controls;
    private Vector2 _move;
    private Vector3 _moveDirection;
    
    private GameObject _cameraManager;
    private GameObject _camera;

    private float _powerUpTime = 0;
    private float _powerUpTimeDefault = 5;

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

        _cameraManager = GameObject.FindGameObjectWithTag("Camera Manager");
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void FixedUpdate()
    {
        // movement
        HandleRotation();
        HandleMovement();
        
        // power up
        HandlePowerUp();
    }

    private void HandleMovement()
    {
        _moveDirection = _camera.transform.forward * _move.y;
        _moveDirection += _camera.transform.right * _move.x;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        Vector3 movementVelocity = _moveDirection * movementSpeed;
        GetComponent<Rigidbody>().velocity = movementVelocity;
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
        if (poweredUp)
        {
            _powerUpTime -= 1 * Time.deltaTime;
            Debug.Log(_powerUpTime);
        }
    }

    public void StartPowerUp()
    {
        poweredUp = true;
        _powerUpTime = _powerUpTimeDefault;
    }
}
