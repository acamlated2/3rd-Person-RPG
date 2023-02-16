using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    // public
    
    // private
    private GameObject player;
    private Vector3 offset;

    private Vector3 currPos;
    private Vector3 desiredPos;
    private Vector3 targetPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        offset = new Vector3(0.0f, 3.0f, -3.0f);
        targetPos = player.transform.position + new Vector3(0, 1.5f, 0);
        transform.LookAt(targetPos);
    }
    
    void FixedUpdate()
    {
        currPos = transform.position;

        float desiredAngle = player.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);
        desiredPos = player.transform.position + (rotation * offset);
        
        // camera movement
        currPos.x = Mathf.Lerp(transform.position.x, desiredPos.x, 10 * Time.deltaTime);
        currPos.y = Mathf.Lerp(transform.position.y, desiredPos.y, 10 * Time.deltaTime);
        currPos.z = Mathf.Lerp(transform.position.z, desiredPos.z, 10 * Time.deltaTime);
        transform.position = currPos;
    }
}
