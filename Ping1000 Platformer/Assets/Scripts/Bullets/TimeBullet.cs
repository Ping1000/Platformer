using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBullet : Bullet
{

    protected override void OnTriggerEnter2D(Collider2D collision) {
        TimeSwappable ts = collision.gameObject.GetComponent<TimeSwappable>();
        if (collision.gameObject.CompareTag("Player")) {
            TimeManager.SwapTime();
            DespawnBullet();
        // } else if (collision.gameObject.CompareTag("Time Swappable")) {
        } else if (ts != null) {
            ts.SwapTime();
            DespawnBullet();
        }
    }
}
