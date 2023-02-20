using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    // public
    
    // private
    private GameObject _player;
    private Vector3 _offset;

    private Vector3 _desiredAngle;
    private Vector3 _desiredPos;
    private Vector3 _targetPos;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        _offset = new Vector3(0.0f, 3.0f, -3.0f);

        _desiredAngle = new Vector3();
    }
    
    void FixedUpdate()
    {
        var look = _player.GetComponent<PlayerController>().look;
        
        _desiredAngle.x = _player.transform.eulerAngles.x + look.y * 30;
        _desiredAngle.y = _player.transform.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(_desiredAngle.x, _desiredAngle.y, 0);
        _desiredPos = _player.transform.position + (rotation * _offset);
        
        // camera movement
        transform.position = _desiredPos;
        _targetPos = _player.transform.position + new Vector3(0, 1.5f, 0);
        transform.LookAt(_targetPos);
    }
}
