using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoveablePlatformScript : MonoBehaviour
{
    // public
    public GameObject firstAnchor;
    public GameObject secondAnchor;
    public float moveSpeed;
    
    // private
    private bool _movingToSecondAnchor;

    private Vector3 _dir;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _movingToSecondAnchor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _movingToSecondAnchor = false;
        }
    }

    private void FixedUpdate()
    {
        // get direction
        if (_movingToSecondAnchor)
        {
            GetDirectionTo(secondAnchor);
        }
        else
        {
            GetDirectionTo(firstAnchor);
        }
        
        // move platform
        transform.Translate(_dir * moveSpeed * Time.deltaTime);
    }

    private void GetDirectionTo(GameObject targetObject)
    {
        if (transform.position != targetObject.transform.position)
        {
            _dir = targetObject.transform.position - transform.position;
            _dir = _dir.normalized;
        }
    }
}
