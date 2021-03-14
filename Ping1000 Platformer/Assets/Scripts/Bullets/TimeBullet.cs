using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBullet : Bullet
{

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            TimeManager.SwapTime();
        } else if (collision.gameObject.CompareTag("Time Swappable")) {
            TimeManager.SwapTime(collision.gameObject);
            DespawnBullet();
        }
    }
}
