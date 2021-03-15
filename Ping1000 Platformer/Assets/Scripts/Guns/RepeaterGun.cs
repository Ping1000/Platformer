using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterGun : GunController
{
    private void Start() {
        fireTimer = fireDelay;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (canShoot) {
            if (fireTimer <= 0) {
                Shoot();
                fireTimer = fireDelay;
            } else {
                fireTimer -= Time.fixedDeltaTime;
            }
        }
    }

    public override void Shoot() {
        GameObject newBullet = Instantiate(bulletPrefab, gunBarrel.position,
            gunBarrel.rotation, TimeManager.activeTimeParent.transform);
    }
}
