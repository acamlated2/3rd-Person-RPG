using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // private
    private bool _opening;


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

            if ((leftPivot.transform.rotation.eulerAngles.y >= 270f) || 
                (leftPivot.transform.rotation.eulerAngles.y == 0f))
            {
                leftPivot.transform.Rotate(0, -9 * Time.deltaTime, 0, Space.Self);
                rightPivot.transform.Rotate(0, 9 * Time.deltaTime, 0, Space.Self);
            }

            else
            {
                _opening = false;
                
                // change camera back
                GameObject cameraManager = GameObject.FindGameObjectWithTag("Camera Manager");
                cameraManager.GetComponent<CameraScript>().StopDoorAnimation();
                
                // allow player to give input
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<PlayerController>().inputBlocked = false;
            }
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
