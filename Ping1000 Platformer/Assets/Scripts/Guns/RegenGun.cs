using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenGun : GunController {
    [Tooltip("The maximum amount of bullets in the gun.")]
    public int maxAmmo;
    public float timeToRecharge;

    public int CurrentAmmo { get; private set; }

    // will probably need this if we want the bullets icons to graduially fill in
    private float nextAmmoPercent;

    // Start is called before the first frame update
    void Start()
    {
        CurrentAmmo = maxAmmo;
        nextAmmoPercent = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentAmmo < maxAmmo) {
            nextAmmoPercent += Time.deltaTime;
            if (nextAmmoPercent >= 1) {
                nextAmmoPercent = 0;
                CurrentAmmo++;
            }
        } else {
            nextAmmoPercent = 0;
        }
    }

    public override void Shoot(string soundPath = "", 
        volumeType volume = volumeType.half) {
        if (CurrentAmmo <= 0)
            return;

        GameObject newBullet = Instantiate(bulletPrefab, gunBarrel.position,
            gunBarrel.rotation, TimeManager.activeTimeParent.transform);
        newBullet.GetComponent<Bullet>().firedBy = transform.parent.gameObject;
        SFXManager.PlayNewSound(soundPath, volume);

        CurrentAmmo--;
    }
}
