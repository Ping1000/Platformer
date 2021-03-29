﻿/* CharacterMovement.cs
 * Copyright (C) 2021 Eric Schneider
 * This software is provided 'as-is', without any express or implied warranty. In no event will the author
 * be held liable for any damages arising from the use of this software.
 * Permission is granted to Ping 1000 to use this software for any purpose, including commercial applications,
 * and to alter it and distribute it freely, subject to the following restrictions:
 *   1. If this software is used in a product, commercial or otherwise, attribution *must* be provided in the
 *      product's credits to Eric Schneider, with the sentence "Summit Studios is the best" clearly visible
 *      in the credits page. IF CORRECT ATTRIBUTION IS NOT PROVIDED IN EVERY DISTRIBUTED BUILD OF THE GAME
 *      RELEASED AFTER MARCH 16, 2021, THIS WILL BE TREATED AS PLAGIARISM.
 *   2. This notice may not be removed or altered from any source distribution.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EricCharacterMovement : MonoBehaviour
{

    [SerializeField] private LayerMask whatIsGround;

    // To detect if the player is on the ground, we cast a box at the bottom of the player's collider.
    // However, that box has to have some width and should also not extend the full width of the player
    // (lest the boxcast collides with the wall). This parameter says how far to extend the boxcast vertically
    // as well as how far to retract it horizontally.
    [SerializeField] private float boxcastMargins;

    [Header("Speed Settings")] 
    [SerializeField] private float minJumpHeight = 2f;
    [SerializeField] private float maxJumpHeight = 4.5f;
    [SerializeField] private float gravityScale = 4f;
    [Space]
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float airAcceleration = 30f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] [Range(0f, 1.1f)] private float stopDamping = 0.001f;

    [Header("Extra Stuff")] 
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float bufferTime = 0.2f;


    private Rigidbody2D _rb;
    private Collider2D _collider;
    private GunController _gun;

    private bool grounded;
    private bool spacePressed;
    private float horizontalVelocity = 0.0f;

    private float timeSinceGrounded = Mathf.Infinity;
    private float timeSinceBuffered = Mathf.Infinity;

    private bool facingRight;

    public int lives = 3;

    // Init and Update functions
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _rb.gravityScale = gravityScale;
        facingRight = true;
        _gun = GetComponentInChildren<GunController>();
    }

    private void FixedUpdate()
    {

        _rb.gravityScale = gravityScale;

        //
        // check if grounded
        //
        Bounds colliderPos = _collider.bounds;
        Vector2 point = new Vector2(colliderPos.center.x, colliderPos.min.y);
        Vector2 size = new Vector2(colliderPos.extents.x - boxcastMargins, boxcastMargins);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(point, size, 0f, whatIsGround);
        grounded = false;
        if (colliders.Length > 0)
        {
            grounded = true;
            timeSinceGrounded = 0f;
        };

        //
        // try to jump
        //
        if (timeSinceBuffered < bufferTime && timeSinceGrounded < coyoteTime)
        {
            _rb.velocity = new Vector2(
                _rb.velocity.x,
                Mathf.Sqrt(-2f * Physics2D.gravity.y * gravityScale * maxJumpHeight));
            timeSinceBuffered = Mathf.Infinity;
            timeSinceGrounded = Mathf.Infinity;

            if (!spacePressed)
            {
                _rb.velocity *= new Vector2(1f, Mathf.Sqrt(minJumpHeight / maxJumpHeight));
            }
        }

        //
        // update horizontal velocity
        //
        float a = grounded ? acceleration : airAcceleration;
        float xVel = _rb.velocity.x + a * horizontalVelocity * Time.fixedDeltaTime;

        if (Mathf.Approximately(horizontalVelocity, 0f) && grounded)
        {
            // damp based on stopDamping
            xVel *= Mathf.Pow(stopDamping / 60f, Time.fixedDeltaTime);
        }
        else
        {
            // damp based on maxSpeed
            xVel *= (maxSpeed / (maxSpeed + a * Time.fixedDeltaTime));
        }
        _rb.velocity = new Vector2(xVel, _rb.velocity.y);

    }

    void Update()
    {
        timeSinceBuffered += Time.deltaTime;
        timeSinceGrounded += Time.deltaTime;

        if (gameObject.transform.position.y < -15)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    // Player functions
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Rotate about the y-axis by 180 degrees.
        transform.Rotate(new Vector3(0, 180, 0));
    }

    public void HitPlayer(int damage = 1)
    {
        lives -= damage;
        if (lives <= 0)
        {
            LevelProgressTracker.PlayerDeath();
        }
    }


    // Action functions
    void OnMove(InputValue value)
    {
        Vector2 val = value.Get<Vector2>();
        if ((val.x > 0 && !facingRight) || (val.x < 0 && facingRight)) { Flip(); }
        horizontalVelocity = val.x;
    }
    void OnJump(InputValue value)
    {
        spacePressed = value.isPressed;
        if (value.isPressed)
        {
            timeSinceBuffered = 0f;
        }
        else
        {
            Vector2 vel = _rb.velocity;
            if (vel.y > 0) // only damp if currently jumping
            {
                _rb.velocity *= new Vector2(1f, Mathf.Sqrt(minJumpHeight / maxJumpHeight));
            }
        }
    }
    void OnShoot(InputValue value)
    {
        _gun.Shoot();
    }
    void OnTimeSwap(InputValue value)
    {
        TimeManager.SwapTime();
        // we can take this out if we want
        //if (_gun.canShoot)
        //{
        //    // set cooldown after switching time too
        //    _gun.canShoot = false;
        //    _gun.fireTimer = _gun.fireDelay;
        //}
    }
}
