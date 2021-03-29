using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BossJumpArea : MonoBehaviour
{
    [Tooltip("Upward force applied to the boss when he enters the collider.")]
    public Vector2 jumpForce;
    private Collider2D _col;

    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<Collider2D>();
        _col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        BossController bc;
        if ((bc = collision.GetComponent<BossController>()) && bc.isChasing) {
            bc.GetComponent<Rigidbody2D>().AddForce(jumpForce);
        }
        // Destroy?
    }
}
