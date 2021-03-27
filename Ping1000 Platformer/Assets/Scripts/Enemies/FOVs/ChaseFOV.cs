using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ChaseFOV : EnemyFOV {
    [SerializeField]
    protected EnemyIdle _enemy;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private Transform groundCheck;

    private Collider2D _col;

    private bool isChasing;
    private GameObject chasedObj;
    public float stopDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
            Chase();
        else
            _enemy._atk.EndAttack();
    }

    /// <summary>
    /// Moves horizontally toward the player
    /// </summary>
    void Chase() {
        if (Vector3.Distance(transform.position, chasedObj.transform.position) > stopDistance) {
            // chase unless at an edge
            Collider2D groundCol = Physics2D.OverlapPoint(groundCheck.position, groundMask);
            if (groundCol != null) {
                // not at edge
                _enemy.transform.position = ChasedPosition(_enemy.transform.position);
            }
        } else {
            if (!_enemy._atk.isAttacking)
                _enemy._atk.BeginAttack();
        }
    }

    Vector3 ChasedPosition(Vector3 curPos) {
        if (curPos.x < chasedObj.transform.position.x) {
            curPos.x = curPos.x + _enemy.moveSpeed * Time.deltaTime;
            if (!_enemy.facingRight) {
                _enemy.Flip();
            }
        } else {
            curPos.x = curPos.x - _enemy.moveSpeed * Time.deltaTime;
            if (_enemy.facingRight) {
                _enemy.Flip();
            }
        }
        return curPos;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            chasedObj = collision.gameObject;
            isChasing = true;
            _enemy.isIdle = false;
            _enemy.isAggro = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // need this else will go off of bullet exit
        if (collision.gameObject.CompareTag("Player")) {
            isChasing = false;
            _enemy.isIdle = true;
            _enemy.isAggro = false;
        }
    }
}
