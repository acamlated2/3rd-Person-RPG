using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    // private
    private GameObject _player;
    
    private bool _retracted;

    private float _extendedScale = 0.1f;
    private float _retractedScale = 0.01f;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (GetComponent<InteractableScript>().collided)
        {
            if (_player.GetComponent<PlayerController>().interacting)
            {
                Debug.Log("interacting");
                _player.GetComponent<PlayerController>().interacting = false;
                Retract();

                GameObject door = GameObject.FindGameObjectWithTag("Door");
                door.GetComponent<DoorScript>().StartOpening();
            }
        }
    }

    private void Retract()
    {
        float yScale = transform.GetChild(1).transform.localScale.y;

        if (!_retracted)
        {
            yScale = Mathf.Lerp(yScale, _retractedScale, 0.5f);
            
            transform.GetChild(1).transform.localScale = new Vector3(0.15f, yScale, 0.15f);
            _retracted = true;
        }
    }

    private void Extend()
    {
        float yScale = transform.GetChild(1).transform.localScale.y;

        if (!_retracted)
        {
            yScale = Mathf.Lerp(yScale, _extendedScale, 0.5f);
            
            transform.GetChild(1).transform.localScale = new Vector3(0.15f, yScale, 0.15f);
            _retracted = false;
        }
    }
}
