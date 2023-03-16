using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    // public
    public bool collided = false;
    
    // private
    private GameObject _interactionColliderObject;
    private Collider _interactionCollider;

    private void Awake()
    {
        _interactionColliderObject = GameObject.FindGameObjectWithTag("Interaction Collider");
        _interactionCollider = _interactionColliderObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _interactionCollider)
        {
            collided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == _interactionCollider)
        {
            collided = false;
        }
    }
}
