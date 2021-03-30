using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeaterGun : GunController
{
    string soundPath;
    volumeType volume;

    private void Start() {
        fireTimer = fireDelay;
    }

    // Update is called once per frame
    private void Update()
    {
        if (canShoot) {
            if (fireTimer <= 0) {
                SendMessage("OnShoot");
                GameObject newBullet = Instantiate(bulletPrefab, gunBarrel.position,
                    gunBarrel.rotation, TimeManager.activeTimeParent.transform);
                newBullet.GetComponent<Bullet>().firedBy = transform.parent.gameObject;
                if (soundPath != null && soundPath != "")
                    SFXManager.PlayNewSound(soundPath, volume);

                fireTimer = fireDelay;
            } else {
                fireTimer -= Time.deltaTime;
            }
        }
    }

    public override void Shoot(string soundPath = "", 
        volumeType volume = volumeType.half) {
        canShoot = true;
        this.soundPath = soundPath;
        this.volume = volume;
    }
}
