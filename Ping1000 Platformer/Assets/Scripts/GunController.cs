using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [HideInInspector]
    public bool canShoot = false;
    [Tooltip("Prefab object for the bullets to shoot.")]
    public GameObject bulletPrefab;
    [Tooltip("Spawn point for the bullets.")]
    public Transform gunBarrel;
    [Tooltip("Delay between shots.")]
    public float fireDelay;

    private float fireTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        fireTimer = fireDelay;
    }

    // Update is called once per frame
    void FixedUpdate()
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

    public void Shoot() {
        GameObject newBullet = Instantiate(bulletPrefab, gunBarrel.position, 
            gunBarrel.rotation, TimeManager.activeTimeParent.transform);
        Bullet _bul = newBullet.GetComponent<Bullet>();
    }
}
