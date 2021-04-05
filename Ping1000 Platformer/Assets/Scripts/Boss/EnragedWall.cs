using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Invisible wall that follows the boss when he's in his enraged phase.
/// </summary>
public class EnragedWall : MonoBehaviour
{
    private EricCharacterMovement player;
    private float killCheck = 0f;

    private void Start() {
        player = FindObjectOfType<EricCharacterMovement>();
    }

    private void Update() {
        if (killCheck > 0)
            killCheck -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<EricCharacterMovement>() == player) {
            
            player.HitPlayer();
            Vector2 pushDir = collision.contacts[0].point - (Vector2)transform.position;
            pushDir = pushDir.normalized;
            player.GetComponent<Rigidbody2D>().AddForce(pushDir * 1500);
            killCheck = 0.1f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (killCheck <= 0) {
            player.isInvincible = false;
            player.HitPlayer(1000);
            return;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    if (collision.gameObject == player.gameObject) {
    //        player.HitPlayer(1000);
    //    }
    //}
}
