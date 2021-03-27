using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BossController : MonoBehaviour
{
    public GameObject missilePrefab;

    private Animator _anim;
    private GunController _gun;

    public int health = 1;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _gun = GetComponentInChildren<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitBoss(int damage = 1) {
        health -= damage;
        if (health <= 0) {
            Destroy(gameObject); // for now
        }
    }

    public void LaunchMissile() {
        GameObject missile = Instantiate(missilePrefab, TimeManager.activeTimeParent);
        // maybe add a "launch position" like with the gun barrels
        missile.transform.position = gameObject.transform.position;
    }

    public void FireGun() {
        _gun.Shoot();
    }
}
