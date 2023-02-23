using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    // private
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider playerCollider = _player.GetComponent<Collider>();

        if (other == playerCollider)
        {
            _player.GetComponent<PlayerController>().StartPowerUp();
        }
    }
}
