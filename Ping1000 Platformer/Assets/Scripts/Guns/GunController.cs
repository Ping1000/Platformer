using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunController : MonoBehaviour
{
    [HideInInspector]
    public bool canShoot = false;
    [Tooltip("Prefab object for the bullets to shoot.")]
    public GameObject bulletPrefab;
    [Tooltip("Spawn point for the bullets.")]
    public Transform gunBarrel;
    [Tooltip("Delay between shots.")]
    public float fireDelay;

    [HideInInspector]
    public float fireTimer;

    public abstract void Shoot(string soundPath = "", 
        volumeType volume = volumeType.half);
}
