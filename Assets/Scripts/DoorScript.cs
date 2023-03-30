using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // private
    private GameObject _hinge;

    private bool _opening;

    private float _animationTimer;
    private float _animationTimerDefault = 5;
    void Awake()
    {
        _hinge = transform.GetChild(1).gameObject;
    }

    
    void Update()
    {
        Animate();
    }

    private void Animate()
    {
        if (_opening)
        {
            GameObject leftPivot = transform.GetChild(0).gameObject;
            GameObject rightPivot = transform.GetChild(1).gameObject;
            Debug.Log(_animationTimer);
            _animationTimer -= 1 * Time.deltaTime;

            if (_animationTimer > 0)
            {
                leftPivot.transform.Rotate(0, -18 * Time.deltaTime, 0, Space.Self);
                rightPivot.transform.Rotate(0, 18 * Time.deltaTime, 0, Space.Self);
            }
            else
            {
                leftPivot.transform.Rotate(0, 0, 0, Space.Self);
                rightPivot.transform.Rotate(0, 0, 0, Space.Self);
                _opening = false;
            }
        }
    }

    public void StartOpening()
    {
        if (!_opening)
        {
            _opening = true;
            _animationTimer = _animationTimerDefault;
        }
    }
}
