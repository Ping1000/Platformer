using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlyingMovement : MonoBehaviour
{
    GameObject target;
    Rigidbody2D _rb;

    [SerializeField] float flySpeed = 2f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float distanceDamping = .15f;
    [SerializeField] float verticalSpeedMultiplier = 1.5f; 
    [SerializeField] float phaseMultiplier = 1.4f;
    [SerializeField] float closestDistToPlayer = 10f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        FlyTowards(target.transform.position);
    }

    private void FlyTowards(Vector3 targetPosition)
    {
        // Desired movement:
        // Fast vertical speed when far from the player to get on the same plane as player
        // Very slow vertical speed when close to the player to allow player to jump over boss
        // avg horizontal speed
        // Very slow speed in all directions when close to the player

        // TODO damp velocity down as approaching the player
        Vector2 targetPos2d = targetPosition;
        Vector2 pos2d = transform.position;
        float distToPlayer = Vector2.Distance(targetPos2d, pos2d);
        Vector2 dir = targetPos2d - pos2d;
        Vector2 dirNorm = dir.normalized;
        //Vector2 velocityVector = new Vector2(dirNorm.x, dirNorm.y * verticalSpeedMultiplier) * flySpeed;
        float speed = Mathf.Clamp(distanceDamping * ((distToPlayer * distToPlayer) - closestDistToPlayer), 0f, maxSpeed);
        Debug.Log(speed);
        Vector2 velocityVector = new Vector2(dirNorm.x * speed, dirNorm.y * speed);
        _rb.velocity = velocityVector;

        return;

    }
}
