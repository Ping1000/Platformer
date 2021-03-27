using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class MissileController : MonoBehaviour
{
    public float turnSpeed;
    public float moveSpeed;

    private bool hasExitedBoss; // have we un-collided with the boss?
    private float hasExitedBossTimer = 1.0f;
    private Collider2D _col;
    private Rigidbody2D _rb;
    private Transform trackedTransform; // for now, just make it player

    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _col.isTrigger = true;
        trackedTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasExitedBossTimer <= 0) {
            hasExitedBoss = true;
        } else {
            hasExitedBossTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate() {
        Vector2 direction = (Vector2)trackedTransform.position - _rb.position;
        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        _rb.angularVelocity = -1 * rotateAmount * turnSpeed;
        _rb.velocity = transform.right * moveSpeed;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Boss")) {
            hasExitedBoss = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!hasExitedBoss)
            return;

        GameObject collidedObj = collision.gameObject;
        if (collidedObj.CompareTag("Player")) {
            collidedObj.GetComponent<EricCharacterMovement>().HitPlayer();
            Destroy(gameObject);
        } else if (collidedObj.CompareTag("Boss")) {
            collidedObj.GetComponent<BossController>().HitBoss();
            Destroy(gameObject);
        }
    }
}
