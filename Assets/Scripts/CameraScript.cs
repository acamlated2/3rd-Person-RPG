using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    // public
    public LayerMask CollisionLayers;
    
    // private
    private GameObject _player;
    private GameObject _cameraPivot;
    private GameObject _camera;

    private Vector3 _followSpeed = Vector3.zero;
    private float _followTime = 0.0f;
    
    private PlayerInputManager _playerInputManager;

    private float _cameraRotateSpeed = 2.5f;
    
    private float _rotateX;
    private float _rotateY;

    private float _minYAngle = -45;
    private float _maxYAngle = 45;

    private float _defaultZPosition;
    private float _cameraCollisionRadius = 2;
    private float _cameraCollisionOffset = 0.2f;
    private float _minimumCollisionOffset = 0.2f;

    private Vector3 _cameraVectorPosition;

    private bool _animating;

    private Vector3 _defaultCameraObjectPos;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _cameraPivot = GameObject.FindGameObjectWithTag("Camera Pivot");
        _camera = GameObject.FindGameObjectWithTag("MainCamera");

        _defaultZPosition = _camera.transform.localPosition.z;
        
        _playerInputManager = FindObjectOfType<PlayerInputManager>();

        _defaultCameraObjectPos = _camera.transform.position;
    }

    public void HandleCamera(Vector2 input)
    {
        if (!_animating)
        {
            FollowPlayer();
            RotateCamera(input);
            CollideCamera();
        }
        else
        {
            DoorAnimation();
        }
    }

    private void FollowPlayer()
    {
        Vector3 playerPosition = Vector3.SmoothDamp(transform.position, _player.transform.position, ref _followSpeed, _followTime);
        transform.position = playerPosition;
    }

    private void RotateCamera(Vector2 input)
    {
        _rotateY += input.x * _cameraRotateSpeed;
        _rotateX += -input.y * _cameraRotateSpeed;
        _rotateX = Mathf.Clamp(_rotateX, _minYAngle, _maxYAngle);

        Vector3 rotation = Vector3.zero;
        rotation.y = _rotateY;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;
        
        rotation = Vector3.zero;
        rotation.x = _rotateX;
        targetRotation = Quaternion.Euler(rotation);
        _cameraPivot.transform.localRotation = targetRotation;
    }

    private void CollideCamera()
    {
        float targetPosition = _defaultZPosition;
        RaycastHit hit;
        Vector3 dir = _camera.transform.position - _cameraPivot.transform.position;
        dir.Normalize();

        if (Physics.SphereCast(_cameraPivot.transform.position, _cameraCollisionRadius, dir, out hit, Mathf.Abs(targetPosition), CollisionLayers))
        {
            float distance = Vector3.Distance(_cameraPivot.transform.position, hit.point);
            targetPosition -= distance + _cameraCollisionOffset;
        }

        if (Mathf.Abs(targetPosition) < _minimumCollisionOffset)
        {
            targetPosition -= _minimumCollisionOffset;
        }

        _cameraVectorPosition.z = Mathf.Lerp(_camera.transform.position.z, targetPosition, 1f);
        _camera.transform.localPosition = _cameraVectorPosition;
    }

    public void StartDoorAnimation()
    {
        _animating = true;
    }

    private void DoorAnimation()
    {
        GameObject door = GameObject.FindGameObjectWithTag("Door");
        Vector3 cameraPreferredPos = door.transform.GetChild(2).gameObject.transform.position;
        Vector3 cameraPreferredLookAt = door.transform.GetChild(3).gameObject.transform.position;

        Vector3 cameraCurrentPos = _camera.transform.position;

        cameraCurrentPos.x = Mathf.Lerp(cameraCurrentPos.x, cameraPreferredPos.x, 0.3f);
        cameraCurrentPos.y = Mathf.Lerp(cameraCurrentPos.y, cameraPreferredPos.y, 0.3f);
        cameraCurrentPos.z = Mathf.Lerp(cameraCurrentPos.z, cameraPreferredPos.z, 0.3f);

        _camera.transform.position = cameraCurrentPos;
        _camera.transform.LookAt(cameraPreferredLookAt);
    }
}
