using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGun : GunController
{
    [Tooltip("Number of bullets in each burst.")]
    public int bulletsPerBurst = 5;
    [Tooltip("Number of bursts to fire in total.")]
    public int burstsPerShoot = 1;
    [Tooltip("Delay between bursts.")]
    public float burstDelay;
    [Tooltip("Random spread factor.")]
    public float spread;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Shoot(string soundPath = "", 
        volumeType volume = volumeType.half) {
        SendMessage("OnShoot");
        StartCoroutine(Shooting(soundPath, volume));
    }

    IEnumerator Shooting(string soundPath = "",
        volumeType volume = volumeType.half) {
        for (int i = 0; i < burstsPerShoot; i++) {
            float burstTimer = burstDelay;
            SFXManager.PlayNewSound(soundPath, volume);

            for (int j = 0; j < bulletsPerBurst; j++) {
                float fireTimer = fireDelay;
                GameObject newBullet = Instantiate(bulletPrefab, gunBarrel.position,
                    gunBarrel.rotation, TimeManager.activeTimeParent.transform);
                Bullet b = newBullet.GetComponent<Bullet>();
                b.firedBy = transform.parent.gameObject;
                // RANDOM ROTATE
                float bulletSpread = Random.Range(-1 * spread, spread);
                b.GetComponent<Rigidbody2D>().AddForce(b.transform.up * bulletSpread);
                while (fireTimer >= 0) {
                    fireTimer -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            while (burstTimer >= 0) {
                burstTimer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
