using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // private
    private GameObject _hinge;

    private bool _opening;
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
            
        }
    }

    public void StartOpening()
    {
        if (!_opening)
        {
            _opening = true;
        }
    }
}
