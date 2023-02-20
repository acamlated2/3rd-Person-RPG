using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // public
    public float speed = 10;
    
    public Vector2 look;
    
    // private
    private PlayerControls _controls;
    private Vector2 _move;

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
    }

    private void FixedUpdate()
    {
        // movement
        Vector3 movement = new Vector3(_move.x, 0.0f, _move.y) * speed * Time.deltaTime;
        transform.Translate(movement, Space.Self);
        
        // rotation
        if (look.x > 0.3)
        {
            transform.Rotate(new Vector3(0, 5, 0));
        }
        else if (look.x < -0.3)
        {
            transform.Rotate(new Vector3(0, -5, 0));
        }
    }

    void Update()
    {
        
    }
}
