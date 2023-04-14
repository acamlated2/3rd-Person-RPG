using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // public
    public GameObject enemyPrefab;

    public int maxHealth = 3;
    
    public int health = 3;
    public float size = 1;
    
    // private 
    private GameObject _player;
    private GameObject _sphereCollider;
    private GameObject _cubeCollider;

    private bool _chasing;

    private float _jumpingTimer;
    private float _jumpingTimerDefault = 2;

    private float _jumpMultiplier = 6;
    private float _pushMultiplier = 4;
    
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _sphereCollider = transform.GetChild(0).gameObject;
        _cubeCollider = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        // check if player is close enough
        if (_sphereCollider.GetComponent<ChaseColliderScript>().playerFound)
        {
            _chasing = true;
        }
        else
        {
            _chasing = false;
        }

        if (_chasing)
        {
            LookAtPlayer();
            Move();
        }
    }

    private void Move()
    {
        if (_jumpingTimer >= 0)
        {
            _jumpingTimer -= 1 * Time.deltaTime;
        }
        else
        {
            Jump();
            _jumpingTimer = _jumpingTimerDefault;
        }
    }

    private void Jump()
    {
        Vector3 jumpVelocity = new Vector3(0, 1, 1);

        jumpVelocity *= _jumpMultiplier;
        jumpVelocity = transform.TransformDirection(jumpVelocity);
        GetComponent<Rigidbody>().velocity += jumpVelocity;
    }

    private void LookAtPlayer()
    {
        Vector3 direction = _player.transform.position - transform.position;

        Vector3 planeDirection = Vector3.ProjectOnPlane(direction, Vector3.up);

        transform.LookAt(transform.position + planeDirection);
    }

    public void Attack()
    {
        if (_cubeCollider.GetComponent<VulnerabilityScript>().vulnerable)
        {
            // push back
            Vector3 pushVelocity = new Vector3(0, 1, 1);

            pushVelocity *= _pushMultiplier;
            pushVelocity = _player.transform.TransformDirection(pushVelocity);
            GetComponent<Rigidbody>().velocity += pushVelocity;
            
            // do damage
            health -= 1;
            
            // spawn new slime if dead
            if (health <= 0)
            {
                if (maxHealth > 1)
                {
                    GameObject[] newEnemies = new GameObject[2];
                    for (int i = 0; i < newEnemies.Length; i++)
                    {
                        newEnemies[i] = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                    
                        // change health and scale
                        newEnemies[i].GetComponent<EnemyScript>().maxHealth = maxHealth - 1;
                        newEnemies[i].GetComponent<EnemyScript>().health = maxHealth - 1;
                        
                        newEnemies[i].GetComponent<EnemyScript>().size = size / 2;
                        Vector3 scale = new Vector3(size / 2, size / 2, size / 2);
                        newEnemies[i].transform.localScale = scale;
                    }
                }
                
                Destroy(gameObject);
            }
        }
    }
}
