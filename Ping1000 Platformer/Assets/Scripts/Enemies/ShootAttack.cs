﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : AttackBehavior
{
    public EnemyIdle _enemy;
    public GunController _gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnShoot() {
        _enemy._anim.SetTrigger("shoot");
    }

    public override void BeginAttack() {
        _gun.canShoot = true;
        isAttacking = true;
    }

    public override void EndAttack() {
        _gun.canShoot = false;
        isAttacking = false;
    }
}
