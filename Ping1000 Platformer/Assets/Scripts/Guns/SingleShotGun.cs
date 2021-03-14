using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : GunController
{
    private void Start() {
        fireTimer = 0;
    }

    private void FixedUpdate() {
        if (fireTimer <= 0) {
            canShoot = true;
        } else {
            fireTimer -= Time.fixedDeltaTime;
        }
    }

    public override void Shoot() {
        if (!canShoot)
            return;

        GameObject newBullet = Instantiate(bulletPrefab, gunBarrel.position,
            gunBarrel.rotation, TimeManager.activeTimeParent.transform);
        Bullet bullet = newBullet.GetComponent<Bullet>();
        newBullet.GetComponent<Rigidbody2D>().
            velocity = bullet.moveSpeed * transform.right;

        canShoot = false;
        fireTimer = fireDelay;
    }
}
