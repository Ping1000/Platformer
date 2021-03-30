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

    // used for tracking ammo, not intented to be permanent solution
    private EricCharacterMovement player;

    // Start is called before the first frame update
    void Start()
    {
        CurrentAmmo = maxAmmo;
        nextAmmoPercent = 0;
        player = transform.parent.gameObject.GetComponent<EricCharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentAmmo < maxAmmo) {
            nextAmmoPercent += Time.deltaTime;
            if (nextAmmoPercent >= 1) {
                nextAmmoPercent = 0;
                CurrentAmmo++;
                if (player != null)
                    PlayerInfoCanvas.AddAmmo();
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

        // temporary implementation, but check if the player was the one who shot
        if (player != null) {
            PlayerInfoCanvas.RemoveAmmo();
        }

        SFXManager.PlayNewSound(soundPath, volume);

        CurrentAmmo--;
    }
}
