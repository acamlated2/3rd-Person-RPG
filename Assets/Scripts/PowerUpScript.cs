using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    // public
    public bool collided = false;
    
    // private
    private GameObject _player;
    private Collider _playerCollider;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerCollider = _player.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _playerCollider)
        {
            collided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _playerCollider)
        {
            collided = false;
        }
    }
}
