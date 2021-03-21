using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controller structure and some segments from https://github.com/Brackeys/2D-Character-Controller/blob/master/CharacterController2D.cs
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour {
    [SerializeField] private float movementSmoothing = .1f;
    [SerializeField] private float jumpForce = 500f;
    [SerializeField] private Transform[] groundChecks;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private GunController _gun;

    // private List<AudioClip> shatterSounds;
    // [SerializeField]
    // private List<AudioClip> jumpSounds;

    [HideInInspector]
    public int lives = 1;

    Vector3 zeroVector = Vector3.zero;

    // public bool isFragile = true;

    private Rigidbody2D rb;
    // private AudioSource src;

    const float groundedRadius = .19f;
    private bool grounded;
    private bool facingRight;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        // src = GetComponent<AudioSource>();
        facingRight = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            _gun.Shoot();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            // we can take this out if we want
            if (_gun.canShoot) {
                // set cooldown after switching time too
                _gun.canShoot = false;
                _gun.fireTimer = _gun.fireDelay;
                TimeManager.SwapTime();
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate() {
        grounded = IsGrounded();
    }

    public void Move(float move, bool jump, bool fall) {
        Vector3 targetVelocity = new Vector2(move, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVector, movementSmoothing);

        if (jump && grounded) {
            rb.AddForce(new Vector2(0f, jumpForce));
            grounded = false;
            // src.clip = jumpSounds[Random.Range(0, jumpSounds.Count)];
            // src.Play();
        }
        if (fall && rb.velocity.y > 0f) {
            //Debug.Log("Early Fall!");
            //rb.AddForce(new Vector2(0f, -.25f * jumpForce));
            rb.velocity = new Vector2(rb.velocity.x, 0f);//rb.velocity.y/10);

            //Debug.Log(rb.velocity);
        }

        // if sprite is moving in opposite direction that it is facing, call Flip()// If the input is moving the player right and the player is facing left...
        if (move > 0 && !facingRight) {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && facingRight) {
            // ... flip the player.
            Flip();
        }
    }

    private void Flip() {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Rotate about the y-axis by 180 degrees.
        transform.Rotate(new Vector3(0, 180, 0));
    }

    bool IsGrounded() {
        foreach (Transform check in groundChecks) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, groundedRadius, groundMask);
            for (int i = 0; i < colliders.Length; i++) {
                //Debug.Log(colliders[i]);
                if (colliders[i] != gameObject) {
                    return true;
                }
            }
        }

        return false;
    }

    public void HitPlayer(int damage) {
        lives -= damage;
        if (lives < 0) {
            LevelProgressTracker.PlayerDeath();
        }
    }
}