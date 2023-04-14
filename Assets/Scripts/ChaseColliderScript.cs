using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseColliderScript : MonoBehaviour
{
    // public
    public bool playerFound;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Interaction Collider"))
        {
            playerFound = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interaction Collider"))
        {
            playerFound = false;
        }
    }
}
