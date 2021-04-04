using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBullet : Bullet
{
    protected override void OnTriggerEnter2D(Collider2D collision) {
        TimeSwappable ts = collision.gameObject.GetComponent<TimeSwappable>();
        BossController bc = collision.gameObject.GetComponent<BossController>();
        if (collision.gameObject.CompareTag("Player") && !CollidedWithSelf(collision)) {
            TimeManager.SwapTime();
            DespawnBullet();
        // } else if (collision.gameObject.CompareTag("Time Swappable")) {
        } else if (ts != null) {
            ts.SwapTime();
            SFXManager.PlayNewSound("Audio/SFX/Time_gun_impact", volumeType.half);
            DespawnBullet();
        } else if (bc != null) {
            TimeManager.instance.HideBoss();
            SFXManager.PlayNewSound("Audio/SFX/Time_gun_impact", volumeType.half);
            DespawnBullet();
        }
    }
}
