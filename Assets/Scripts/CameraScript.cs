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
    
    public bool Animating;
    
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

    private Vector3 _defaultCameraObjectPos;

    private Transform _savedCameraTransform;

    private bool _doorOpening;

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
        if (!Animating)
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
        Animating = true;
        _doorOpening = true;
    }

    public void StopDoorAnimation()
    {
        _doorOpening = false;
    }

    private void DoorAnimation()
    {
        Vector3 cameraPreferredPos;
        Vector3 cameraCurrentPos;
        
        // opening
        if (_doorOpening)
        {
            _savedCameraTransform = _camera.transform;

            GameObject door = GameObject.FindGameObjectWithTag("Door");
            cameraPreferredPos = door.transform.GetChild(2).gameObject.transform.position;
            Vector3 cameraPreferredLookAt = door.transform.GetChild(3).gameObject.transform.position;

            cameraCurrentPos = _camera.transform.position;

            cameraCurrentPos.x = Mathf.Lerp(cameraCurrentPos.x, cameraPreferredPos.x, 0.05f);
            cameraCurrentPos.y = Mathf.Lerp(cameraCurrentPos.y, cameraPreferredPos.y, 0.05f);
            cameraCurrentPos.z = Mathf.Lerp(cameraCurrentPos.z, cameraPreferredPos.z, 0.05f);

            _camera.transform.position = cameraCurrentPos;
            _camera.transform.LookAt(cameraPreferredLookAt);
        }
        else // closing
        {
            cameraPreferredPos = _savedCameraTransform.position;
            cameraCurrentPos = _camera.transform.position;
            
            cameraCurrentPos.x = Mathf.Lerp(cameraCurrentPos.x, cameraPreferredPos.x, 0.01f);
            cameraCurrentPos.y = Mathf.Lerp(cameraCurrentPos.y, cameraPreferredPos.y, 0.01f);
            cameraCurrentPos.z = Mathf.Lerp(cameraCurrentPos.z, cameraPreferredPos.z, 0.01f);
            
            _camera.transform.position = cameraCurrentPos;
            _camera.transform.localRotation = Quaternion.identity;
            
            // stop animating
            Animating = false;
        }
    }

    public void ResetCamera()
    {
        _camera.transform.position = _savedCameraTransform.position;
        _camera.transform.localRotation = Quaternion.identity;
    }
}
