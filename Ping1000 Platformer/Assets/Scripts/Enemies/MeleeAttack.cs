﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class MeleeAttack : AttackBehavior
{
    [SerializeField]
    private Collider2D _col;

    // Start is called before the first frame update
    void Start()
    {
        _col = GetComponent<Collider2D>();
        _col.isTrigger = true;
        _col.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void BeginAttack() {
        _col.enabled = true;
    }

    public override void EndAttack() {
        _col.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision) {
        CharacterController2D player = collision.gameObject.
            GetComponent<CharacterController2D>();
        if (player != null) {
            player.HitPlayer(1);
        }
    }
}
