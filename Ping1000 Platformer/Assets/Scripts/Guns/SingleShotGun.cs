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

    public override void Shoot(string soundPath = "", 
        volumeType volume = volumeType.half) {
        if (!canShoot)
            return;

        GameObject newBullet = Instantiate(bulletPrefab, gunBarrel.position,
            gunBarrel.rotation, TimeManager.activeTimeParent.transform);
        newBullet.GetComponent<Bullet>().firedBy = transform.parent.gameObject;
        SFXManager.PlayNewSound(soundPath, volume);

        canShoot = false;
        fireTimer = fireDelay;
    }
}
