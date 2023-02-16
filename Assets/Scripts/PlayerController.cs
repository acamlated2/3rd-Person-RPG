using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // public
    public float speed = 10;
    
    // private
    private PlayerControls controls;
    private Vector2 move;

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += ctx => move =
            ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => move = Vector2.zero;
    }

    void SendMessage(Vector2 coordinates)
    {
        Debug.Log("thumb stick coords = " + coordinates);
    }

    private void OnMove(InputValue movementValue)
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(move.x, 0.0f, move.y) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    void Update()
    {
        
    }
}
