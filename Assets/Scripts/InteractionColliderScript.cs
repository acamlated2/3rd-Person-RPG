using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionColliderScript : MonoBehaviour
{
    //// public
    //public bool collided = false;
    //
    //// private
    //private GameObject[] _interactables;
    //private Collider[] _interactablesCollider;

    private void Awake()
    {
        //_interactables = GameObject.FindGameObjectsWithTag("Interactable");
        //for (int i = 0; i < _interactables.Length; i++)
        //{
        //    _interactablesCollider[i] = _interactables[i].GetComponent<Collider>();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other == _playerCollider)
        //{
        //    collided = true;
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other == _playerCollider)
        //{
        //    collided = false;
        //}
    }
}
